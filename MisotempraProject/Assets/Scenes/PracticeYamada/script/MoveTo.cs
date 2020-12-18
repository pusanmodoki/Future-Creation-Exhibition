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
	string m_targetTransformBlackboardKey = "";

	[SerializeField]
    float distance = 1.0f;//判定距離

	Transform m_playerTransform = null;

	public override void OnCreate()
	{
		m_playerTransform = blackboard.GetValue<Transform>(m_targetTransformBlackboardKey);
	}
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
        navMeshAgent.SetDestination(m_playerTransform.position);
        //Debug.Log((blackboard.transforms["PlayerTransform"].position - rigidbody.transform.position).sqrMagnitude);
        if ((m_playerTransform.position - rigidbody.transform.position).sqrMagnitude < (distance /** distance*/))
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
