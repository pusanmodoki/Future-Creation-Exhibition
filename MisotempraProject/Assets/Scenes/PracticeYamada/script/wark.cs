using System;
using System.Collections;
using System.Collections.Generic;
using AI.BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
[System.Serializable]//必須

public class wark : AI.BehaviorTree.BaseTask
{

    UpdateResult flag;

    public override void FixedUpdate()
    {
    }

    public override EnableResult OnEnale()
    {
        navMeshAgent.isStopped = false;
        rigidbody.isKinematic = true;
        flag = UpdateResult.Run;
        Debug.Log("wark init");
        return EnableResult.Success;
    }

    public override void OnQuit(UpdateResult result)
    {
        rigidbody.isKinematic = false;
        navMeshAgent.isStopped = true;
        Debug.Log("wark quit");
    }

    public override UpdateResult Update()
    {
		var playerTransform = blackboard.GetValue<Transform>("PlayerTransform");

		navMeshAgent.SetDestination(playerTransform.position);
        if ((playerTransform.position - rigidbody.transform.position).sqrMagnitude <= 3)
            flag = UpdateResult.Success;
        return flag;
    }
}
