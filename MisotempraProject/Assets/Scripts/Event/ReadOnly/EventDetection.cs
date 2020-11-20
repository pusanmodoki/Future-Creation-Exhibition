//作成者 : 植村将太
using System.Collections.Generic;
using UnityEngine;

namespace GameEvent
{
	/// <summary>
	/// イベントを検知するEventDetection class
	/// </summary>
	public class EventDetection : MonoBehaviour
	{
		/// <summary>
		/// [EventDetectionCallback]
		/// Eventにヒットした場合Callされる
		/// 引数1: 呼び出しクラスの識別名
		/// 引数2: イベントオブジェクト名
		/// 引数3: イベントタグ
		/// </summary>
		public delegate void EventDetectionCallback(EventPoint eventPoint, GameObject thisObject);

		///<summary>除外リスト</summary>
		public static List<int> hitTransformInstanceIDs { get; private set; } = new List<int>();

		///<summary>This Type</summary>
		[SerializeField, Tooltip("This Type")]
		EventHitCharacters m_detectionType = EventHitCharacters.Player;
		///<summary>EventPoint LayerMask</summary>
		[SerializeField, Tooltip("EventPoint LayerMask")]
		LayerMaskEx m_eventPointLayer = int.MaxValue;
		///<summary>Event Call Exclude Once Hit?</summary>
		[SerializeField, Tooltip("Event Call Exclude Once Hit?")]
		bool m_isExcludeOnceHit = true;

		///<summary>EventDetection Callback</summary>
		EventDetectionCallback m_eventDetectionCallback = null;

		/// <summary>[Start]</summary>
		void Start()
		{
			EventManager.eventDetections.Add(this);
			m_eventDetectionCallback += EventManager.instance.EventDetectionCallback;
		}

		/// <summary>[OnCollisionEnter]</summary>
		private void OnCollisionEnter(Collision other)
		{
			//Hit
			if (m_eventPointLayer.EqualBitsForGameObject(other.gameObject))
			{
				//自分が発行できる？
				var eventPoint = other.collider.GetComponent<EventPoint>();
				if (eventPoint == null || !eventPoint.isEnabledEvent
					|| !eventPoint.eventHitCharacters.HasFlag(m_detectionType)) return;
				//呼び出しコスト削減
				int instanceID = other.transform.GetInstanceID();

				//呼び出し可能ならコールバック
				if (!hitTransformInstanceIDs.Contains(instanceID))
				{
					//Callback
					m_eventDetectionCallback?.Invoke(eventPoint, gameObject);
					//除外？
					if (m_isExcludeOnceHit & !eventPoint.isForceRepeated)
						hitTransformInstanceIDs.Add(instanceID);
				}
			}
		}

		/// <summary>[OnTriggerEnter]</summary>
		void OnTriggerEnter(Collider other)
		{
			//Hit
			if (m_eventPointLayer.EqualBitsForGameObject(other.gameObject))
			{
				//自分が発行できる？
				var eventPoint = other.GetComponent<EventPoint>();
				if (eventPoint == null || !eventPoint.isEnabledEvent || !eventPoint.eventHitCharacters.HasFlag(m_detectionType)) return;

				//呼び出しコスト削減
				int instanceID = other.transform.GetInstanceID();
				//呼び出し可能ならコールバック
				if (!hitTransformInstanceIDs.Contains(instanceID))
				{
					//Callback
					m_eventDetectionCallback?.Invoke(eventPoint, gameObject);
					//除外？
					if (m_isExcludeOnceHit & !eventPoint.isForceRepeated)
						hitTransformInstanceIDs.Add(instanceID);
				}
			}
		}
	}
}