using System;
using System.Collections;
using System.Collections.Generic;
using AI.BehaviorTree;
using UnityEngine;

public class Enemy_Ini : AI.BehaviorTree.BaseBlackboardInitializer
{
    [SerializeField]
    Animator animator = null;
    [SerializeField]
    Transform m_obj;

    public override void InitializeAllInstance(Blackboard blackboard)
    {
        blackboard.transforms["PlayerTransform"] = m_obj;
    }

    public override void InitializeFirstInstance(Blackboard blackboard)//最初のインスタンス
    {
        blackboard.components["Animator"] = animator;
    }
}
