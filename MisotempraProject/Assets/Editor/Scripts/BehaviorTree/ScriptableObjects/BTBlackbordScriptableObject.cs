using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.BehaviorTree.CashContainer;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		namespace ScriptableObject
		{
			public class BTBlackboardScriptableObject : UnityEngine.ScriptableObject
			{
				public BlackboardCashContainer cashContainer { get { return m_cashContainer; } }

				[SerializeField]
				BlackboardCashContainer m_cashContainer = null;

				public void Initialize(BlackboardCashContainer cashContainer)
				{
					m_cashContainer = cashContainer;
				}
			}
		}
	}
}