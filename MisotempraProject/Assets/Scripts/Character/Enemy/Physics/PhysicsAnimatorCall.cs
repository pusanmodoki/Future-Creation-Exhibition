using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
	namespace Enemy
	{
		public class PhysicsAnimatorCall : MonoBehaviour
		{
			[SerializeField]
			AI.AIAgent m_aiAgent = null;
			[SerializeField]
			string m_blackboardEndAttackAnimationKey = "EndAttackAnimation";
			
			public void EndAttackAnimation()
			{
				m_aiAgent.behaviorTree.blackboard.SetValue(m_blackboardEndAttackAnimationKey, 1);
			}
		}
	}
}
