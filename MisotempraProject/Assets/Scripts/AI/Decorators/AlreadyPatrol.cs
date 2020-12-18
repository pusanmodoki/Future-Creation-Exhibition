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
			public class AlreadyPatrol : BaseDecorator
			{
				public override bool IsPredicate(AIAgent agent, Blackboard blackboard)
				{
					if (agent.behaviorTree.nowTask == null)
						return false;
					else if (agent.behaviorTree.nowTask.task is Task.PatrolMove)
						return true;
					
					{
						var cast = agent.behaviorTree.nowTask.task as Task.ToPatrolPoint;
						if (cast != null && cast.isArrival) return true;
					}

					foreach(var e in agent.behaviorTree.nowTask.parentNode.childrenNodes)
					{
						var task = e as Node.TaskNode;
						if (task != null && task.task is Task.PatrolMove)
							return true;
					}
					return false;
				}
			}
		}
	}
}