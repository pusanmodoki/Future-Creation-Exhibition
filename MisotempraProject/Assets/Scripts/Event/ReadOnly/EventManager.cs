using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameEvent
{
	/// <summary>
	/// Eventを管理するEventManager
	/// </summary>
	public class EventManager : Singleton.DontDestroySingletonMonoBehaviour<EventManager>
	{
		/// <summary>Detection List</summary>
		public static List<EventDetection> eventDetections { get; private set; } = new List<EventDetection>();

		/// <summary>Receive event functions</summary>
		Dictionary<EventType, List<BaseEventFunction>> m_eventFunctions = new Dictionary<EventType, List<BaseEventFunction>>();


		protected override void Init()
		{
		}
		protected override void OnSceneChanged(Scene scene, LoadSceneMode loadSceneMode)
		{
			//感知クラスの除外リストクリア
			EventDetection.hitTransformInstanceIDs.Clear();
			//登録した感知クラスクリア
			eventDetections.Clear();
			//登録した関数削除
			m_eventFunctions.Clear();
		}

		/// <summary>
		/// [AddFunction]
		/// 関数を登録
		/// 引数1: 登録する関数
		/// </summary>
		public void AddFunction(BaseEventFunction function)
		{
			//関数タイプ登録できてなかったら登録する
			if (!m_eventFunctions.ContainsKey(function.receiveEventType))
				m_eventFunctions.Add(function.receiveEventType, new List<BaseEventFunction>());

			//Add
			m_eventFunctions[function.receiveEventType].Add(function);
		}
		/// <summary>
		/// [RemoveFunction]
		/// 関数を削除
		/// 引数1: 登録する関数
		/// </summary>
		public void RemoveFunction(BaseEventFunction function)
		{
			//登録できてなかったら終了
			if (!m_eventFunctions.ContainsKey(function.receiveEventType)) return;

			//登録できてるか検索
			for (int i = 0; i < m_eventFunctions[function.receiveEventType].Count;)
			{
				//見つけたら削除
				if (m_eventFunctions[function.receiveEventType][i].GetInstanceID() == function.GetInstanceID())
					m_eventFunctions[function.receiveEventType].RemoveAt(i);
				else
					++i;
			}
		}

		/// <summary>
		/// [EventDetectionCallback]
		/// イベントを検知すると呼ばれるコールバック
		/// </summary>
		public void EventDetectionCallback(EventPoint eventPoint, GameObject thisObject)
		{
			//Debug Only, 呼べたかな？
#if UNITY_EDITOR
			bool dIsCalled = false;
#endif

			//メッセージ送信
			if (eventPoint.isSendEventMessage)
			{
				//呼んだリスト
				List<int> calledFunctions = new List<int>();

				//メッセージ送信ループ
				foreach (var eventMessage in eventPoint.invokeEventMessages)
				{
					//登録リストにイベントタイプが存在
					if (m_eventFunctions.ContainsKey(eventMessage.eventType))
					{
						//イベント検索ループ
						foreach (var eventFunction in m_eventFunctions[eventMessage.eventType])
						{
							//呼び出しフラグ存在->Callback
							if (eventMessage.eventNumber.HasFlag(eventFunction.receiveEventNumber))
							{
								eventFunction.EventCallback(eventPoint.eventObject, thisObject,
									eventPoint.parameter1, eventPoint.parameter2, eventPoint.parameter3);

								calledFunctions.Add(eventFunction.instanceEventID);

								//Debug Only, フラグ操作
#if UNITY_EDITOR
								dIsCalled = true;
								Debug.Log("Event Invoke. EventPoint Name : " + eventPoint.name + ", Event Type : " + eventMessage.eventType
									+ ", Event Name : " + eventFunction.name);
#endif
							}
						}
					}
				}
			}

			//invoke->eventPoint.invokeEvents loop
			foreach (var eventPointFunction in eventPoint.invokeEvents)
			{
				//Callback
				eventPointFunction.EventCallback(eventPoint.eventObject, thisObject,
					eventPoint.parameter1, eventPoint.parameter2, eventPoint.parameter3);

				//Debug Only, フラグ操作
#if UNITY_EDITOR
				dIsCalled = true;
				Debug.Log("EventPoint->InvokeEvents Invoke. EventPoint Name : " + eventPoint.name + ", Event Name : " + eventPointFunction.name);
#endif
			}

			//呼べなかった場合はError
#if UNITY_EDITOR
			if (!dIsCalled)
			{
				string eventNames = eventPoint.invokeEvents.Count > 0 ? "" : "null";
				string enumNames = eventPoint.invokeEventMessages.Count > 0 ? "" : "null";

				foreach (var e in eventPoint.invokeEvents)
					eventNames += e.name + ", ";
				foreach (var e in eventPoint.invokeEventMessages)
					enumNames += e + ", ";

				Debug.LogWarning("Event Not callBack. Event Name : " + eventPoint.name +
					"Invoke Events : " + eventNames + ", Message Types : " + enumNames);
			}
#endif
		}
	}
}