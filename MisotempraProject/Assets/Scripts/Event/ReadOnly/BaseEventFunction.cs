//作成者 : 植村将太
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvent
{
	/// <summary>
	/// Event処理関数のBaseとなるBaseEventFunction class
	/// </summary>
	public abstract class BaseEventFunction : MonoBehaviour
	{
		///<summary>自身が所属するEventPoint</summary>
		public EventPoint myPoint { get; set; } = null;
		///<summary>this EventType</summary>
		public EventType receiveEventType { get { return m_receiveEventType; } }
		///<summary>this EventNumber</summary>
		public EventNumber receiveEventNumber { get { return m_receiveEventNumber; } }
		///<summary>this event instance id</summary>
		public int instanceEventID { get; private set; } = -1;

		///<summary>Instance id counter</summary>
		static int m_instanceIDCounter = 0;

		///<summary>メッセージ受信設定</summary>
		[SerializeField, Tooltip("メッセージ受信設定")]
		bool m_isReceiveMessage = false;
		///<summary>受信するEventType</summary>
		[SerializeField, Tooltip("受信するEventType")]
		EventType m_receiveEventType = EventType.Default;
		///<summary>受信するEventNumber</summary>
		[SerializeField, Tooltip("受信するEventNumber")]
		EventNumber m_receiveEventNumber = EventNumber.Default;

		//Debug Only
#if UNITY_EDITOR
		/// <summary>OnValidate用OldBuf</summary>
		EventType m_oldReceiveEventType = EventType.Default;
		/// <summary>OnValidate用OldBuf</summary>
		bool m_isOldReceiveMessage = false;
#endif

		/// <summary>
		/// [EventCallback]
		/// イベント発生時に行われるコールバック
		/// 引数1: 呼び出しクラスの識別名
		/// 引数2: イベントオブジェクト名
		/// 引数3: イベントタグ
		/// 引数4: イベント番号
		/// </summary>
		public abstract void EventCallback(GameObject eventObject, GameObject detectionObject,
			GameObject parameter1, string parameter2, long parameter3);

		/// <summary>[Start]</summary>
		protected void Start()
		{
			//Debug Only, 保存
#if UNITY_EDITOR
			m_oldReceiveEventType = m_receiveEventType;
			m_isOldReceiveMessage = m_isReceiveMessage;
#endif

			//Receive設定の場合Managerに登録
			if (m_isReceiveMessage)
				EventManager.instance.AddFunction(this);
			//InstanceID設定
			if (instanceEventID == -1)
				instanceEventID = m_instanceIDCounter++;
		}

		//Debug Only
#if UNITY_EDITOR
		/// <summary>[OnValidate]</summary>
		protected void OnValidate()
		{
			//Managerがいなければ終了
			if (EventManager.instance == null) return;

			//Not Receive->Receive = 登録
			if ((m_isReceiveMessage ^ m_isOldReceiveMessage) & m_isReceiveMessage)
				EventManager.instance.AddFunction(this);
			//Receive->Not Receive = 解除
			else if ((m_isReceiveMessage ^ m_isOldReceiveMessage) & m_isOldReceiveMessage)
				EventManager.instance.RemoveFunction(this);
			//Receive != Receive = 解除 & 登録
			else if (m_oldReceiveEventType != m_receiveEventType)
			{
				var temp = m_receiveEventType;
				m_receiveEventType = m_oldReceiveEventType;
				EventManager.instance.RemoveFunction(this);
				m_receiveEventType = temp;
				EventManager.instance.AddFunction(this);
			}

			//保存
			m_oldReceiveEventType = m_receiveEventType;
			m_isOldReceiveMessage = m_isReceiveMessage;
		}
#endif

		/// <summary>[OnDestroy]</summary>
		protected void OnDestroy()
		{
			//Receive設定の場合Remove
			if (m_isReceiveMessage && EventManager.instance)
				EventManager.instance.RemoveFunction(this);
		}
	}
}