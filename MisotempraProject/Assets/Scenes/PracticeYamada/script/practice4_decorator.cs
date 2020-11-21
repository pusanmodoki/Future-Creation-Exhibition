using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using AI.BehaviorTree;
using UnityEngine;
[System.Serializable]//必須

public class practice4_decorator : AI.BehaviorTree.BaseDecorator
{
    [SerializeField]
    KeyCode keyCode;

    public override bool IsPredicate(AIAgent agent, Blackboard blackboard)
    {
		return Input.GetKeyDown(keyCode);
    }
}
