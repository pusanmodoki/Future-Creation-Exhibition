using System.Collections;
using System.Collections.Generic;
using AI.BehaviorTree.CashContainer.Detail;
using AI.BehaviorTree.Node.Detail;
using UnityEngine;


namespace AI
{
	namespace BehaviorTree
	{
		namespace Node
		{
			public class RootNode : Detail.BaseNode
			{
				int m_selectIndex = 0;

				public override EnableResult OnEnable()
				{
					if (childrenNodes.Count == 0) return EnableResult.Failed;

					m_selectIndex = -1; 
					for (int i = 0; i < childrenNodes.Count; ++i)
					{
						if (childrenNodes[i].isAllTrueDecorators
							&& childrenNodes[i].OnEnable() == EnableResult.Success)
						{
							m_selectIndex = i;
							break;
						}
					}

					return EnableResult.Success;
				}
				public override void OnDisable(UpdateResult result) { }

				public override UpdateResult Update(AIAgent agent, Blackboard blackboard)
				{
					if (m_selectIndex == -1)
					{
						FindRunNode(agent);
						if (m_selectIndex == -1)
							return UpdateResult.Failed;
					}

					var result = childrenNodes[m_selectIndex].Update(agent, blackboard);
					switch (result)
					{
						case UpdateResult.Success:
							childrenNodes[m_selectIndex].OnDisable(UpdateResult.Success);

							FindRunNode(agent);
							return UpdateResult.Run;

						case UpdateResult.Failed:
							childrenNodes[m_selectIndex].OnDisable(UpdateResult.Failed);
							
							FindRunNode(agent);
							return UpdateResult.Failed;
						default:
							return UpdateResult.Run;
					}
				}

				public override void Load(BaseCashContainer container, Blackboard blackboard) {}
				public override BaseNode Clone(AIAgent agnet, BehaviorTree behaviorTree)
				{
					var result = new RootNode();
					result.CloneBase(behaviorTree, this);
					return result;
				}

				bool FindRunNode(AIAgent agent)
				{
					bool isRunOk = false;

					for (int i = 0; i < m_childrenNodes.Count; ++i)
					{
						isRunOk = childrenNodes[i].isAllTrueDecorators && childrenNodes[i].OnEnable() == EnableResult.Success;
						if (isRunOk)
						{
							m_selectIndex = i;
							break;
						}
					}
					
					if (!isRunOk)
					{
#if UNITY_EDITOR
						Debug.Log("実行できるタスクが存在しません: " + agent.gameObject.name);
#endif
						m_selectIndex = -1;
					}

					return m_selectIndex > -1;
				}
			}
		}
	}
}