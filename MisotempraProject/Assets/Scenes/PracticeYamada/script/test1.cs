using System;
using System.Collections;
using System.Collections.Generic;
using AI.BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
[System.Serializable]//必須

public class test1 : AI.BehaviorTree.BaseTask
{
    //public NavMeshAgent agent;//追加
    public GameObject target;//追加　目的地

    public override void FixedUpdate()
    {

    }

    public override EnableResult OnEnale()//start
    {
        return EnableResult.Success;
    }

    public override void OnQuit(UpdateResult result)
    {

    }

    public override UpdateResult Update()
    {
		//navMeshAgent.SetDestination(blackboard.gameObjects["Target"].transform.position);

		//agent.SetDestination(target.transform.position);
		
		
		//return UpdateResult.Run;
		return UpdateResult.Success;
	}
}
