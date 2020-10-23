using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		public abstract class BaseTask
		{
			public abstract EnableResult OnEnale();
			public abstract UpdateResult Update();
			public abstract void OnQuit(UpdateResult result);
		}
	}
}