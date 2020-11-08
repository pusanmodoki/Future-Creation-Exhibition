using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		[System.Serializable]
		public class BaseTask
		{
			public virtual EnableResult OnEnale() { return EnableResult.Success; }
			public virtual UpdateResult Update() { return UpdateResult.Success; }	
			public virtual void OnQuit(UpdateResult result) { }
		}
	}
}