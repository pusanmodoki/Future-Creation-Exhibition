using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>MisoTempra editor</summary>
namespace LocalEditor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		namespace ScriptableObject
		{
			public class BTTaskFunctionScriptableObject : UnityEngine.ScriptableObject
			{
				public AI.BehaviorTree.BaseTask task { get; private set; } = null;

				[SerializeField]
				AI.BehaviorTree.BaseTask m_task = null;

				public void Initialize(AI.BehaviorTree.BaseTask task)
				{
					m_task = task;
				}
			}
		}
	}
}