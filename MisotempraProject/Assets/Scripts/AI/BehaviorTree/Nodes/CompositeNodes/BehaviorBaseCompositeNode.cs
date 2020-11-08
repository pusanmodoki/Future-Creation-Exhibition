using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		namespace Node
		{
			namespace Composite
			{
				public enum ParallelFinishMode
				{
					Immediate,
					Delayed,
					Null,
				}

				namespace Detail
				{
					public abstract class BaseCompositeNode : BehaviorBaseNode
					{
						public ParallelFinishMode parallelFinishMode { get; private set; } = ParallelFinishMode.Null;

						public List<BaseService> services { get; private set; } = new List<BaseService>();
						public List<BehaviorBaseNode> nodes { get; private set; } = new List<BehaviorBaseNode>();
					}
				}
			}
		}
	}
}