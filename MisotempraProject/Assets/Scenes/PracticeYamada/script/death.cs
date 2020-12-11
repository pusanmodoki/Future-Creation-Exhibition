using System;
using System.Collections;
using System.Collections.Generic;
using AI.BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
[System.Serializable]//必須


//敵死亡処理


public class death : AI.BehaviorTree.BaseTask
{
    public override void FixedUpdate()
    {
    }

    public override EnableResult OnEnale()
    {
        return EnableResult.Success;
    }

    public override void OnQuit(UpdateResult result)
    {
        Debug.Log("death");
    }

    public override UpdateResult Update()
    {
        return UpdateResult.Success;
    }
}
