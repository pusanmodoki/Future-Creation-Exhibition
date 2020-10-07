using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		public abstract class BaseTask
		{
			public abstract void Start();
			public abstract void Update();
			public abstract void Quit();
		}
	}
}