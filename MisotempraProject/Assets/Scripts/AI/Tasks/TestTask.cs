using System.Collections;
using System.Collections.Generic;
using AI.BehaviorTree;
using UnityEngine;

[System.Serializable]
public class TestTask : AI.BehaviorTree.BaseTask
{
	[SerializeField]
	int a = 0;

	int b = 0;

	public int c;

	public override EnableResult OnEnale()
	{
		return EnableResult.Success;
	}

	public override UpdateResult Update()
	{
		Debug.Log("Hello!! + " + this.GetHashCode());

		if (Input.GetKeyDown(KeyCode.Space))
			return UpdateResult.Success;
		else if (Input.GetKeyDown(KeyCode.A))
			return UpdateResult.Failed;
		return UpdateResult.Run;
	}

	public override void OnQuit(UpdateResult result)
	{
	}

	public override void FixedUpdate()
	{
	}
}
