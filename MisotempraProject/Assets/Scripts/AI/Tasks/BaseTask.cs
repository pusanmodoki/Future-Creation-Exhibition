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
			public Node.TaskNode thisNode { get; private set; } = null;
			public string subsequentTaskKey { get; private set; } = null;

			public abstract EnableResult OnEnale();
			public abstract UpdateResult Update();
			public abstract void FixedUpdate();
			public abstract void OnQuit(UpdateResult result);

			public virtual void OnCreate() { }
			public virtual void OnCollisionEnter(Collision collision) { }
			public virtual void OnCollisionStay(Collision collision) { }
			public virtual void OnCollisionExit(Collision collision) { }
			public virtual void OnTriggerEnter(Collider other) { }
			public virtual void OnTriggerStay(Collider other) { }
			public virtual void OnTriggerExit(Collider other) { }

			public void InitializeBase(BehaviorTree behaviorTree, AIAgent agent, Node.TaskNode thisNode)
			{
				aiAgent = agent;
				this.behaviorTree = behaviorTree;
				this.thisNode = thisNode;
			}
			public void RegisterSubsequentTask(string callKey)
			{
				subsequentTaskKey = aiAgent.behaviorTree.subsequentTasks.ContainsKey(callKey) ? callKey : null;
			}
		}
	}
}