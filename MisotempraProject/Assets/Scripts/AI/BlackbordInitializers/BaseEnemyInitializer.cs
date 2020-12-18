using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		namespace BlackBoardInitializer
		{
			public class BaseEnemyInitializer : BaseBlackboardInitializer
			{
				[SerializeField]
				string m_playerTransformKey = "PlayerTransform";
				[SerializeField]
				string m_playerObjectName = "Player";

				public override void InitializeAllInstance(Blackboard blackboard)
				{
					blackboard.transforms[m_playerTransformKey] = GameObject.Find(m_playerObjectName).transform;
				}

				public override void InitializeFirstInstance(Blackboard blackboard)
				{
				}
			}
		}
	}
}
