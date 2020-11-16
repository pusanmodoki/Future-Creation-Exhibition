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
		blackboard.transforms["PlayerTransform"] = GameObject.Find("Player").transform;
	}
}
