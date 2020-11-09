using System.Collections;
using System.Collections.Generic;
using AI.BehaviorTree;
using UnityEngine;
using AI;


namespace Test_Uemura
{
	public class TestS1 : AI.BehaviorTree.BaseService
	{
		public override void ServiceFunction(AIAgent agent, Blackboard blackboard)
		{
			Debug.Log("S1");
		}
		public override BaseService ReturnNewThisClass()
		{
			return new TestS1();
		}
	}
	public class TestS2 : AI.BehaviorTree.BaseService
	{
		public override void ServiceFunction(AIAgent agent, Blackboard blackboard)
		{
			Debug.Log("S2");
		}
		public override BaseService ReturnNewThisClass()
		{
			return new TestS2();
		}
	}
}