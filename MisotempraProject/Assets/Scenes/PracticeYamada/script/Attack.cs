using System;
using System.Collections;
using System.Collections.Generic;
using AI.BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
[System.Serializable]//必須

public class Attack : AI.BehaviorTree.BaseTask
{
    [SerializeField]
    string m_animationAttackKey = null;
    int m_animationID;

    public override void FixedUpdate()
    {

    }

    public override void OnCreate()//オブジェクトが作られたときに1度だけ呼ばれる
    {
        m_animationID = Animator.StringToHash(m_animationAttackKey);
    }

    public override EnableResult OnEnale()
    {
        return EnableResult.Success;
    }

    public override void OnQuit(UpdateResult result)
    {

    }

    public override UpdateResult Update()
    {
        (blackboard.components["Animator"] as Animator).SetTrigger(m_animationID);
        return UpdateResult.Success;
    }
}
