using System.Collections;
using System.Collections.Generic;
using AI.BehaviorTree;
using UnityEngine;
using AI;


namespace Test_Uemura
{
	[System.Serializable]
	public class TestS1 : AI.BehaviorTree.BaseService
	{
		[SerializeField]
		int a = 0;
		public int b = 1;

		public override void ServiceFunction(AIAgent agent, Blackboard blackboard)
		{
			Debug.Log(a + "+"+ b);
		}
	}
	[System.Serializable]
	public class TestS2 : AI.BehaviorTree.BaseService
	{
		public override void ServiceFunction(AIAgent agent, Blackboard blackboard)
		{
			Debug.Log("S2");
		}
	}
}