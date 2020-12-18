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
			public class Death : BaseTask
			{
				public override void FixedUpdate()
				{
				}

				public override EnableResult OnEnale()
				{
					GameObject.Destroy(aiAgent.gameObject);
					return EnableResult.Success;
				}

				public override void OnQuit(UpdateResult result)
				{
				}

				public override UpdateResult Update()
				{
					return UpdateResult.Success;
				}
			}
		}
	}
}