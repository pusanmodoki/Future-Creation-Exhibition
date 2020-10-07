using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		public class BehaviorTaskNode : BehaviorBaseNode
		{
			public BaseTask task { get; private set; } = null;

			public override bool Update()
			{

			}
		}
	}
}