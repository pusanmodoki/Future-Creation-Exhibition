using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		namespace Task
		{
			[System.Serializable]
			public class PatrolMove : BaseTask
			{
				string m_thisClassName = null;

				public override void OnCreate()
				{
					m_thisClassName = typeof(PatrolMove).FullName;
				}
				public override void FixedUpdate()
				{
					throw new System.NotImplementedException();
				}

				public override EnableResult OnEnale()
				{
					throw new System.NotImplementedException();
				}

				public override void OnQuit(UpdateResult result)
				{
					throw new System.NotImplementedException();
				}

				public override UpdateResult Update()
				{
					throw new System.NotImplementedException();
				}
			}
		}
	}
}