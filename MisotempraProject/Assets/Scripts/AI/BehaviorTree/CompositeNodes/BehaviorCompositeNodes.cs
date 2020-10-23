using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		public class BehaviorCompositeSequenceNode : BehaviorBaseCompositeNode
		{
			int m_selectIndex = 0;

			public override EnableResult OnEnable()
			{
				if (!isAllTrueDecorators || nodes.Count == 0 || !nodes[0].isAllTrueDecorators
					|| nodes[m_selectIndex].OnEnable() == EnableResult.Failed)
					return EnableResult.Failed;

				m_selectIndex = 0;
				foreach (var e in services) e.OnEnable();

				return EnableResult.Success;
			}
			public override void OnDisable(UpdateResult result) {}

			public override UpdateResult Update()
			{
				foreach (var e in services) e.Update();
				var result = nodes[m_selectIndex].Update();
				switch(result)
				{
					case UpdateResult.Success:
						nodes[m_selectIndex].OnDisable(UpdateResult.Success);
						if (++m_selectIndex == nodes.Count)
							return UpdateResult.Success;
						else if (!nodes[m_selectIndex].isAllTrueDecorators || nodes[m_selectIndex].OnEnable() == EnableResult.Failed)
							return UpdateResult.Failed;
						else
							return UpdateResult.Run;
					case UpdateResult.Failed:
						nodes[m_selectIndex].OnDisable(UpdateResult.Failed);
						return UpdateResult.Failed; 
					default:
						return UpdateResult.Run;
				}
			}
		}

		public class BehaviorCompositeSelectorNode : BehaviorBaseCompositeNode
		{
			int m_selectIndex = 0;

			public override EnableResult OnEnable()
			{
				if (!isAllTrueDecorators)
					return EnableResult.Failed;

				m_selectIndex = -1;
				foreach (var e in services) e.OnEnable();

				for (int i = 0; i < nodes.Count; ++i)
				{
					if (nodes[i].isAllTrueDecorators && nodes[i].OnEnable() == EnableResult.Success)
						m_selectIndex = i;
				}

				return m_selectIndex >= 0 ? EnableResult.Success : EnableResult.Failed;
			}

			public override void OnDisable(UpdateResult result) {}

			public override UpdateResult Update()
			{
				foreach (var e in services) e.Update();
				var result = nodes[m_selectIndex].Update();

				switch (result)
				{
					case UpdateResult.Success:
						nodes[m_selectIndex].OnDisable(UpdateResult.Success);
						return UpdateResult.Success;
					case UpdateResult.Failed:
						nodes[m_selectIndex].OnDisable(UpdateResult.Failed);
						for (int i = m_selectIndex = 0; i < nodes.Count; ++i)
						{
							if (nodes[i].isAllTrueDecorators && nodes[i].OnEnable() == EnableResult.Success)
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
		}

		public class BehaviorCompositeRandomSelectorNode : BehaviorBaseCompositeNode
		{
			HashSet<int> randomIndexes = new HashSet<int>();
			int m_selectIndex = 0;


			public override EnableResult OnEnable()
			{
				if (nodes.Count == 0) return EnableResult.Failed;
				randomIndexes.Clear();
				m_selectIndex = 0;
				foreach (var e in services) e.OnEnable();

				for (int i = 0, index = 0; i < nodes.Count; ++i)
				{
					do
					{
						index = Random.Range(0, nodes.Count);
					} while (!randomIndexes.Contains(m_selectIndex));
					randomIndexes.Add(m_selectIndex);
				}

				if (nodes[m_selectIndex].isAllTrueDecorators && nodes[m_selectIndex].OnEnable() == EnableResult.Success)
					return EnableResult.Success;

				return EnableResult.Failed;
			}

			public override void OnDisable(UpdateResult result) { }

			public override UpdateResult Update()
			{
				foreach (var e in services) e.Update();
				var result = nodes[m_selectIndex].Update();

				switch (result)
				{
					case UpdateResult.Success:
						nodes[m_selectIndex].OnDisable(UpdateResult.Success);
						return UpdateResult.Success;
					case UpdateResult.Failed:
						{
							nodes[m_selectIndex].OnDisable(UpdateResult.Failed);
							for (int i = m_selectIndex = 0; i < nodes.Count; ++i)
							{
								if (nodes[i].isAllTrueDecorators && nodes[i].OnEnable() == EnableResult.Success)
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
		}

		public class BehaviorCompositeParallelNode : BehaviorBaseCompositeNode
		{
			bool[] m_isEndNodes = new bool[2] { false, false };

			public override EnableResult OnEnable()
			{
				if (nodes.Count != 2) return EnableResult.Failed;

				m_isEndNodes[0] = !nodes[0].isAllTrueDecorators && nodes[0].OnEnable() == EnableResult.Success;
				m_isEndNodes[1] = !nodes[1].isAllTrueDecorators && nodes[1].OnEnable() == EnableResult.Success;
				foreach (var e in services) e.OnEnable();

				if ((parallelFinishMode == ParallelFinishMode.Immediate && (!m_isEndNodes[0] & !m_isEndNodes[1]))
					|| (parallelFinishMode == ParallelFinishMode.Delayed && (!m_isEndNodes[0] | !m_isEndNodes[1])))
					return EnableResult.Success;

				return EnableResult.Failed;
			}
			public override void OnDisable(UpdateResult result) {}

			public override UpdateResult Update()
			{
				UpdateResult result;
				foreach (var e in services) e.Update();

				for (int i = 0, partner = 1; i < 2; ++i, --partner)
				{
					if (!m_isEndNodes[i])
					{
						result = nodes[i].Update();
						switch (result)
						{
							case UpdateResult.Success:
								nodes[i].OnDisable(UpdateResult.Success);
								m_isEndNodes[i] = true;

								if (m_isEndNodes[partner]) return UpdateResult.Success;

								if (parallelFinishMode == ParallelFinishMode.Immediate)
								{
									nodes[partner].OnDisable(UpdateResult.Success);
									return UpdateResult.Success;
								}
								break;
							case UpdateResult.Failed:
								nodes[i].OnDisable(UpdateResult.Failed);
								m_isEndNodes[i] = true;

								if (m_isEndNodes[partner]) return UpdateResult.Failed;

								if (parallelFinishMode == ParallelFinishMode.Immediate)
								{
									nodes[partner].OnDisable(UpdateResult.Failed);
									return UpdateResult.Failed;
								}
								break;
							default:
								break;
						}
					}
				}

				return m_isEndNodes[0] & m_isEndNodes[1] ? UpdateResult.Success : UpdateResult.Run;
			}
		}


		public class BehaviorCompositeSimpleParallelNode : BehaviorBaseCompositeNode
		{
			bool[] m_isEndNodes = new bool[2] { false, false };

			public override EnableResult OnEnable()
			{
				if (nodes.Count != 2) return EnableResult.Failed;

				m_isEndNodes[0] = !nodes[0].isAllTrueDecorators && nodes[0].OnEnable() == EnableResult.Success;
				m_isEndNodes[1] = !nodes[1].isAllTrueDecorators && nodes[1].OnEnable() == EnableResult.Success;
				foreach (var e in services) e.OnEnable();

				if ((parallelFinishMode == ParallelFinishMode.Immediate && (!m_isEndNodes[0]))
					|| (parallelFinishMode == ParallelFinishMode.Delayed && (!m_isEndNodes[0] | !m_isEndNodes[1])))
					return EnableResult.Success;

				return EnableResult.Failed;
			}

			public override void OnDisable(UpdateResult result) { }

			public override UpdateResult Update()
			{
				UpdateResult result;
				foreach (var e in services) e.Update();

				if (!m_isEndNodes[0])
				{
					result = nodes[0].Update();
					switch (result)
					{
						case UpdateResult.Success:
							nodes[0].OnDisable(UpdateResult.Success);
							m_isEndNodes[0] = true;

							if (parallelFinishMode == ParallelFinishMode.Immediate)
							{
								nodes[1].OnDisable(UpdateResult.Success);
								return UpdateResult.Success;
							}
							break;
						case UpdateResult.Failed:
							nodes[0].OnDisable(UpdateResult.Failed);
							m_isEndNodes[1] = true;

							if (parallelFinishMode == ParallelFinishMode.Immediate)
							{
								nodes[0].OnDisable(UpdateResult.Failed);
								return UpdateResult.Failed;
							}
							break;
						default:
							break;
					}
				}

				result = nodes[1].Update();
				if (!(parallelFinishMode == ParallelFinishMode.Delayed && m_isEndNodes[0]))
					return UpdateResult.Run;

				switch(result)
				{
					case UpdateResult.Success:
						nodes[1].OnDisable(UpdateResult.Success);
						return UpdateResult.Success;					
					case UpdateResult.Failed:
						nodes[1].OnDisable(UpdateResult.Failed);
						return UpdateResult.Failed;
					default:
						return UpdateResult.Run;
				}
			}
		}


	}
}