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
			public class DiscoverPlayer : BaseDecorator
			{
				[SerializeField, Tooltip("ビット反転")]
				bool m_isBitInversion = false;

				public override bool IsPredicate(AIAgent agent, Blackboard blackboard)
				{
					return agent.vision ? agent.vision.isDiscoverStay ^ m_isBitInversion: false;
				}
			}
		}
	}
}