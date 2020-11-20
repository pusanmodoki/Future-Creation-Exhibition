//作成者 : 植村将太
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvent
{
	/// <summary>
	/// EventPoint用識別子となるEventPoint class
	/// </summary>
	public class EventPoint : MonoBehaviour
	{
		///<summary>Invoke Events</summary>
		public List<BaseEventFunction> invokeEvents { get { return m_invokeEvents; } }
		///<summary>Invoke Event Messages</summary>
		public List<InvokeEvent> invokeEventMessages { get { return m_invokeEventMessages; } }
		///<summary>Event Hit Characters Mask</summary>
		public EventHitCharacters eventHitCharacters { get { return m_eventHitCharacters; } }
		///<summary>Event Object</summary>
		public GameObject eventObject { get { return gameObject; } }
		///<summary>Callback argument 1</summary>
		public GameObject parameter1 { get { return m_parameter1; } }
		///<summary>Callback argument 2</summary>
		public string parameter2 { get { return m_parameter2; } }
		///<summary>Callback argument 3</summary>
		public long parameter3 { get { return m_parameter3; } }
		///<summary>Is Enabled Event</summary>
		public bool isEnabledEvent { get { return m_isEnabledEvent; } set { m_isEnabledEvent = value; } }
		///<summary>Is Send Event Message</summary>
		public bool isSendEventMessage { get { return m_isSendEventMessage; } set { m_isSendEventMessage = value; } }
		///<summary>Is Force Repeated Event</summary>
		public bool isForceRepeated { get { return m_isForceRepeated; } }

		///<summary>Event Hit Characters Mask</summary>
		[SerializeField, Tooltip("Event Hit Characters Mask"), EnumFlags]
		EventHitCharacters m_eventHitCharacters = EventHitCharacters.Player;
		///<summary>Callback argument 1</summary>
		[SerializeField, Tooltip("Callback argument 1 (not 'this')")]
		GameObject m_parameter1 = null;
		///<summary>Callback argument 2</summary>
		[SerializeField, Tooltip("Callback argument 2")]
		string m_parameter2 = "";
		///<summary>Callback argument 3</summary>
		[SerializeField, Tooltip("Callback argument 3")]
		long m_parameter3 = 0;
		///<summary>Is Enabled Event</summary>
		[SerializeField, Tooltip("Is Enabled Event")]
		bool m_isEnabledEvent = true;
		///<summary>Is Enabled Event</summary>
		[SerializeField, Tooltip("Is Send Event Message")]
		bool m_isSendEventMessage = false;
		///<summary>Is Force Repeated Event</summary>
		[SerializeField, Tooltip("Is Force Repeated Event")]
		bool m_isForceRepeated = false;
		///<summary>Invoke Events</summary>
		[SerializeField, Tooltip("Invoke Events")]
		List<BaseEventFunction> m_invokeEvents = new List<BaseEventFunction>();
		///<summary>Invoke Event Messages</summary>
		[SerializeField, Tooltip("Invoke Event Messages")]
		List<InvokeEvent> m_invokeEventMessages = new List<InvokeEvent>();

		/// <summary>[Awake]</summary>
		void Awake()
		{
			//MyPoint登録
			foreach (var e in m_invokeEvents)
				e.myPoint = this;
		}
	}
}