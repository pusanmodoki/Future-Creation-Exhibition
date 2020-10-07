//制作者: 植村将太
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Timer
{
	/// <summary>開始時間</summary>
	public float startTime { get; private set; }
	/// <summary>経過時間</summary>
	public float elapasedTime
	{
		get
		{
#if UNITY_EDITOR
			if (!m_dIsStart) Debug.LogWarning("Timer::Startが未実行です");
#endif
			return Time.time - startTime;
		}
	}  
	/// <summary>Is Start?</summary>
	public bool isStart { get { return startTime > 0.0f; } }
	/// <summary>Is Stop?</summary>
	public bool isStop { get { return startTime < Mathf.Epsilon; } }

	//Debug only
#if UNITY_EDITOR
	/// <summary> Startを実行したかのフラグ(debug only) </summary>
	bool m_dIsStart;
#endif

	/// <summary> 計測を開始する </summary>
	public void Start()
	{
		startTime = Time.time;
		//Debug only, フラグ設定
#if UNITY_EDITOR
		m_dIsStart = true;
#endif
	}
	/// <summary> 計測を停止する </summary>
	public void Stop()
	{
		startTime = 0;
	}
}
