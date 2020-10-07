using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		public abstract class BehaviorBaseNode
		{
			public enum EnableResult
			{
				Success,
				Failed,
			}
			public enum UpdateResult
			{
				Run,
				Success,
				Failed,
			}
			public List<BaseDecorator> decorators { get; private set; } = new List<BaseDecorator>();
			public bool isAllTrueDecorators
			{
				get
				{
					bool isResult = true;
					foreach (var e in decorators)
						isResult &= e.isPredicate();
					return isResult;
				}
			}

			public abstract EnableResult OnEnable();
			public abstract UpdateResult Update();
			public abstract void OnDisable(UpdateResult result);
		}
	}
}