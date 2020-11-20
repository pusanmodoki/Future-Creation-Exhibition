//作成者 : 植村将太
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvent
{
	/// <summary>
	/// Event Type
	/// </summary>
	public enum EventType
	{
		Default,
		BreakBlock,
		CreateEnemy,
		EnableInfoEdit,
	}

	/// <summary>
	/// Event Hit Character Flags
	/// </summary>
	[System.Flags]
	public enum EventHitCharacters
	{
		/// <summary>Player</summary>
		Player = 0x00000001,
		///// <summary>ルンバ</summary>
		//Helper = 0x00000002,
		///// <summary>チェイサー</summary>
		//Sentinel = 0x00000004,
	}

	/// <summary>
	/// Event Number Flags
	/// </summary>
	[System.Flags]
	public enum EventNumber
	{
		Default = 0x00000001,
		Event01 = 0x00000002,
		Event02 = 0x00000004,
		Event03 = 0x00000008,
		Event04 = 0x00000010,
		Event05 = 0x00000020,
		Event06 = 0x00000040,
		Event07 = 0x00000080,
		Event08 = 0x00000100,
		Event09 = 0x00000200,
		Event10 = 0x00000400,
		Event11 = 0x00000800,
		Event12 = 0x00001000,
		Event13 = 0x00002000,
		Event14 = 0x00004000,
		Event15 = 0x00008000,
		Event16 = 0x00010000,
		Event17 = 0x00020000,
		Event18 = 0x00040000,
		Event19 = 0x00080000,
		Event20 = 0x00100000,
		Event21 = 0x00200000,
		Event22 = 0x00400000,
		Event23 = 0x00800000,
		Event24 = 0x01000000,
		Event25 = 0x02000000,
		Event26 = 0x04000000,
		Event27 = 0x08000000,
		Event28 = 0x10000000,
		Event29 = 0x20000000,
		Event30 = 0x40000000,
	}

	/// <summary>
	/// Event発行に必要なInvokeEvent
	/// </summary>
	[System.Serializable]
	public struct InvokeEvent
	{
		/// <summary>[コンストラクタ]</summary>
		public InvokeEvent(EventType eventType, EventNumber eventNumber)
		{
			m_eventType = eventType;
			m_eventNumber = eventNumber;
		}

		/// <summary>EventType</summary>
		public EventType eventType { get { return m_eventType; } }
		/// <summary>EventNumber</summary>
		public EventNumber eventNumber { get { return m_eventNumber; } }

		/// <summary>EventType</summary>
		[SerializeField, Tooltip("EventType")]
		public EventType m_eventType;
		/// <summary>EventNumber</summary>
		[SerializeField, Tooltip("EventNumber")]
		public EventNumber m_eventNumber;
	}
}