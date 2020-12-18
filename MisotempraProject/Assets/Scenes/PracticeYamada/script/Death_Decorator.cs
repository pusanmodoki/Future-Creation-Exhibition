using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using AI.BehaviorTree;
using UnityEngine;
[System.Serializable]//必須

//敵死亡判定decorator

public class Death_Decorator : AI.BehaviorTree.BaseDecorator
{
    [SerializeField]
    bool live = false;

    public override bool IsPredicate(AIAgent agent, Blackboard blackboard)
    {
        return !live;
    }
}
