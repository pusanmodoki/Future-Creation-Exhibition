using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		public abstract class BaseService
		{
			public enum CallMode
			{
				TimeInterval,
				
			}

			public float callInterval { get; private set; } = 0.0f;
			Timer m_timer = new Timer();

			public void OnEnable() { m_timer.Start(); }
			public void Update()
			{
				if (m_timer.elapasedTime >= callInterval)
				{
					m_timer.Start();
					ServiceFunction();
				}
			}

			public abstract void ServiceFunction();
		}
	}
}