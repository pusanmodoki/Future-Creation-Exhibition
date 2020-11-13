using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		[System.Serializable]
		public abstract class BaseTask
		{
			public AIAgent aiAgent { get; private set; } = null;
			public UnityEngine.AI.NavMeshAgent navMeshAgent { get { return aiAgent.navMeshAgent; } }
			public Rigidbody rigidbody { get { return aiAgent.rigidbody; } }
			public BehaviorTree behaviorTree { get; private set; } = null;
			public Blackboard blackboard { get { return behaviorTree.blackboard; } }
			public string subsequentTaskKey { get; private set; } = null;

			public abstract EnableResult OnEnale();
			public abstract UpdateResult Update();
			public abstract void FixedUpdate();
			public abstract void OnQuit(UpdateResult result);

			public void InitializeBase(BehaviorTree behaviorTree, AIAgent agent)
			{
				aiAgent = agent;
				this.behaviorTree = behaviorTree;
			}
			public void RegisterSubsequentTask(string callKey)
			{
				subsequentTaskKey = behaviorTree.subsequentTasks.ContainsKey(callKey) ? callKey : null;
			}
		}
	}
}