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
		public AgentGroup group { get { return m_group; } }
		public AIStatus status { get { return m_status; } } 
		public AIVision vision { get { return m_vision; } }

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
		[SerializeField]
		AIStatus m_status = null;
		[SerializeField]
		AIVision m_vision = null;
#if UNITY_EDITOR
		[SerializeField, TextArea]
		string m_dNowTask = null;
#endif

		public void SwitchMoveAgent()
		{
			m_rigidbody.velocity = Vector3.zero;
			m_rigidbody.isKinematic = true;
			m_navMeshAgent.isStopped = false;
			m_navMeshAgent.updatePosition = true;
			m_navMeshAgent.updateRotation = true;
		}
		public void SwitchMoveRigidbody()
		{
			m_rigidbody.isKinematic = false;
			m_navMeshAgent.isStopped = true;
			m_navMeshAgent.updatePosition = false;
			m_navMeshAgent.updateRotation = false;
		}

		public void SetGroup(AgentGroup group)
		{
			m_group = group;
		}

		void Awake()
		{
			behaviorTree = new BehaviorTree.BehaviorTree(m_fileName, this, m_status, m_blackboardInitialzier);
			if (behaviorTree.rootNode.OnEnable() == BehaviorTree.EnableResult.Failed)
				throw new System.InvalidOperationException("Behavior tree data copy failed.");
		}
		void Start()
		{
			m_group?.RegisterAgent(gameObject);	
		}
		void Update()
		{
			behaviorTree.Update();

#if UNITY_EDITOR
			List<string> names = new List<string>();
			m_dNowTask = "";
			for (BehaviorTree.Node.Detail.BaseNode node = behaviorTree.nowTask; node.parentNode != null ; node = node.parentNode)
				names.Add(node.name);
			for (int i = names.Count - 1; i >= 0; --i)
				m_dNowTask += (i != names.Count - 1 ? new string(' ', (names.Count - 1) - i) : "") + names[i] + "\n";
			m_dNowTask += new string(' ', (names.Count - 1)) + behaviorTree.nowTask.task.GetType().FullName;
#endif
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