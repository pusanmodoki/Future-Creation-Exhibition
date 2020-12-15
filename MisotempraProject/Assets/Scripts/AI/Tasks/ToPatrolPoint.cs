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
			public class ToPatrolPoint : BaseTask
			{
				public bool isArrival { get; private set; } = false;

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