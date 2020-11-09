using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		public abstract class BaseService
		{
			public float callInterval { get; private set; } = 0.0f;
			Timer m_timer = new Timer();

			public void LoadInterval(float callInterval) { this.callInterval = callInterval; }

			public void OnEnable() { m_timer.Start(); }
			public void Update(AIAgent agent, Blackboard blackboard)
			{
				if (m_timer.elapasedTime >= callInterval)
				{
					m_timer.Start();
					ServiceFunction(agent, blackboard);
				}
			}

			public abstract void ServiceFunction(AIAgent agent, Blackboard blackboard);
			public abstract BaseService ReturnNewThisClass();
		}
	}
}