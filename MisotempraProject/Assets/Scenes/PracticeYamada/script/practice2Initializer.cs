using System;
using System.Collections;
using System.Collections.Generic;
using AI.BehaviorTree;
using UnityEngine;

public class practice2Initializer : AI.BehaviorTree.BaseBlackboardInitializer
{
    [SerializeField]
    GameObject m_obj = null;

    public override void InitializeAllInstance(Blackboard blackboard)
    {
        blackboard.gameObjects["target"] = m_obj;
    }

    public override void InitializeFirstInstance(Blackboard blackboard)
    {
    }
}
