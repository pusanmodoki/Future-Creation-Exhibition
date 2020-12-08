using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 巡回ポイントリストとして使うPatrolPoints
/// </summary>
[System.Serializable]
public class AIPatrolPoints
{
	/// <summary>
	/// 一つのポイントを定義するPointData
	/// </summary>
	[System.Serializable]
	public class PointData
	{
		/// <summary>
		/// 次に進むポイントを定義するNext
		/// </summary>
		[System.Serializable]
		public struct Next
		{
			/// <summary>次に進むObjectのIndex (Points Element XX ＜- これ)</summary>
			[Tooltip("次に進むObjectのIndex (Points Element XX <- これ)")]
			public int nextElementIndex;
			/// <summary>選出確率 (合計1厳守)</summary>
			[Range(0.0f, 1.0f), Tooltip("選出確率 (合計1厳守)")]
			public float probability;
		}

		/// <summary>X, Y軸基準となるObject</summary>
		[Tooltip("X, Y軸基準となるObject")]
		public Transform transform;
		/// <summary>次の目標となるポイントリスト</summary>
		[Tooltip("次の目標となるポイント")]
		public List<Next> nextInfos;

		/// <summary>確率テーブル</summary>
		float[] m_probabilityTable;

		/// <summary>
		/// [NextIndex]
		/// return: 次に進むIndexをランダムに選出する
		/// </summary>
		public int NextIndex()
		{
			//Get random
			float random = Random.value;
			//テーブルから検索->見つけたらindex return
			for (int i = 0; i < m_probabilityTable.Length; i++)
				if (random < m_probabilityTable[i]) return nextInfos[i].nextElementIndex;
			//見つかりまへんでした
			return -1;
		}

		/// <summary>
		/// [AllocateTable]
		/// 初期化処理, 確率テーブルを生成する
		/// </summary>
		public void AllocateTable()
		{
			//配列を作成
			m_probabilityTable = new float[nextInfos.Count];
			//確率テーブル生成ループ (足し算してまわるだけ)
			for (int i = 0; i < nextInfos.Count; i++)
			{
				if (i > 0)
					m_probabilityTable[i] =
						m_probabilityTable[i - 1] + nextInfos[i].probability;
				else
					m_probabilityTable[i] = nextInfos[i].probability;
			}
		}
	}

	/// <summary>
	/// PatorolPointsの各関数で返り値として使用するPointResult
	/// </summary>
	public class PointResult
	{
		public PointResult(PointData pointData, float y)
		{
			Vector3 position = pointData.transform.position;
			position.y = position.y + y;
			this.pointData = pointData;
			this.position = position;
		}
		/// <summary>Point info</summary>
		public PointData pointData { get; private set; }
		/// <summary>目標座標</summary>
		public Vector3 position { get; private set; }
	}

	// Debug Only
#if UNITY_EDITOR
	/// <summary>Gizmo四角形描画サイズ</summary>
	static readonly Vector3 m_dPointsDrawSize = Vector3.one * 0.5f;
	/// <summary>Gizmo縦線描画の長さ (半分)</summary>
	static readonly float m_dPointsDrawHalfDistance = 10.0f;
	/// <summary>変更禁止! 各Indexの色</summary>
	[SerializeField, Tooltip("変更禁止! 各Indexの色")]
	Color[] m_dReadOnlyColorTable = new Color[13]
	{
		new Color(1.0f, 0.0f, 0.0f, 0.8f),
		new Color(0.0f, 1.0f, 0.0f, 0.8f),
		new Color(0.0f, 0.0f, 1.0f, 0.8f),
		new Color(1.0f, 1.0f, 0.0f, 0.8f),
		new Color(1.0f, 0.0f, 1.0f, 0.8f),
		new Color(0.0f, 1.0f, 1.0f, 0.8f),
		new Color(1.0f, 0.5f, 0.5f, 0.8f),
		new Color(0.5f, 1.0f, 0.5f, 0.8f),
		new Color(0.5f, 0.5f, 1.0f, 0.8f),
		new Color(1.0f, 1.0f, 0.5f, 0.8f),
		new Color(1.0f, 0.5f, 1.0f, 0.8f),
		new Color(0.5f, 1.0f, 1.0f, 0.8f),
		new Color(1.0f, 1.0f, 1.0f, 0.8f),
	};
	/// <summary>描画を1秒毎にするために測ります</summary>
	double m_drawTime = 0.0f;
#endif

