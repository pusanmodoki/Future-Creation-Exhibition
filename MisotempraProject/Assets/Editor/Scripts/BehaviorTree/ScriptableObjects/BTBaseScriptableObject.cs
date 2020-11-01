using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.BehaviorTree.CashContainer.Detail;
using CashContainer = AI.BehaviorTree.CashContainer;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		namespace ScriptableObject
		{
			namespace Detail
			{
				public class BTBaseScriptableObject : UnityEngine.ScriptableObject
				{
					public BaseCashContainer cashContainer { get { return m_cashContainer; } }

					[SerializeField]
					protected BaseCashContainer m_cashContainer = null;

					public void Initialize(BaseCashContainer cashContainer)
					{
						m_cashContainer = cashContainer;
					}
				}
			}

			public class BTRootScriptableObject : Detail.BTBaseScriptableObject
			{
			}
			public class BTCompositeScriptableObject : Detail.BTBaseScriptableObject
			{
			}
			public class BTParallelScriptableObject : Detail.BTBaseScriptableObject
			{
			}
			public class BTRandomScriptableObject : Detail.BTBaseScriptableObject
			{
			}
			public class BTTaskScriptableObject : Detail.BTBaseScriptableObject
			{
			}
		}
	}
}