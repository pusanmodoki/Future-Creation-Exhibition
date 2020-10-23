using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
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
		public abstract class BehaviorBaseNode
		{
			public List<BaseDecorator> decorators { get; private set; } = new List<BaseDecorator>();
			public string name { get; private set; } = "";
			public string guid { get; private set; } = "";
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