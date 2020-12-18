using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	[DisallowMultipleComponent, RequireComponent(typeof(AIAgent))]
	public class AIVision : MonoBehaviour
	{
        /// <summary>AIAgent</summary>
		public AIAgent aiAgent { get { return m_agent; } }
        /// <summary>Target transform</summary>
		public Transform targetTransform { get; private set; } = null;
		/// <summary>Target transform root</summary>
		public Transform targetTransformRoot { get; private set; } = null;
		public RaycastHit raycastHit { get { return m_raycastHit; } }
		/// <summary>Distance the vision</summary>
		public float distance{ get { return m_distance; } }
		/// <summary>Angle Visibility</summary>
		public float angle{ get { return m_angle; } }
		/// <summary>Height limit the vision</summary>
		public float heightLimit { get { return m_heightLimit; } }
		/// <summary>distance to lose sight the target</summary>
		public float loseSightDistance { get { return m_loseSightDistance; } }
		/// <summary>Raycast Center Position</summary>
		public Vector3 raycastCenter
		{
			get
			{
				Transform myTransform = transform;
				return m_raycastCenter.x * myTransform.right + m_raycastCenter.y * myTransform.up
					+ m_raycastCenter.z * myTransform.forward;
			}
		}
		public bool isDiscoverStay { get { ExecuteVision(); return m_isResult; } }
		public bool isDiscoverEnter { get { ExecuteVision(); return (m_isResult ^ m_isOldResult) & m_isResult; } }
		public bool isDiscoverExit { get { ExecuteVision(); return (m_isResult ^ m_isOldResult) & m_isOldResult; } }

		/// <summary>AIAgent</summary>
		[SerializeField, Tooltip("AIAgent")]
		AIAgent m_agent = null;
		[SerializeField]
		string m_targetTransformBlackboardKey = "";
		[SerializeField, Tooltip("見失った場合に現在のタスクに設定する後続タスク")]
		string m_loseSightSubsequentTaskKey = "";
		[SerializeField, Tooltip("見つけた場合に関数を再割り当てするか")]
		bool m_isDiscoverItRescheduleAI = false;

		/// <summary>Visibility Distance (forward)</summary>
		[SerializeField, Header("Visibility Info"), Tooltip("Visibility Distance (forward)")]
		float m_distance = 10;
		/// <summary>Visibility Angle</summary>
		[SerializeField, Range(0.0f, 180.0f), Tooltip("Visibility Angle")]
		float m_angle = 90;
		/// <summary>Visibility Height Limit (Y Axis [this + -Max] ~ [this + Max])</summary>
		[SerializeField, Tooltip("Visibility Height Limit (Y Axis [this + -Max] ~ [this + Max])")]
		float m_heightLimit = 5;
		/// <summary>lose sight distance</summary>
		[SerializeField, Tooltip("distance to lose sight the target")]
		float m_loseSightDistance = 5.0f;
		/// <summary>見失ったと判断する時間</summary>
		[SerializeField, Tooltip("見失ったと判断する時間")]
		float m_timeToLoseSight = 5.0f;

		/// <summary>Visibility Raycast Info</summary>
		[SerializeField, Header("Visibility Raycast Info"), Tooltip("Raycast Center Position")]
		Vector3 m_raycastCenter = Vector3.zero;
		/// <summary>Raycast LayerMask</summary>
		[SerializeField, Tooltip("Raycast LayerMask")]
		LayerMaskEx m_layerMask = int.MaxValue;

		/// <summary>SE Player</summary>
		[SerializeField, Tooltip("SE Player"), Space]
		SEPlayer m_sePlayer = null;
		/// <summary>Hit->Play SE Index</summary>
		[SerializeField, Tooltip("Hit->Play SE Index")]
		int m_indexOfSE = 0;
		/// <summary>SE Looping</summary>
		[SerializeField, Tooltip("SE Looping?")]
		bool m_isLoopSE = false;

		//debug only
#if UNITY_EDITOR
		/// <summary>Draw Gizmo Mesh (debug only)</summary>
		public Mesh dGizmoMesh { get; set; } = null;
		/// <summary>Draw Gizmo Mesh ID (debug only)</summary>
		public int dMeshID { get; set; } = -1;
		/// <summary>Discover? (debug only)</summary>
		public bool dIsDiscover { get { return m_dIsDiscover; } }
		/// <summary>Draw vision? (debug only)</summary>
		public bool dIsDrawGizmos { get { return m_dIsDrawGizmos; } set { m_dIsDrawGizmos = value; } }

		/// <summary>Visibility Discovered? (debug only)</summary>
		[SerializeField, Header("Debug Only"), Tooltip("Discover? (debug only)")]
		bool m_dIsDiscover = false;
		/// <summary>Draw vision? (debug only)</summary>
		[SerializeField, Tooltip("Draw vision? (debug only)")]
		bool m_dIsDrawGizmos = true;
		/// <summary>Drawing lose sight timer (debug only)</summary>
		[SerializeField, Tooltip("Drawing lose sight timer (debug only)")]
		float m_dLoseSightTimer = 0.0f;
#endif

		/// <summary>RaycastHit</summary>
		RaycastHit m_raycastHit = new RaycastHit();
		/// <summary>Lose sight timer</summary>
		Timer m_loseSightTimer = new Timer();
		/// <summary>Angle変換用</summary>
		float m_angleToCosine = 0.0f;
		/// <summary>最後に処理を行ったフレーム</summary>
		int m_lastExecuteFrameCount = 0;
		/// <summary>Result</summary>
		bool m_isResult = false;
		/// <summary>Old result</summary>
		bool m_isOldResult = false;

		// Start is called before the first frame update
		void Start()
		{
			//Transform取得
			targetTransform = aiAgent.behaviorTree.blackboard.transforms[m_targetTransformBlackboardKey];
			targetTransformRoot = targetTransform.root;
			//視界角度を変換
			m_angleToCosine = Mathf.Cos(m_angle * 0.5f * Mathf.Deg2Rad);
			m_loseSightTimer.Start();
		}
		/// <summary>[OnValidate]</summary>
		void OnValidate()
		{
			//視界角度を変換
			m_angleToCosine = Mathf.Cos(m_angle * 0.5f * Mathf.Deg2Rad);

			//debug only
#if UNITY_EDITOR
			//meshを変更するのでnullに
			dGizmoMesh = null;
#endif
		}
		// Update is called once per frame
		void Update()
		{
			ExecuteVision();

			if (m_isDiscoverItRescheduleAI && isDiscoverEnter)
				m_agent.behaviorTree.ForceReschedule();

			if (m_loseSightSubsequentTaskKey != null && m_loseSightSubsequentTaskKey.Length > 0 && isDiscoverExit)
				m_agent.behaviorTree.nowTask.task.RegisterSubsequentTask(m_loseSightSubsequentTaskKey);

			if (m_sePlayer != null && isDiscoverEnter)
				m_sePlayer.PlaySE(m_indexOfSE, m_isLoopSE);
		}

		void ExecuteVision()
		{
			if (m_lastExecuteFrameCount == Time.frameCount) return;

			m_lastExecuteFrameCount = Time.frameCount;

			//フラグ初期化
			m_isOldResult = m_isResult;
			m_isResult = true;

			//呼び出しコスト削減
			Vector3 thisPosition = transform.position;
			Vector3 targetPosition = targetTransform ? targetTransform.position : Vector3.zero;
			Vector3 center = raycastCenter;

			//ターゲットから自分への方向ベクトル
			Vector3 toTargetDirection = (targetPosition - (thisPosition + center)).normalized;
			//呼び出しコスト削減
			Vector3 centerPosition = thisPosition + center;

			//同位置？
			if (toTargetDirection.sqrMagnitude < Mathf.Epsilon)
			{
				//とりあえずRaycast
				Physics.Raycast(centerPosition, (targetPosition - thisPosition).normalized, out m_raycastHit, m_distance, m_layerMask);

				//TimerStart
				m_loseSightTimer.Start();

				//フラグ = true
				m_isResult = true;
#if UNITY_EDITOR
				m_dIsDiscover = true;
				m_dLoseSightTimer = m_loseSightTimer.elapasedTime;
#endif
				return;
			}

			//高さが設定以上離れている or 距離が設定以上離れている->return false
			if (Mathf.Abs(thisPosition.y - targetPosition.y) > m_heightLimit
				|| (targetPosition - thisPosition).sqrMagnitude > m_distance * m_distance)
				m_isResult = false;

			//Dotで視界角度内にいるか判定
			if (m_isResult && Vector3.Dot(transform.forward, toTargetDirection) < m_angleToCosine)
				m_isResult = false;

			//Raycast, ヒットしない->return false
			if (m_isResult && !Physics.Raycast(centerPosition, toTargetDirection, out m_raycastHit, m_distance, m_layerMask))
				m_isResult = false;

			//Raycastにヒットした物体がTargetの場合ヒットしたとする->return true
			if (m_isResult && m_raycastHit.transform.root != targetTransformRoot)
				m_isResult = false;

			//TimerStart
			if (m_isResult) m_loseSightTimer.Start();

			//Hit or 前フレーム見つけていたら距離判定をとる
			if (m_isResult || (m_isOldResult
				&& (targetPosition - thisPosition).sqrMagnitude < m_loseSightDistance * m_loseSightDistance
				&& m_loseSightTimer.elapasedTime <= m_timeToLoseSight))
			{
				//フラグ = true
				m_isResult = true;
#if UNITY_EDITOR
				m_dIsDiscover = true;
				m_dLoseSightTimer = m_loseSightTimer.elapasedTime;
#endif
			}
			else
			{
				m_isResult = false;
#if UNITY_EDITOR
				m_dIsDiscover = false;
				m_dLoseSightTimer = m_loseSightTimer.elapasedTime;
#endif
			}
		}


#if UNITY_EDITOR
		public void DExecuteVision()
		{
			m_loseSightTimer.Start();
			ExecuteVision();
		}
#endif
	}
}