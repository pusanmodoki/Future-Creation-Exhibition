using System.Collections;
using System.Collections.Generic;
using AI.BehaviorTree;
using UnityEngine;

public class TestInitializer : AI.BehaviorTree.BaseBlackboardInitializer
{
	public override void InitializeAllInstance(Blackboard blackboard)
	{
	}

	public override void InitializeFirstInstance(Blackboard blackboard)
	{
		blackboard.SetValue("PlayerTransform", GameObject.Find("Player").transform);
	}
}
