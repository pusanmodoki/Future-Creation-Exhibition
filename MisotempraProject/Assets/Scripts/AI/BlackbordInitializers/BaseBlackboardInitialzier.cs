using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		public abstract class BaseBlackboardInitialzier : MonoBehaviour
		{
			public abstract void InitializeFirstInstance(Blackboard blackboard);
			public abstract void InitializeAllInstance(Blackboard blackboard);
		}
	}
}