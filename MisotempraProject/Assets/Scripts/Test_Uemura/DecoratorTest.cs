using System.Collections;
using System.Collections.Generic;
using AI.BehaviorTree;
using UnityEngine;


namespace Test_Uemura
{
	public class TestD1 : AI.BehaviorTree.BaseDecorator
	{
		public override bool IsPredicate()
		{
			return true;
		}

		public override BaseDecorator ReturnNewThisClass()
		{
			return new TestD1();
		}
	}
	public class TestD2 : AI.BehaviorTree.BaseDecorator
	{
		public override bool IsPredicate()
		{
			return true;
		}

		public override BaseDecorator ReturnNewThisClass()
		{
			return new TestD2();
		}
	}
}