using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		public class BehaviorCompositeSequenceNodes : BehaviorBaseCompositeNode
		{
			int m_selectIndex = 0;

			public override EnableResult OnEnable()
			{
				if (!isAllTrueDecorators || !nodes[0].isAllTrueDecorators
					|| nodes[m_selectIndex].OnEnable() == EnableResult.Failed)
					return EnableResult.Failed;

				m_selectIndex = 0;
				return EnableResult.Success;
			}
			public override void OnDisable(UpdateResult result) {}

			public override UpdateResult Update()
			{
				var result = nodes[m_selectIndex].Update();
				switch(result)
				{
					case UpdateResult.Success:
						nodes[m_selectIndex].OnDisable(UpdateResult.Success);
						if (++m_selectIndex == nodes.Count)
							return UpdateResult.Success;
						else if (nodes[m_selectIndex].isAllTrueDecorators || nodes[m_selectIndex].OnEnable() == EnableResult.Failed)
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
	}
}