using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damage
{
	public enum DamageType
	{
		Physical,
		Energy
	}

	public enum AttackType
	{
		Weak,
		Middle,
		Strong
	}

	/// <summary>
	/// リクエストの中身となるRequestQueue
	/// </summary>
	[System.Serializable]
	public struct RequestQueue
	{
		[System.Serializable]
		public struct Details
		{
			public Details(DamageType damageType, AttackType attackType)
			{
				this.damageType = damageType;
				this.attackType = attackType;
			}
			[Tooltip("Damage type")]
			public DamageType damageType;
			[Tooltip("Attack type")]
			public AttackType attackType;
		}

		public RequestQueue(GameObject attackObject, float attack, Details details, int attackID)
		{
			m_attackObject = attackObject;
			m_attack = attack;
			m_details = details;
			m_attackID = attackID;
		}
		public RequestQueue(RequestQueue copy)
		{
			m_attackObject = copy.attackObject;
			m_attack = copy.attack;
			m_details = copy.details;
			m_attackID = copy.m_attackID;
		}

		public GameObject attackObject { get { return m_attackObject; } }
		public float attack { get { return m_attack; } }
		public Details details { get { return m_details; } }
		public int attackID { get { return m_attackID; } }

		/// <summary>攻撃するオブジェクト</summary>
		[SerializeField, Tooltip("攻撃するオブジェクト"), NonEditable]
		GameObject m_attackObject;
		/// <summary>攻撃力</summary>
		[SerializeField, Tooltip("攻撃力"), NonEditable]
		float m_attack;
		[SerializeField, Tooltip("Details"), NonEditable]
		Details m_details;
		[SerializeField, Tooltip("ID"), NonEditable]
		int m_attackID;
	}

	[System.Serializable]
	public class AttackInfo
	{
		public float attack { get { return m_attack; } }
		public RequestQueue.Details details { get { return m_details; } }
		public string key { get { return m_key; } }
		public float attackScale { get; private set; } = 1.0f;
		public int id { get; private set; } = -1;
		public bool isEnableAction { get { return id >= 0; } }

		public AttackInfo(string key, float attack, RequestQueue.Details details)
		{
			m_attack = attack;
			m_details = details;
			m_key = key;
		}
		public void SetID(int id)
		{
			this.id = id;
		}
		public void SetScale(float scale)
		{
			attackScale = scale;
		}
		public void Awake()
		{
			attackScale = 1.0f;
			id = -1;
		}

		[SerializeField, Tooltip("Key")]
		string m_key;
		/// <summary>攻撃力</summary>
		[SerializeField, Tooltip("攻撃力")]
		float m_attack;
		[SerializeField, Tooltip("Details")]
		RequestQueue.Details m_details;
	}
}