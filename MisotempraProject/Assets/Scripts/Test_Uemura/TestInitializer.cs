using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakaTe
{
    public class TestInitializer : AI.BehaviorTree.BaseBlackboardInitializer
    {
        [SerializeField]
        GameObject m_obj = null;
        //シーンの中で、一番最初のオブジェクト
        public override void InitializeFirstInstance(AI.BehaviorTree.Blackboard blackboard)
        {
            blackboard.SetValue("Target", m_obj);
        }

        //毎回生成されるたび呼ばれる
        public override void InitializeAllInstance(AI.BehaviorTree.Blackboard blackboard)
        {

        }
    }

}