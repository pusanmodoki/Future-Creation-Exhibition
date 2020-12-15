using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		namespace Decorator
		{
			[System.Serializable]
			public class ToPlayerDistance : BaseDecorator
			{
				[SerializeField]
				string m_targetTransformBlackboardKey = "";
				[SerializeField]
				float m_trueDistance = 1.0f;
				[SerializeField]
				bool isNear = false;

				public override bool IsPredicate(AIAgent agent, Blackboard blackboard)
				{
					return m_targetTransformBlackboardKey != null && m_targetTransformBlackboardKey.Length > 0 ?
						(blackboard.transforms[m_targetTransformBlackboardKey].position
						- agent.transform.position).sqrMagnitude > (m_trueDistance * m_trueDistance) ^ isNear : false;
				}
			}
		}
	}
}