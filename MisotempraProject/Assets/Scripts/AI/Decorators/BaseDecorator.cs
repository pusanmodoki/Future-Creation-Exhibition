using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		public abstract class BaseDecorator
		{
			public abstract bool IsPredicate();

			public abstract BaseDecorator ReturnNewThisClass();
		}
	}
}