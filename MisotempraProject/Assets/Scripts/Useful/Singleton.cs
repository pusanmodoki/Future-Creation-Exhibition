//制作者: 植村将太
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Singleton
{
	namespace Detail
	{
		public abstract class BaseSingletonMonoBeehavior : MonoBehaviour
		{
			public static GameObject attachObject { get; private set; } = null;

			[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
			static void OnBeforeSceneLoad()
			{
				if (attachObject == null)
				{
					attachObject = new GameObject("Singletons");
					GameObject.DontDestroyOnLoad(attachObject);
				}
			}
		}
		public abstract class BaseGenericSingletonMonoBeehavior<T> : BaseSingletonMonoBeehavior 
			where T : MonoBehaviour
		{
			/// <summary>Awakeの代わり</summary>
			protected abstract void Init();
			protected void CallInit()
			{
				if (!m_isCalledInit)
				{
					m_isCalledInit = true;
					Init();
				}
			}

			/// <summary>インスタンスを設定, ある場合削除</summary>
			virtual protected void Awake()
			{
				if (m_instance == null)
				{
					m_instance = this as T;
					CallInit();
				}
				else if (!m_isThisInstance)
					Destroy(this);
			}

			/// <summary>自分がinstanceか否か</summary>
			protected bool m_isThisInstance { get { return m_instance == this; } }
			/// <summary>static instance</summary>
			protected static T m_instance = null;
			/// <summary>called init?</summary>
			bool m_isCalledInit = false;
		}
	}

	/// <summary>
	/// シングルトンのBaseとなるSingletonMonoBehaviour class
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[DefaultExecutionOrder(-1000)]
	public abstract class SingletonMonoBehaviour<T> : Detail.BaseGenericSingletonMonoBeehavior<T>
		where T : MonoBehaviour
	{
		/// <summary>static instance (get)</summary>
		public static T instance
		{
			get
			{
				if (m_instance == null)
				{
#if UNITY_EDITOR
						if (m_instance == null)
							Debug.LogError("instanceがnullです!!\n原因: アタッチされていない or DefaultExecutionOrderが-1000以下のクラスのAwakeで呼び出し");
#endif
				}
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
					if (m_instance != null)
						(m_instance as SingletonMonoBehaviour<T>).CallInit();
#if UNITY_EDITOR
					else
						Debug.LogError("instanceがnullです!!\n原因: アタッチされていない");
#endif
				}
				return m_instance;
			}
		}
	}


	/// <summary>
	/// Singletonsオブジェクト(DontDestroy)に自動アタッチされるシングルトンのBaseとなるDontDestroySingletonMonoBehaviour class
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[DefaultExecutionOrder(-1000)]
	public abstract class DontDestroySingletonMonoBehaviour<T> : Detail.BaseGenericSingletonMonoBeehavior<T>
		where T : MonoBehaviour
	{
		/// <summary>static instance (get)</summary>
		public static T instance 
		{
			get
			{
				if (m_instance == null)
				{
					m_instance = attachObject.AddComponent<T>();
					(m_instance as DontDestroySingletonMonoBehaviour<T>).CallInit();
				}
				return m_instance;
			}
		}

		new void Awake()
		{
			if (this.gameObject != attachObject)
			{
				Destroy(this);
				return;
			}
			base.Awake();
		}
	}
}