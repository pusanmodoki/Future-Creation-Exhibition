//作成者 : 植村将太
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ColliderをN秒後無効化するColliderDisableSeconds
/// </summary>
public class ColliderDisableSeconds : MonoBehaviour
{
	/// <summary>Collider disabled seconds</summary>
	[SerializeField, Tooltip("Collider disabled seconds")]
	float m_disableTime = 1.0f;
	/// <summary>Disabled Collides</summary>
	[SerializeField, Tooltip("Disabled Collides")]
	List<Collider> m_colliders = new List<Collider>();
	/// <summary>Disabled Collisions</summary>
	[SerializeField, Tooltip("Disabled Collisions")]
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
		//指定秒数経過したらenabled = falseに
        if (!m_isCompleted && m_timer.elapasedTime > m_disableTime)
		{
			m_isCompleted = true;
			foreach (var e in m_colliders)
				e.enabled = false;
			foreach (var e in m_collisions)
				e.collider.enabled = false;
		}
    }
}
