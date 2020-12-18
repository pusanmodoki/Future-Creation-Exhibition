using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		namespace Task
		{
			[System.Serializable]
			public class ToPatrolPoint : BaseTask
			{
				public bool isArrival { get; private set; } = false;

				[SerializeField]
				float m_arrivalDistance = 1.0f;
				[SerializeField]
				string m_arrivalPointBlackboradKey = "";

				AI.AIPatrolPoints.PointResult m_result = null;
				Vector3 m_target = Vector3.zero;

				public override void FixedUpdate()
				{
				}

				public override EnableResult OnEnale()
				{
					isArrival = false;
					blackboard.SetValue <PatrolMove.PatrolInfo>(m_arrivalPointBlackboradKey, null);
					if (aiAgent.group == null) return EnableResult.Failed;

					m_result = aiAgent.group.patrolPoints.ShortestPoint(aiAgent.transform.position);
					m_target = new Vector3(m_result.position.x, 0.0f, m_result.position.x);
					aiAgent.SwitchMoveAgent();
					aiAgent.navMeshAgent.SetDestination(m_result.position);

					return EnableResult.Success;
				}

				public override void OnQuit(UpdateResult result)
				{
				}

				public override UpdateResult Update()
				{
					var position = aiAgent.transform.position;
					position.y = 0.0f;
					if ((m_target - position).sqrMagnitude < m_arrivalDistance * m_arrivalDistance)
					{
						isArrival = true;
						blackboard.SetValue(m_arrivalPointBlackboradKey, new PatrolMove.PatrolInfo(m_result, false));
						return UpdateResult.Success;
					}

					return UpdateResult.Run;
				}
			}
		}
	}
}