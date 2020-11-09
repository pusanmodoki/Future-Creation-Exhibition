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
					if (childrenNodes.Count == 0 || !childrenNodes[0].isAllTrueDecorators
						|| childrenNodes[m_selectIndex].OnEnable() == EnableResult.Failed)
						return EnableResult.Failed;
					
					return EnableResult.Success;
				}
				public override void OnDisable(UpdateResult result) { }

				public override UpdateResult Update(AIAgent agent, Blackboard blackboard)
				{
					if (m_selectIndex == -1)
						FindRunNode(agent);

					var result = childrenNodes[m_selectIndex].Update(agent, blackboard);
					switch (result)
					{
						case UpdateResult.Success:
							childrenNodes[m_selectIndex].OnDisable(UpdateResult.Success);

							m_selectIndex = 0;
							FindRunNode(agent);
							return UpdateResult.Run;

						case UpdateResult.Failed:
							childrenNodes[m_selectIndex].OnDisable(UpdateResult.Failed);
							m_selectIndex = 0;
							return UpdateResult.Failed;
						default:
							return UpdateResult.Run;
					}
				}

				public override void Load(BaseCashContainer container, Blackboard blackboard) {}
				public override BaseNode Clone(AIAgent agnet, BehaviorTree behaviorTree)
				{
					var result = new RootNode();
					result.CloneBase(this);
					return result;
				}

				bool FindRunNode(AIAgent agent)
				{
					int findIndex = m_selectIndex;
					bool isRunOk = false;
					do
					{
						findIndex = (findIndex + 1) % childrenNodes.Count;

						isRunOk = childrenNodes[m_selectIndex].isAllTrueDecorators
						&& childrenNodes[m_selectIndex].OnEnable() == EnableResult.Success;

					} while (!isRunOk || findIndex != m_selectIndex);
					if (!isRunOk && findIndex == m_selectIndex)
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