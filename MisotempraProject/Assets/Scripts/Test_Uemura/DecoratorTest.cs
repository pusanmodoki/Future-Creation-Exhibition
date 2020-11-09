using System.Collections;
using System.Collections.Generic;
using AI.BehaviorTree;
using UnityEngine;


namespace Test_Uemura
{
	[System.Serializable]
	public class TestD1 : AI.BehaviorTree.BaseDecorator
	{
		[SerializeField]
		int a;
		public override bool IsPredicate()
		{
			return true;
		}

	}
	[System.Serializable]
	public class TestD2 : AI.BehaviorTree.BaseDecorator
	{
		public override bool IsPredicate()
		{
			return true;
		}

	}
}