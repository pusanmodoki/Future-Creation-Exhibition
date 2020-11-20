//作成者 : 植村将太
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Colliderヒット時にSEを再生するCollisionEnterPlaySE
/// </summary>
public class CollisionEnterPlaySE : MonoBehaviour
{
	/// <summary>this SEPlayer</summary>
	[SerializeField, Tooltip("this SEPlayer")]
	SEPlayer m_sePlayer = null;
	/// <summary>Play SEPlayer index</summary>
	[SerializeField, Tooltip("Play SEPlayer index")]
	int m_playIndex = 0;
	/// <summary>Hit layer mask</summary>
	[SerializeField, Tooltip("Hit layer mask")]
	LayerMaskEx m_hitMask = int.MaxValue;

	/// <summary>[OnCollisionEnter]</summary>
	void OnCollisionEnter(Collision collision)
	{
		//当たったらSE再生
		if (m_hitMask.EqualBitsForGameObject(collision.gameObject))
			m_sePlayer.PlaySE(m_playIndex);
	}
	/// <summary>[OnTriggerEnter]</summary>
	void OnTriggerEnter(Collider collision)
	{
		//当たったらSE再生
		if (m_hitMask.EqualBitsForGameObject(collision.gameObject))
			m_sePlayer.PlaySE(m_playIndex);
	}
}
