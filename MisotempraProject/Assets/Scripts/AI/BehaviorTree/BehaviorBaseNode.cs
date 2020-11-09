using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		public enum EnableResult
		{
			Success,
			Failed,
		}
		public enum UpdateResult
		{
			Run,
			Success,
			Failed,
		}
		namespace Node
		{
			namespace Detail
			{
				public class BaseNode
				{
					public string name { get; protected set; } = "";
					public string guid { get; protected set; } = "";
					public string editorMemo { get; protected set; } = "";
					public BaseNode parentNode { get; private set; } = null;
					public ReadOnlyCollection<NotRootNode> childrenNodes { get; private set; } = null;
					protected List<NotRootNode> m_childrenNodes { get; private set; } = null;

					public virtual EnableResult OnEnable() { return default; }
					public virtual UpdateResult Update(AIAgent agent, Blackboard blackboard) { return default; }
					public virtual void OnDisable(UpdateResult result) { }
					public virtual void Load(CashContainer.Detail.BaseCashContainer container, Blackboard blackboard) { }
					public virtual BaseNode Clone(AIAgent agent, BehaviorTree behaviorTree) { return default; }

					public BaseNode()
					{
						m_childrenNodes = new List<NotRootNode>();
						childrenNodes = new ReadOnlyCollection<NotRootNode>(m_childrenNodes);
					}

					public void RegisterParent(BaseNode node) { parentNode = node; }
					public void RegisterChildren(BaseNode node) { m_childrenNodes.Add(node as NotRootNode); }

					public void BaseInitialize(CashContainer.Detail.BaseCashContainer container)
					{
						name = container.nodeName;
						guid = container.guid;
						editorMemo = container.memo;
					}

					protected void CloneBase(BaseNode node)
					{
						name = node.name;
						guid = node.guid;
						editorMemo = node.editorMemo;
					}
				}
			}
		}
	}
}