using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.BehaviorTree.CashContainer.Detail;
using CashContainer = AI.BehaviorTree.CashContainer;
using System.Linq;
using UnityEditor;

/// <summary>MisoTempra editor</summary>
namespace LocalEditor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		namespace ScriptableObject
		{
			namespace Detail
			{
				public abstract class BTBaseScriptableObject : UnityEngine.ScriptableObject
				{
					public List<BaseCashContainer> cashContainers { get { return m_cashContainers; } }

					public abstract void Initialize(BaseCashContainer thisContainer, List<BaseCashContainer> cashContainers);

					[SerializeField]
					protected List<BaseCashContainer> m_cashContainers = null;
				}
			}

			public class BTRootScriptableObject : Detail.BTBaseScriptableObject
			{
				public CashContainer.RootCashContainer thisContainer { get { return m_thisContainer; } }

				[SerializeField]
				protected CashContainer.RootCashContainer m_thisContainer = null;

				public override void Initialize(BaseCashContainer thisContainer, List<BaseCashContainer> cashContainers)
				{
					m_thisContainer = thisContainer as CashContainer.RootCashContainer;
					m_cashContainers = cashContainers;
				}
			}
			public class BTCompositeScriptableObject : Detail.BTBaseScriptableObject
			{
				public CashContainer.CompositeCashContainer thisContainer { get { return m_thisContainer; } }

				[SerializeField]
				protected CashContainer.CompositeCashContainer m_thisContainer = null;

				public override void Initialize(BaseCashContainer thisContainer, List<BaseCashContainer> cashContainers)
				{
					m_thisContainer = thisContainer as CashContainer.CompositeCashContainer;
					m_cashContainers = cashContainers;
				}
			}
			public class BTParallelScriptableObject : Detail.BTBaseScriptableObject
			{
				public CashContainer.ParallelCashContainer thisContainer { get { return m_thisContainer; } }

				[SerializeField]
				protected CashContainer.ParallelCashContainer m_thisContainer = null;

				public override void Initialize(BaseCashContainer thisContainer, List<BaseCashContainer> cashContainers)
				{
					m_thisContainer = thisContainer as CashContainer.ParallelCashContainer;
					m_cashContainers = cashContainers;
				}
			}
			public class BTRandomScriptableObject : Detail.BTBaseScriptableObject
			{
				public CashContainer.RandomCashContainer thisContainer { get { return m_thisContainer; } }

				[SerializeField]
				protected CashContainer.RandomCashContainer m_thisContainer = null;

				public override void Initialize(BaseCashContainer thisContainer, List<BaseCashContainer> cashContainers)
				{
					m_thisContainer = thisContainer as CashContainer.RandomCashContainer;
					m_cashContainers = cashContainers;
				}
			}
			public class BTTaskScriptableObject : Detail.BTBaseScriptableObject
			{
				public CashContainer.TaskCashContainer thisContainer { get { return m_thisContainer; } }

				[SerializeField]
				protected CashContainer.TaskCashContainer m_thisContainer = null;

				public override void Initialize(BaseCashContainer thisContainer, List<BaseCashContainer> cashContainers)
				{
					m_thisContainer = thisContainer as CashContainer.TaskCashContainer;
					m_cashContainers = cashContainers;
				}
			}
		}
	}
}