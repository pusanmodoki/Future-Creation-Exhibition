using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		public class BehaviorTaskNode : BehaviorBaseNode
		{
			public BaseTask task { get; /*private*/ set; } = null;

			public override EnableResult OnEnable()
			{
				return task.OnEnale();
			}

			public override void OnDisable(UpdateResult result)
			{
				task.OnQuit(result);
			}

			public override UpdateResult Update()
			{
				return task.Update();
			}
		}
	}
}