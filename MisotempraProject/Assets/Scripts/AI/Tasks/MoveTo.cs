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
			public class MoveTo : BaseTask
			{
				[SerializeField]
				string m_targetTransformBlackboardKey = "";
				[SerializeField]
				float m_moveTime = 0.2f;
				[SerializeField]
				float m_arrivalDistance = 2.3f;

				Transform m_target = null;
				Timer m_timer = new Timer();

				public override void FixedUpdate()
				{
				}

				public override EnableResult OnEnale()
				{
					m_timer.Start();
					m_target = blackboard.transforms[m_targetTransformBlackboardKey];
					aiAgent.SwitchMoveAgent();
					navMeshAgent.SetDestination(m_target.position);
					return EnableResult.Success;
				}

				public override void OnQuit(UpdateResult result)
				{
				}

				public override UpdateResult Update()
				{
					Vector3 position = aiAgent.transform.position, toPosition = m_target.position;
					position.y = toPosition.y = 0.0f;

					if (m_timer.elapasedTime >= m_moveTime 
						|| (toPosition - position).sqrMagnitude < m_arrivalDistance * m_arrivalDistance)
					{
						navMeshAgent.ResetPath();
						return UpdateResult.Success;
					}

					return UpdateResult.Run;
				}
			}
		}
	}
}