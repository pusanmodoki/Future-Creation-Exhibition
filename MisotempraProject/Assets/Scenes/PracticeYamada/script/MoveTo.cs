using System;
using System.Collections;
using System.Collections.Generic;
using AI.BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
[System.Serializable]//必須

public class MoveTo : AI.BehaviorTree.BaseTask
{
    [SerializeField]
    float distance = 1.0f;//判定距離

    public override void FixedUpdate()
    {
    }

    public override EnableResult OnEnale()
    {
        navMeshAgent.isStopped = false;
        rigidbody.isKinematic = true;
        return EnableResult.Success;
    }

    public override void OnQuit(UpdateResult result)
    {
    }

    public override UpdateResult Update()
    {
        navMeshAgent.SetDestination(blackboard.transforms["PlayerTransform"].position);
        //Debug.Log((blackboard.transforms["PlayerTransform"].position - rigidbody.transform.position).sqrMagnitude);
        if ((blackboard.transforms["PlayerTransform"].position - rigidbody.transform.position).sqrMagnitude < (distance /** distance*/))
        {
            //Debug.Log("a");
            navMeshAgent.isStopped = true;
            rigidbody.isKinematic = false;
            return UpdateResult.Success;
        }            
        else
            return UpdateResult.Run;
    }
}
