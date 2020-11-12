using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AI.BehaviorTree.Node;

namespace AI
{
	[DisallowMultipleComponent, DefaultExecutionOrder(-1), RequireComponent(typeof(Rigidbody)),
		RequireComponent(typeof(NavMeshAgent))]
	public class AIAgent : MonoBehaviour
	{
		public BehaviorTree.BehaviorTree behaviorTree { get; private set; } = null;
		public NavMeshAgent navMeshAgent { get { return m_navMeshAgent; } }
		public new Rigidbody rigidbody { get { return m_rigidbody; } }

		[SerializeField]
		BehaviorTree.Detail.BehaviorFileName m_fileName = null;
		[SerializeField]
		BehaviorTree.BaseBlackboardInitialzier m_blackboardInitialzier = null;
		[SerializeField]
		NavMeshAgent m_navMeshAgent = null;
		[SerializeField]
		Rigidbody m_rigidbody = null;

		TaskNode m_nowTask = null;

		public void RegisterTask(TaskNode node) { m_nowTask = node; }
		public void UnregisterTask() { m_nowTask = null; }

		void OnEnable()
		{
			behaviorTree = new BehaviorTree.BehaviorTree(m_fileName, this, m_blackboardInitialzier);
		}

		void Update()
		{
			behaviorTree.Update();
		}
		void FixedUpdate()
		{
			if (m_nowTask != null) m_nowTask.FixedUpdate();
		}
	}
}