using System.Collections;
using System.Collections.Generic;
using AI.BehaviorTree;
using UnityEngine;

public class TestTask : AI.BehaviorTree.BaseTask
{
	public override void OnQuit(UpdateResult result)
	{
	}

	public override EnableResult OnEnale()
	{
		return EnableResult.Success;
	}

	public override UpdateResult Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			return UpdateResult.Success;
		else if (Input.GetKeyDown(KeyCode.A))
			return UpdateResult.Failed;
		return UpdateResult.Run;
	}
}
