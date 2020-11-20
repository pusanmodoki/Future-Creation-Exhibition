//作成者 : 植村将太
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ColliderをN秒後有効化するColliderEnableSeconds
/// </summary>
public class ColliderEnableSeconds : MonoBehaviour
{
	/// <summary>Collider enabled seconds</summary>
	[SerializeField, Tooltip("Collider enabled seconds")]
	float m_enableTime = 1.0f;
	/// <summary>Enabled Collides</summary>
	[SerializeField, Tooltip("Enabled Collides")]
	List<Collider> m_colliders = new List<Collider>();
	/// <summary>Enabled Collisions</summary>
	[SerializeField, Tooltip("Enabled Collisions")]
	List<Collision> m_collisions = new List<Collision>();

	/// <summary>Timer</summary>
	Timer m_timer = new Timer();
	/// <summary>変更終わりました</summary>
	bool m_isCompleted = false;

	/// <summary>[Start]</summary>
	void Start()
	{
		//計測開始
		m_timer.Start();
	}

	/// <summary>[Update]</summary>
	void Update()
    {
		//指定秒数経過したらenabled = trueに
        if (!m_isCompleted && m_timer.elapasedTime > m_enableTime)
		{
			m_isCompleted = true;
			foreach (var e in m_colliders)
				e.enabled = true;
			foreach (var e in m_collisions)
				e.collider.enabled = true;
		}
    }
}
