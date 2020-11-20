using System;
using System.Collections;
using System.Collections.Generic;
using AI.BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
[System.Serializable]//必須

public class practice3 : AI.BehaviorTree.BaseTask
{
    [SerializeField]
    KeyCode keyCode;
    [SerializeField]
    string str;

    public override void FixedUpdate()
    {
    }

    public override EnableResult OnEnale()
    {
        return EnableResult.Success;
    }

    public override void OnQuit(UpdateResult result)
    {
    }

    public override UpdateResult Update()
    {
        if(Input.GetKeyDown(keyCode))
        {
            Debug.Log(str);
            return UpdateResult.Success;
        }
        else
        {
            return UpdateResult.Run;
        }
    }
}
