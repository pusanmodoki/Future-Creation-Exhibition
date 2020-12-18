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
			public class IsDeath : BaseDecorator
			{
				public override bool IsPredicate(AIAgent agent, Blackboard blackboard)
				{
					return !agent.status.isAlive;
				}
			}
		}
	}
}