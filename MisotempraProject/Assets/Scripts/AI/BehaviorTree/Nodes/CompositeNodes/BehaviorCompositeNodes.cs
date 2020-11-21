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
			namespace Composite
			{
				public class SequenceNode : Detail.BaseCompositeNode
				{
					int m_selectIndex = 0;

					public override EnableResult OnEnable()
					{
						m_selectIndex = 0;

						if (!isAllTrueDecorators || childrenNodes.Count == 0 || !childrenNodes[0].isAllTrueDecorators
							|| childrenNodes[m_selectIndex].OnEnable() == EnableResult.Failed)
							return EnableResult.Failed;
						
						foreach (var e in services) e.OnEnable();

						return EnableResult.Success;
					}
					public override void OnDisable(UpdateResult result) { }

					public override UpdateResult Update(AIAgent agent, Blackboard blackboard)
					{
						if (!isAllTrueDecoratorsWithRun)
						{
							childrenNodes[m_selectIndex].OnDisable(UpdateResult.Failed);
							return UpdateResult.Failed;
						}

						foreach (var e in services) e.Update(agent, blackboard);
						var result = childrenNodes[m_selectIndex].Update(agent, blackboard);
						switch (result)
						{
							case UpdateResult.Success:
								childrenNodes[m_selectIndex].OnDisable(UpdateResult.Success);
								if (++m_selectIndex == childrenNodes.Count)
									return UpdateResult.Success;
								else if (!childrenNodes[m_selectIndex].isAllTrueDecorators || childrenNodes[m_selectIndex].OnEnable() == EnableResult.Failed)
									return UpdateResult.Failed;
								else
									return UpdateResult.Run;
							case UpdateResult.Failed:
								childrenNodes[m_selectIndex].OnDisable(UpdateResult.Failed);
								return UpdateResult.Failed;
							default:
								return UpdateResult.Run;
						}
					}

					public override void Load(BaseCashContainer container, Blackboard blackboard)
					{
						LoadDecoratorAndService(container as CashContainer.NotRootCashContainer);
					}
					public override BaseNode Clone(AIAgent agent, BehaviorTree behaviorTree)
					{
						var result = new SequenceNode();
						result.CloneBase(behaviorTree, this);
						result.CloneDecoratorAndService(this);
						return result;
					}
				}

				public class SelectorNode : Detail.BaseCompositeNode
				{
					int m_selectIndex = 0;

					public override EnableResult OnEnable()
					{
						if (!isAllTrueDecorators)
							return EnableResult.Failed;

						m_selectIndex = -1;
						foreach (var e in services) e.OnEnable();

						for (int i = 0; i < childrenNodes.Count; ++i)
						{
							if (childrenNodes[i].isAllTrueDecorators && childrenNodes[i].OnEnable() == EnableResult.Success)
							{
								m_selectIndex = i;
								break;
							}
						}

						return m_selectIndex >= 0 ? EnableResult.Success : EnableResult.Failed;
					}

					public override void OnDisable(UpdateResult result) { }

					public override UpdateResult Update(AIAgent agent, Blackboard blackboard)
					{
						if (!isAllTrueDecoratorsWithRun)
						{
							childrenNodes[m_selectIndex].OnDisable(UpdateResult.Failed);
							return UpdateResult.Failed;
						}
						foreach (var e in services) e.Update(agent, blackboard);
						var result = childrenNodes[m_selectIndex].Update(agent, blackboard);

						switch (result)
						{
							case UpdateResult.Success:
								childrenNodes[m_selectIndex].OnDisable(UpdateResult.Success);
								return UpdateResult.Success;
							case UpdateResult.Failed:
								childrenNodes[m_selectIndex].OnDisable(UpdateResult.Failed);
								for (int i = m_selectIndex = 0; i < childrenNodes.Count; ++i)
								{
									if (childrenNodes[i].isAllTrueDecorators && childrenNodes[i].OnEnable() == EnableResult.Success)
									{
										m_selectIndex = i;
										return UpdateResult.Run;
									}
								}
								return UpdateResult.Failed;
							default:
								return UpdateResult.Run;
						}
					}

					public override void Load(BaseCashContainer container, Blackboard blackboard)
					{
						LoadDecoratorAndService(container as CashContainer.NotRootCashContainer);
					}
					public override BaseNode Clone(AIAgent agent, BehaviorTree behaviorTree)
					{
						var result = new SelectorNode();
						result.CloneBase(behaviorTree, this);
						result.CloneDecoratorAndService(this);
						return result;
					}
				}

				public class RandomSelectorNode : Detail.BaseCompositeNode
				{
					List<float> m_probabilitys = new List<float>();
					int m_selectIndex = 0;

					public override EnableResult OnEnable()
					{
						if (childrenNodes.Count == 0) return EnableResult.Failed;

						foreach (var e in services) e.OnEnable();

						List<int> indexes = new List<int>();
						for (int i = 0, cnt = childrenNodes.Count - 1; i < cnt; ++i)
						{
							float probability;
							do
							{
								probability = Random.value;
								for (int k = 0; k < m_probabilitys.Count; ++k)
								{
									if (probability <= m_probabilitys[k])
									{
										m_selectIndex = k;
										break;
									}
								}
							} while (indexes.Contains(m_selectIndex));

							if (childrenNodes[m_selectIndex].isAllTrueDecorators && childrenNodes[m_selectIndex].OnEnable() == EnableResult.Success)
								return EnableResult.Success;

							indexes.Add(m_selectIndex);
						}

						for (int i = 0; i < childrenNodes.Count; ++i)
							if (!indexes.Contains(i))
							{
								m_selectIndex = i;

								if (childrenNodes[m_selectIndex].isAllTrueDecorators && childrenNodes[m_selectIndex].OnEnable() == EnableResult.Success)
									return EnableResult.Success;

								break;
							}

						return EnableResult.Failed;
					}

					public override void OnDisable(UpdateResult result) { }

					public override UpdateResult Update(AIAgent agent, Blackboard blackboard)
					{
						if (!isAllTrueDecoratorsWithRun)
						{
							childrenNodes[m_selectIndex].OnDisable(UpdateResult.Failed);
							return UpdateResult.Failed;
						}
						foreach (var e in services) e.Update(agent, blackboard);
						var result = childrenNodes[m_selectIndex].Update(agent, blackboard);

						switch (result)
						{
							case UpdateResult.Success:
								childrenNodes[m_selectIndex].OnDisable(UpdateResult.Success);
								return UpdateResult.Success;
							case UpdateResult.Failed:
								{
									childrenNodes[m_selectIndex].OnDisable(UpdateResult.Failed);
									for (int i = m_selectIndex = 0; i < childrenNodes.Count; ++i)
									{
										if (childrenNodes[i].isAllTrueDecorators && childrenNodes[i].OnEnable() == EnableResult.Success)
										{
											m_selectIndex = i;
											return UpdateResult.Run;
										}
									}
									return UpdateResult.Failed;
								}
							default:
								return UpdateResult.Run;
						}
					}

					public override void Load(BaseCashContainer container, Blackboard blackboard)
					{
						var cast = container as CashContainer.RandomCashContainer;
						LoadDecoratorAndService(cast);

						float sum = 0.0f;
						foreach (var probability in cast.probabilitys)
						{
							sum += probability;
							m_probabilitys.Add(sum);
						}
					}
					public override BaseNode Clone(AIAgent agent, BehaviorTree behaviorTree)
					{
						var result = new RandomSelectorNode();
						result.CloneBase(behaviorTree, this);
						result.CloneDecoratorAndService(this);

						foreach (var probability in m_probabilitys)
							result.m_probabilitys.Add(probability);

						return result;
					}
				}

				public class ParallelNode : Detail.BaseCompositeNode
				{
					bool[] m_isEndNodes = new bool[2] { false, false };

					public override EnableResult OnEnable()
					{
						if (childrenNodes.Count != 2) return EnableResult.Failed;

						m_isEndNodes[0] = !(childrenNodes[0].isAllTrueDecorators && childrenNodes[0].OnEnable() == EnableResult.Success);
						m_isEndNodes[1] = !(childrenNodes[1].isAllTrueDecorators && childrenNodes[1].OnEnable() == EnableResult.Success);
						foreach (var e in services) e.OnEnable();

						if ((parallelFinishMode == ParallelFinishMode.Immediate && (!m_isEndNodes[0] & !m_isEndNodes[1]))
							|| (parallelFinishMode == ParallelFinishMode.Delayed && (!m_isEndNodes[0] | !m_isEndNodes[1])))
							return EnableResult.Success;

						return EnableResult.Failed;
					}
					public override void OnDisable(UpdateResult result) { }

					public override UpdateResult Update(AIAgent agent, Blackboard blackboard)
					{
						if (!isAllTrueDecoratorsWithRun)
						{
							if (!m_isEndNodes[0]) childrenNodes[0].OnDisable(UpdateResult.Failed);
							if (!m_isEndNodes[1]) childrenNodes[1].OnDisable(UpdateResult.Failed);
							return UpdateResult.Failed;
						}
						UpdateResult result;
						foreach (var e in services) e.Update(agent, blackboard);

						for (int i = 0, partner = 1; i < 2; ++i, --partner)
						{
							if (!m_isEndNodes[i])
							{
								result = childrenNodes[i].Update(agent, blackboard);

								if (result != UpdateResult.Run)
								{
									childrenNodes[i].OnDisable(result);
									m_isEndNodes[i] = true;

									if (m_isEndNodes[partner]) return result;

									if (parallelFinishMode == ParallelFinishMode.Immediate)
									{
										childrenNodes[partner].OnDisable(result);
										return result;
									}
								}
							}
						}

						return m_isEndNodes[0] & m_isEndNodes[1] ? UpdateResult.Success : UpdateResult.Run;
					}

					public override void Load(BaseCashContainer container, Blackboard blackboard)
					{
						LoadDecoratorAndService(container as CashContainer.NotRootCashContainer);
					}
					public override BaseNode Clone(AIAgent agent, BehaviorTree behaviorTree)
					{
						var result = new ParallelNode();
						result.CloneBase(behaviorTree, this);
						result.CloneDecoratorAndService(this);
						result.parallelFinishMode = parallelFinishMode;
						return result;
					}
				}


				public class SimpleParallelNode : Detail.BaseCompositeNode
				{
					bool[] m_isEndNodes = new bool[2] { false, false };
					TaskNode m_task = null;
					Detail.BaseCompositeNode m_composite = null;

					public override EnableResult OnEnable()
					{
						if (childrenNodes.Count != 2) return EnableResult.Failed;

						m_task = childrenNodes[0] as TaskNode;
						if (m_task == null)
							m_task = childrenNodes[1] as TaskNode;

						m_composite = childrenNodes[0] as Detail.BaseCompositeNode;
						if (m_composite == null)
							m_composite = childrenNodes[1] as Detail.BaseCompositeNode;

						if (m_task == null || m_composite == null)
							return EnableResult.Failed;

						m_isEndNodes[0] = !(m_task.isAllTrueDecorators && m_task.OnEnable() == EnableResult.Success);
						m_isEndNodes[1] = !(m_composite.isAllTrueDecorators && m_composite.OnEnable() == EnableResult.Success);
						foreach (var e in services) e.OnEnable();

						if ((parallelFinishMode == ParallelFinishMode.Immediate && (!m_isEndNodes[0]))
							|| (parallelFinishMode == ParallelFinishMode.Delayed && (!m_isEndNodes[0] | !m_isEndNodes[1])))
							return EnableResult.Success;

						return EnableResult.Failed;
					}

					public override void OnDisable(UpdateResult result) { }

					public override UpdateResult Update(AIAgent agent, Blackboard blackboard)
					{
						if (!isAllTrueDecoratorsWithRun)
						{
							if (!m_isEndNodes[0]) m_task.OnDisable(UpdateResult.Failed);
							if (!m_isEndNodes[1]) m_composite.OnDisable(UpdateResult.Failed);
							return UpdateResult.Failed;
						}
						UpdateResult result;
						foreach (var e in services) e.Update(agent, blackboard);

						if (!m_isEndNodes[0])
						{
							result = m_task.Update(agent, blackboard);
							if (result != UpdateResult.Run)
							{
								m_task.OnDisable(result);
								m_isEndNodes[0] = true;

								if (parallelFinishMode == ParallelFinishMode.Immediate)
								{
									m_task.OnDisable(result);
									return result;
								}
							}
						}

						result = m_composite.Update(agent, blackboard);
						if (!(parallelFinishMode == ParallelFinishMode.Delayed && m_isEndNodes[0]))
							return UpdateResult.Run;
						else
						{
							childrenNodes[1].OnDisable(result);
							return result;
						}
					}

					public override void Load(BaseCashContainer container, Blackboard blackboard)
					{
						LoadDecoratorAndService(container as CashContainer.NotRootCashContainer);
					}
					public override BaseNode Clone(AIAgent agent, BehaviorTree behaviorTree)
					{
						var result = new SimpleParallelNode();
						result.CloneBase(behaviorTree, this);
						result.CloneDecoratorAndService(this);
						result.parallelFinishMode = parallelFinishMode;
						return result;
					}
				}

			}
		}
	}
}