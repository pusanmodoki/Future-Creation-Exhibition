//作成者 : 植村将太
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	/// <summary>
	/// AgentをGroup化するAgentGroup
	/// </summary>
	public class AgentGroup : MonoBehaviour
	{
		/// <summary>PatrolPoints getter</summary>
		public AIPatrolPoints patrolPoints { get { return m_patrolPoints; } }
		/// <summary>group objects getter</summary>
		public List<GameObject> groupObjects { get { return m_drawingGroupObjects; } }
		/// <summary>instanceID of EnemyGroup</summary>
		public int instanceGroupID { get; private set; } = -1;

		/// <summary>this PatrolPoints</summary>
		[SerializeField, Tooltip("this PatrolPoints")]
		AIPatrolPoints m_patrolPoints = new AIPatrolPoints();
		/// <summary>Drawing my group enemys</summary>
		[SerializeField, Tooltip("Drawing my group enemys")]
		List<GameObject> m_drawingGroupObjects = new List<GameObject>();

		//debug only
#if UNITY_EDITOR
		/// <summary>is draw gizmos (debug only)</summary>
		[SerializeField, Tooltip("is draw gizmos (debug only)")]
		bool m_dIsDrawGizmos = true;
		/// <summary>is draw gizmos (debug only)</summary>
		public bool dIsDrawGizmos { get { return m_dIsDrawGizmos; } set { m_dIsDrawGizmos = value; } }
#endif

		/// <summary>instanceID Counter</summary>
		static int m_instanceCounter = 0;

		/// <summary>[Start]</summary>
		void Start()
		{
			//Set InstanceID
			instanceGroupID = m_instanceCounter++;

			//Managerに登録
			AgentGroupManager.instance.Add(this);
			//PatrolPoints初期化
			m_patrolPoints.Start();
		}
		/// <summary>[OnValidate]</summary>
		void OnValidate()
		{
			//PatrolPoints変更
			m_patrolPoints.OnValidate();
		}
		/// <summary>[OnDestroy]</summary>
		void OnDestroy()
		{
			//Managerから削除
			AgentGroupManager.instance?.Remove(this);
		}

		/// <summary>
		/// [RegisterAgent]
		/// Agentを登録する
		/// 引数1: agent
		/// </summary>
		public void RegisterAgent(GameObject agent)
		{
			//Add
			m_drawingGroupObjects.Add(agent);
		}

		/// <summary>
		/// [UnregisterAgent]
		/// Agentを登録解除する
		/// 引数1: agent
		/// </summary>
		public void UnregisterAgent(GameObject agent)
		{
			//Add
			m_drawingGroupObjects.Remove(agent);
		}

		/// <summary>
		/// [InstantiateAgent]
		/// Agentをインスタンス化する
		/// 引数1: enemy object
		/// 引数2: instantiate position
		/// 引数3: instantiate rotation
		/// 引数4: move target, used TargetMovement
		/// </summary>
		public void InstantiateAgent(GameObject instantiateObject,
			Vector3 position = new Vector3(), Quaternion rotation = new Quaternion(),
			Transform moveTarget = null)
		{
			//Instantiate
			GameObject instantiate = Instantiate(instantiateObject, transform);

			//transform設定
			instantiate.transform.position = position;
			instantiate.transform.rotation = rotation;
			
			////MoveTarget
			//var targetMovement = instantiate.GetComponent<AIFunction.TargetMovement>();
			//if (targetMovement != null)
			//	targetMovement.target = moveTarget;

			//NavMeshAgent設定
			var agent = instantiate.GetComponent<UnityEngine.AI.NavMeshAgent>();
			if (agent != null)
			{
				instantiate.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
				instantiate.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(position);
			}

			//AIAgent設定
			var aiAgent = instantiate.GetComponent<AIAgent>();
			if (aiAgent != null)
			{
				aiAgent.SetGroup(this);
			}
		}

		/// <summary>
		/// [DestroyGroupObjects]
		/// GroupにいるEnemyを全て削除する
		/// </summary>
		public void DestroyGroupObjects()
		{
			//Destroy
			foreach (var e in m_drawingGroupObjects)
				Destroy(e);
			//Clear
			m_drawingGroupObjects.Clear();
		}

		//Debug Only
#if UNITY_EDITOR
		/// <summary>[OnDrawGizmos]</summary>
		void OnDrawGizmos()
		{
			//OnDrawGizmos
			if (m_dIsDrawGizmos) m_patrolPoints.OnDrawGizmos();
		}
#endif
	}
}