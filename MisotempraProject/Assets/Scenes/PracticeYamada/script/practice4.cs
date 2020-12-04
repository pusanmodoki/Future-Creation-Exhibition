using System;
using System.Collections;
using System.Collections.Generic;
using AI.BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
[System.Serializable]//必須

public class practice4 : AI.BehaviorTree.BaseTask
{
    [SerializeField]
    string str = "";

    public override void FixedUpdate()
    {
    }

    public override EnableResult OnEnale()
    {
        return EnableResult.Success;
    }

    public override void OnQuit(UpdateResult result)
    {
		Debug.Log("quit: " + str);
	}

    public override UpdateResult Update()
    {
        Debug.Log(str);
        return UpdateResult.Success;
    }
}