	/// <summary>Pointを配置する高さ (配置数)</summary>
	[SerializeField, Tooltip("Pointを配置する高さ (0は自動で追加)")]
	List<float> m_installationHeights = new List<float>();
	/// <summary>Point Datas</summary>
	[SerializeField, Tooltip("Point Datas")]
	List<PointData> m_points = new List<PointData>();

	/// <summary>パトロールポイントのポジションたち(X, Zのみ参照)</summary>
	Vector3[] m_positions = null;

	/// <summary>[Start]</summary>
	public void Start()
	{
		if (!m_installationHeights.Contains(0.0f))
			m_installationHeights.Add(0.0f);

		//Positionリスト確保
		m_positions = new Vector3[m_points.Count];

		//Positionリスト設定ループ
		for (int i = 0; i < m_points.Count; ++i)
		{
			//参照トランスフォームがある場合
			if (m_points[i].transform != null)
			{
				//代入, ポイントの確率テーブルをついでに生成
				m_positions[i] = m_points[i].transform.position;
				m_points[i].AllocateTable();
			}
			//なかった場合は0を代入
			else
				m_positions[i] = Vector3.zero;
		}
	}
	/// <summary>[OnValidate]</summary>
	public void OnValidate()
	{
		//Start実行
		Start();
	}

	/// <summary>
	/// [NextPoint]
	/// 第一引数を基に次のポイントを検索する
	/// </summary>
	public PointResult NextPoint(PointResult data, Vector3 position)
	{
		//次のindexを検索
		int next = data.pointData.NextIndex();

		//次のインデックス、見つかりません
		if (next == -1)
		{
			//Error
			Debug.LogError("PatrolPoints NextPoint Not found!! Points Name : "
				+ data.pointData.transform.name);
			return new PointResult(new PointData(), 0.0f);
		}

		//使用する高さIndex
		int useHeightIndex = 0;
		//最短距離を保存
		float minDistance = 1000.0f;
		float positionY = m_points[next].transform.position.y;
		//検索ループ
		for (int i = 0; i < m_installationHeights.Count; i++)
		{
			//各高さまでの距離を取得して最短の高さを求める
			float abs = Mathf.Abs(position.y - positionY + m_installationHeights[i]);
			if (minDistance > abs)
			{
				minDistance = abs;
				useHeightIndex = i;
			}
		}

		//Positionと高さを基にResultを作成
		return new PointResult(m_points[next], m_installationHeights[useHeightIndex]);
	}

	/// <summary>
	/// [ShortestAvoidPoint]
	/// 現在座標から最短の回避ポイントを検索する
	/// 引数1: 現在座標
	/// 引数2: 回避対象の座標
	/// 引数3: 回避に最低限必要な距離
	/// </summary>
	public PointResult ShortestAvoidPoint(Vector3 nowPosition, Vector3 targetPosition, float avoidDistance)
	{
		//使用するポイントのIndex
		int index = 0;
		//使用する高さのIndex
		int useHeightIndex = 0;
		//最短距離を保存
		float minDistance = 100000.0f;

		//最短距離初期化
		minDistance = 10000.0f;
		//最短ポイント検索ループ
		for (int i = 0; i < m_positions.Length; ++i)
		{
			//自身からの距離とターゲットからの距離を求める
			float distance = (nowPosition - m_positions[i]).sqrMagnitude;
			float distance2 = (targetPosition - m_positions[i]).sqrMagnitude;

			//最短座標を更新 & 回避距離の外にある場合のみIndex更新
			if (distance < minDistance && distance2 > avoidDistance * avoidDistance)
			{
				minDistance = distance;
				index = i;
			}
		}

		//高さIndex検索ループ
		float positionY = m_points[index].transform.position.y;
		for (int i = 0; i < m_installationHeights.Count; i++)
		{
			//各高さまでの距離を取得して最短の高さを求める
			float abs = Mathf.Abs(nowPosition.y - positionY + m_installationHeights[i]);
			if (minDistance > abs)
			{
				minDistance = abs;
				useHeightIndex = i;
			}
		}

		//求めたIndex, Height indexを基にResultを作成
		return new PointResult(m_points[index], m_installationHeights[useHeightIndex]);
	}

