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
		BehaviorTree.BaseBlackboardInitializer m_blackboardInitialzier = null;
		[SerializeField]
		NavMeshAgent m_navMeshAgent = null;
		[SerializeField]
		Rigidbody m_rigidbody = null;
		[SerializeField]
		AgentGroup m_group = null;

		public void SetGroup(AgentGroup group)
		{
			m_group = group;
		}

		void Awake()
		{
			behaviorTree = new BehaviorTree.BehaviorTree(m_fileName, this, m_blackboardInitialzier);
		}
		void Start()
		{
			m_group?.RegisterAgent(gameObject);	
		}
		void Update()
		{
			behaviorTree.Update();
		}
		void FixedUpdate()
		{
			behaviorTree.FixedUpdate();
		}
		void OnDestroy()
		{
			m_group?.UnregisterAgent(gameObject);
		}

		void OnCollisionEnter(Collision collision)
		{
			behaviorTree.nowTask?.task.OnCollisionEnter(collision);
		}
		void OnCollisionStay(Collision collision)
		{
			behaviorTree.nowTask?.task.OnCollisionStay(collision);
		}
		void OnCollisionExit(Collision collision)
		{
			behaviorTree.nowTask?.task.OnCollisionExit(collision);
		}
		void OnTriggerEnter(Collider other)
		{
			behaviorTree.nowTask?.task.OnTriggerEnter(other);
		}
		void OnTriggerStay(Collider other)
		{
			behaviorTree.nowTask?.task.OnTriggerStay(other);
		}
		void OnTriggerExit(Collider other)
		{
			behaviorTree.nowTask?.task.OnTriggerExit(other);
		}
	}
}