using System;
using System.Collections;
using System.Collections.Generic;
using AI.BehaviorTree;
using UnityEngine;
[System.Serializable]//必須


public class tackle : AI.BehaviorTree.BaseTask
{
    bool tackleFlag;
    UpdateResult flag;

    public override void FixedUpdate()
    {
    }

    public override EnableResult OnEnale()
    {
        rigidbody.isKinematic = false;
        navMeshAgent.isStopped = true;
        tackleFlag = true;
        flag = UpdateResult.Run;
        Debug.Log("tackle init");
        return EnableResult.Success;
    }

    public override void OnQuit(UpdateResult result)
    {
        Debug.Log("tackle quit");
    }

    public override UpdateResult Update()
    {
        if (tackleFlag)
        {
            Debug.Log("tackle");
            Vector3 pos = blackboard.gameObjects["target"].transform.position - rigidbody.transform.position;
            pos.Normalize();
            rigidbody.AddForce(pos * 10, ForceMode.Impulse);
            tackleFlag = false;
            flag = UpdateResult.Failed;
        }
        return flag;
    }
}
