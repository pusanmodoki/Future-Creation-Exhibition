using System;
using System.Collections;
using System.Collections.Generic;
using AI.BehaviorTree;
using UnityEngine;

public class Enemy_Ini : AI.BehaviorTree.BaseBlackboardInitializer
{

    [SerializeField]
    string m_animationAttackKey = null;
    int m_animationID;

    public override void InitializeAllInstance(Blackboard blackboard)//
    {
        throw new NotImplementedException();
    }

    public override void InitializeFirstInstance(Blackboard blackboard)//最初のインスタンス
    {
        throw new NotImplementedException();
    }
}
