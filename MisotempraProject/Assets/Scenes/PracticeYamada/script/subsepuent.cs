using System;
using System.Collections;
using System.Collections.Generic;
using AI.BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]//必須

public class subsepuent : AI.BehaviorTree.BaseTask
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
        RegisterSubsequentTask("task4");
        Debug.Log("quit: " + str);
    }

    public override UpdateResult Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(str+" → subseouent 1");
            RegisterSubsequentTask("task2");
            return UpdateResult.Success;
        }
        else if(Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log(str + " → subseouent 2");
            RegisterSubsequentTask("task3");
            return UpdateResult.Success;
        }
        else
        {
            return UpdateResult.Run;
        }
    }
}
