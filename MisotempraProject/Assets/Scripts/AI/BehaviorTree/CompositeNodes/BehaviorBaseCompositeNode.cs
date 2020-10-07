using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		public abstract class BehaviorBaseCompositeNode : BehaviorBaseNode
		{
			//public enum CompositeMode
			//{
			//	Sequence,
			//	Selector,
			//	Random,
			//	SimpleParallel,
			//}
			public enum SimpleParallelFinishMode
			{
				Immediate,
				Delayed,
				Null,
			}
			
			public SimpleParallelFinishMode simpleParallelFinishMode { get; private set; } = SimpleParallelFinishMode.Null;

			public List<BaseService> services { get; private set; } = new List<BaseService>();
			public List<BehaviorBaseNode> nodes { get; private set; } = new List<BehaviorBaseNode>();

		}
	}
}