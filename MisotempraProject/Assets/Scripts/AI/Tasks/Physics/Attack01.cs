using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		namespace Task
		{
			namespace Physics
			{
				[System.Serializable]
				public class Attack01 : BaseTask
				{
					[SerializeField]
					string m_targetTransformBlackboardKey = "";
					[SerializeField]
					string m_animationAttackKey = "";

					Transform m_target = null;
					int m_animationID = -1;
					int m_keyHash = 0;

					public override void FixedUpdate()
					{
					}

					public override EnableResult OnEnale()
					{
						m_target = blackboard.transforms[m_targetTransformBlackboardKey];

						m_animationID = Animator.StringToHash(m_animationAttackKey);
						(blackboard.GetValue<Animator>("Animator")).SetTrigger(m_animationID);

						aiAgent.SwitchMoveRigidbody();
						m_keyHash = ("EndAttackAnimation").GetHashCode();

						return EnableResult.Success;
					}

					public override void OnQuit(UpdateResult result)
					{
					}

					public override UpdateResult Update()
					{
						Vector3 position = aiAgent.transform.position, toPosition = m_target.position;
						position.y = toPosition.y = 0.0f;

						aiAgent.transform.rotation = Quaternion.Slerp(aiAgent.transform.rotation, Quaternion.LookRotation((toPosition - position).normalized), 2 * Time.deltaTime);

						if (blackboard.GetValue<int>(m_keyHash) == 1)
						{
							blackboard.SetValue<int>(m_keyHash, 0);
							return UpdateResult.Success;
						}
						return UpdateResult.Run;
					}
				}
			}
		}
	}
}