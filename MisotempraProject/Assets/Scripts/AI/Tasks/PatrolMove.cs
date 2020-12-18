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
			public class PatrolMove : BaseTask
			{
				public class PatrolInfo
				{
					public PatrolInfo(AIPatrolPoints.PointResult result, bool isArrival)
					{
						this.result = result;
						this.isArrival = isArrival;
					}
					public AIPatrolPoints.PointResult result = null;
					public bool isArrival = false;
				}

				[SerializeField]
				float m_arrivalDistance = 1.0f;
				[SerializeField]
				string m_arrivalPointBlackboradKey = "";

				PatrolInfo m_patrolInfo = null;
				Vector3 m_target = Vector3.zero;

				public override void OnCreate()
				{
				}
				public override void FixedUpdate()
				{
				}

				public override EnableResult OnEnale()
				{
					m_patrolInfo = blackboard.GetValue<PatrolInfo>(m_arrivalPointBlackboradKey);
					if (m_patrolInfo == null || aiAgent.group == null) return EnableResult.Failed;

					if (m_patrolInfo.isArrival)
						m_patrolInfo = new PatrolInfo(aiAgent.group.patrolPoints.NextPoint(m_patrolInfo.result, aiAgent.transform.position), false);

					aiAgent.SwitchMoveAgent();
					navMeshAgent.SetDestination(m_patrolInfo.result.position);
					m_target = m_patrolInfo.result.position;
					m_target.y = 0.0f;
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
						m_patrolInfo.isArrival = true;
						blackboard.SetValue(m_arrivalPointBlackboradKey, m_patrolInfo);
						return UpdateResult.Success;
					}

					return UpdateResult.Run;
				}
			}
		}
	}
}