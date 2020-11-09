using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
					public abstract class BaseCompositeNode : Node.Detail.NotRootNode
					{
						public ParallelFinishMode parallelFinishMode { get; protected set; } = ParallelFinishMode.Null;
					}
				}
			}
		}
	}
}