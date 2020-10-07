//制作者: 植村将太
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シングルトンのBaseとなるSingletonMonoBehaviour class
/// </summary>
/// <typeparam name="T"></typeparam>
[DefaultExecutionOrder(-1000)]
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
	/// <summary>static instance (get)</summary>
	public static T instance
	{
		get
		{
#if UNITY_EDITOR
			if (m_instance == null)
				Debug.LogError("instanceがnullです!!\n原因: アタッチされていない or DefaultExecutionOrderが-1000以下のクラスのAwakeで呼び出し");
#endif
			return m_instance;
		}
	}
	/// <summary>instanceがnullの場合検索を行った後に返却する, DefaultExecutionOrderが-1000以下のクラスからAwakeを呼び出す際有効</summary>
	public static T awakeFindInstance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = FindObjectOfType<T>();
				if (m_instance == null) Debug.LogError("instanceがnullです!!\n原因: アタッチされていない");
				else
				{
					var cast = (m_instance as SingletonMonoBehaviour<T>);
					if (!(m_instance as SingletonMonoBehaviour<T>).m_isCalledInit)
					{
						cast.Init();
						cast.m_isCalledInit = true;
					}
				}
			}
			return m_instance;
		}
	}
	/// <summary>自分がinstanceか否か</summary>
	protected bool m_isThisInstance { get { return m_instance == this; } }

	/// <summary>static instance</summary>
	static T m_instance= null;
	/// <summary>called init?</summary>
	bool m_isCalledInit = false;

	/// <summary>インスタンスを設定, ある場合削除</summary>
	virtual protected void Awake()
	{
		if (m_instance == null)
		{
			m_instance = this as T;
			if (!m_isCalledInit)
			{
				Init();
				m_isCalledInit = true;
			}
		}
		else if (!m_isThisInstance)
			Destroy(this);
	}
	/// <summary>Awakeの代わり</summary>
	protected abstract void Init();
}