	/// <summary>
	/// [ShortestAvoidPoint]
	/// 現在座標から最短のポイントを検索する
	/// 引数1: 現在座標
	/// </summary>
	public PointResult ShortestPoint(Vector3 nowPosition)
	{
		//使用するポイントのIndex
		int index = 0;
		//使用する高さのIndex
		int useHeightIndex = 0;
		//最短距離を保存
		float minDistance = 100000.0f;

		//最短距離初期化
		minDistance = 10000.0f;
		//最短ポイント検索ループ
		for (int i = 0; i < m_positions.Length; ++i)
		{
			//距離を取得して最短座標を求める
			float distance = (nowPosition - m_positions[i]).sqrMagnitude;
			if (distance < minDistance)
			{
				minDistance = distance;
				index = i;
			}
		}

		//高さIndex検索ループ
		float positionY = m_points[index].transform.position.y;
		for (int i = 0; i < m_installationHeights.Count; i++)
		{
			//各高さまでの距離を取得して最短の高さを求める
			float abs = Mathf.Abs(nowPosition.y - positionY + m_installationHeights[i]);
			if (minDistance > abs)
			{
				minDistance = abs;
				useHeightIndex = i;
			}
		}

		//求めたIndex, Height indexを基にResultを作成
		return new PointResult(m_points[index], m_installationHeights[useHeightIndex]);
	}


	//Debug Only
#if UNITY_EDITOR
	/// <summary>
	/// [OnDrawGizmos]
	/// Gizmoを描画する
	/// </summary>
	public void OnDrawGizmos()
	{
		//停止中の場合1秒毎に初期化を行う
		if ((!UnityEditor.EditorApplication.isPlaying
			|| UnityEditor.EditorApplication.isPaused
			|| m_positions == null)
			&& UnityEditor.EditorApplication.timeSinceStartup > m_drawTime)
		{
			Start();
			m_drawTime = UnityEditor.EditorApplication.timeSinceStartup + 1.0f;
		}

		Vector3 down = Vector3.zero;

		for (int i = 0, max = m_positions.Length; i < max; ++i)
		{
			//各ポイントごとにY座標を少しずらす
			down += Vector3.down * 0.1f;

			//縦線描画
			Gizmos.color = m_dReadOnlyColorTable[i % m_dReadOnlyColorTable.Length];
			Gizmos.DrawLine(m_positions[i] + Vector3.up * m_dPointsDrawHalfDistance,
				m_positions[i] + Vector3.up * -m_dPointsDrawHalfDistance);

			//高さリストを基に四角を描画
			foreach (var e in m_installationHeights)
				Gizmos.DrawWireCube(new Vector3(m_positions[i].x, m_positions[i].y + e, m_positions[i].z), m_dPointsDrawSize);
			Gizmos.DrawWireCube(m_positions[i], m_dPointsDrawSize * 1.3f);

			//NextIndexを基に線描画ループ
			Gizmos.color = m_dReadOnlyColorTable[i % m_dReadOnlyColorTable.Length];
			foreach (var next in m_points[i].nextInfos)
			{
				//ポイントからポイントへ中間, 上辺, 下辺に線を描画する
				Gizmos.DrawLine(m_positions[i] + down + Vector3.up * m_dPointsDrawHalfDistance,
							m_positions[next.nextElementIndex] + down + Vector3.up * m_dPointsDrawHalfDistance);
				Gizmos.DrawLine(m_positions[i] + down + Vector3.up * -m_dPointsDrawHalfDistance,
						  m_positions[next.nextElementIndex] + down + Vector3.up * -m_dPointsDrawHalfDistance);
				Gizmos.DrawLine(m_positions[i] + down, m_positions[next.nextElementIndex] + down);
			}
		}
	}
#endif
}
