using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	[DisallowMultipleComponent, RequireComponent(typeof(AIAgent)), RequireComponent(typeof(Damage.DamageController))]
	public class AIStatus : MonoBehaviour
	{
		/// <summary>is Alive</summary>
		public bool isAlive { get { return (m_hp > 0.0f); } }

		[SerializeField]
		Damage.DamageController m_damageController = null;
		[SerializeField]
		AIAgent m_aiAgent = null;
		[SerializeField]
		Animator m_animator = null;
		[SerializeField]
		float m_hp = 0.0f;

		void Awake()
		{
			m_aiAgent.SetAIStatus(this);	
		}

		/// <summary>
		/// [Damage]
		/// HP - attack
		/// return: isAlive
		/// 引数1: attack value
		/// </summary>
		public bool Damage(float attack)
		{
			m_hp -= attack;
			return isAlive;
		}
		/// <summary>
		/// [EnabledAttack]
		/// Enabled Attack Flags
		/// 引数1: Enabled attack key
		/// 引数2: Enabled attack scale, default = 1.0f
		/// </summary>
		public void EnabledAttack(string attackKey, float attackScale = 1.0f)
		{
			m_damageController.EnableAction(attackKey, attackScale);
		}
		/// <summary>
		/// [DisabledAttack]
		/// 引数1: Disabled attack key
		/// </summary>
		public void DisabledAttack(string attackKey)
		{
			m_damageController.DisableAction(attackKey);
		}
	}
}