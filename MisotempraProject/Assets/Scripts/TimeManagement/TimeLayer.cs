//制作者: 植村将太
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

/// <summary>Time management</summary>
namespace TimeManagement
{
	/// <summary>DeltaTimeをLayer形式で提供するTimeLayer class</summary>
	[System.Serializable]
	public class TimeLayer
	{
		/// <summary>マネージャー呼び出しの関数をまとめたCallManagerFunctions structure</summary>
		public struct CallManagerFunctions
		{
			/// <summary>コンストラクタ</summary>
			/// <param name="layer"></param>
			public CallManagerFunctions(TimeLayer layer)
			{
				m_layer = layer;
			}


			/// <summary>Awake(TimeManager呼び出し用)</summary>
			public static void Awake(TimeLayer root, ReadOnlyCollection<TimeLayer> layers) { TimeLayer.root = root; TimeLayer.layers = layers; }
			/// <summary>Add children layer(debug時はList.Add(layer), Release時はArray[index] = layer)</summary>
			public void AddChildren(TimeLayer layer)
			{
				m_layer.m_childrens.Add(layer);
			}
			/// <summary>Set parent layer(TimeManager呼び出し用)</summary>
			public void SetParent(TimeLayer layer) { m_layer.parent = layer; }
			/// <summary>Set layer name(TimeManager呼び出し用)</summary>
			public void SetName(string set) { m_layer.name = set; }
			/// <summary>Set layer time scale(TimeManager呼び出し用)</summary>
			public void SetScale(float scale) { m_layer.timeScale = scale; }

			/// <summary>Update(TimeManager呼び出し用)</summary>
			public void Update(float deltaTime)
			{
				m_layer.deltaTime = deltaTime * m_layer.timeScale;
				foreach (var e in m_layer.m_childrens)
					e.callManagerFunctions.Update(m_layer.deltaTime);
			}
			/// <summary>FixedUpdate(TimeManager呼び出し用)</summary>
			public void FixedUpdate(float fixedDeltaTime)
			{
				m_layer.fixedDeltaTime = fixedDeltaTime * m_layer.timeScale;
				foreach (var e in m_layer.m_childrens)
					e.callManagerFunctions.FixedUpdate(m_layer.fixedDeltaTime);
			}

			//Debug only
#if UNITY_EDITOR
			/// <summary>Delete children layer(debug only)</summary>
			public List<string> DDeleteChildren(TimeLayer layer)
			{
				List<string> result = new List<string>();
				DDeleteChildrenRecursion(layer, result);
				m_layer.m_childrens.Remove(layer);
				return result;
			}
			/// <summary>Delete children layer(debug only, 再帰関数用)</summary>
			void DDeleteChildrenRecursion(TimeLayer layer, List<string> list)
			{
				while (layer.m_childrens.Count > 0)
				{
					DDeleteChildrenRecursion(layer.m_childrens[0], list);
					list.Add(layer.m_childrens[0].guid);
					layer.m_childrens.RemoveAt(0);
				}
			}
			/// <summary>delta timeを即時計算(debug only, 再帰関数用)</summary>
			public float DCalculateScale()
			{
				float result = 1.0f;
				for (var layer = m_layer; layer != null; layer = layer.parent)
					result *= layer.timeScale;
				return result;
			}
#endif

			/// <summary>this layer</summary>		
			TimeLayer m_layer;
		}

		/// <summary>static root layer instance</summary>
		public static TimeLayer root { get; private set; } = null;
		/// <summary>static layers</summary>
		public static ReadOnlyCollection<TimeLayer> layers { get; private set; } = null;

		/// <summary>parent layer</summary>
		public TimeLayer parent { get; private set; } = null;
		/// <summary>children layers</summary>
		public ReadOnlyCollection<TimeLayer> childrens { get; private set; } = null;
		/// <summary>layer name</summary>
		public string name { get; private set; } = null;
		/// <summary>this delta time</summary>
		public float deltaTime { get; private set; } = 0.0f;
		/// <summary>this fixed delta time</summary>
		public float fixedDeltaTime { get; private set; } = 0.0f;
		/// <summary>this time scale</summary>
		public float timeScale { get; private set; } = 1.0f;
		/// <summary>this guid</summary>
		public string guid { get { return m_guid; } private set { m_guid = value; } }
		/// <summary>call manager functions structure</summary>
		public CallManagerFunctions callManagerFunctions { get; private set; } = default;

		/// <summary>set time scale</summary>
		public void SetTimeScale(float scale) { timeScale = scale; }

		/// <summary>guid</summary>
		[SerializeField, HideInInspector]
		string m_guid = "";
		/// <summary>children layers</summary>
		[System.NonSerialized]
		List<TimeLayer> m_childrens = null;

		/// <summary>コンストラクタ(TimeManager呼び出し用)</summary>
		public TimeLayer(string name, float timeScale, string guid)
		{
			this.name = name;
			parent = null;
			this.timeScale = timeScale;
			m_childrens = new List<TimeLayer>();
			childrens = new ReadOnlyCollection<TimeLayer>(m_childrens);
			m_guid = guid;
			callManagerFunctions = new CallManagerFunctions(this);
		}
		
		/// <summary>Init layer(Start or Awakeで必ず一度呼び出すこと)</summary>
		public static void InitLayer(ref TimeLayer layer)
		{
			layer = FindByGuid(layer.guid);
		}

		/// <summary>Find layer(name)</summary>
		public static TimeLayer Find(string name)
		{
			foreach (var e in layers) if (e.name == name) return e;
			return null;
		}
		/// <summary>Find layer(name)</summary>
		public static TimeLayer[] FindAll(string name)
		{
			List<TimeLayer> result = new List<TimeLayer>();
			foreach(var e in layers)
				if (e.name == name) result.Add(e);
			return result.ToArray();
		}
		/// <summary>Find layer(layer guid)</summary>
		public static TimeLayer FindByGuid(string guid)
		{
#if UNITY_EDITOR
			if (!TimeManager.instance.layerIndexes.ContainsKey(guid))
			{
				Debug.LogError("TimeManager->FindByGuid, キーが見つかりません. key: " + guid);
				return null;
			}
#endif
			return layers[TimeManager.instance.layerIndexes[guid]];
		}
	}

	/// <summary>TimeManagement detail</summary>
	namespace Detail
	{
		//セーブ用タイムレイヤー
		[System.Serializable]
		public class SaveTimeLayer
		{
			/// <summary>コンストラクタ</summary>
			public SaveTimeLayer(string name, string parentGuid,
				List<string> childrensGuid, float timeScale)
			{
				m_name = name;
				m_parentGuid = parentGuid;
				m_timeScale = timeScale;
				if (childrensGuid != null)
				{
					for (int i = 0; i < childrensGuid.Count; ++i)
						m_childrensGuid.Add(childrensGuid[i]);
				}

				m_guid = System.Guid.NewGuid().ToString();
			}

			/// <summary>layer name</summary>
			public string name { get { return m_name; } }
			/// <summary>layer parent guid</summary>
			public string parentGuid { get { return m_parentGuid; } }
			/// <summary>layer childrens guid</summary>
			public List<string> childrensGuid { get { return m_childrensGuid; } }
			/// <summary>layer time scale</summary>
			public float timeScale { get { return m_timeScale; } }
			/// <summary>layer guid</summary>
			public string guid { get { return m_guid; } }

			/// <summary>layer name</summary>
			[SerializeField]
			string m_name;
			/// <summary>layer parent guid</summary>
			[SerializeField]
			string m_parentGuid = null;
			/// <summary>layer childrens guid</summary>
			[SerializeField]
			List<string> m_childrensGuid = new List<string>();
			/// <summary>layer time scale</summary>
			[SerializeField]
			float m_timeScale = 0.0f;
			/// <summary>layer guid</summary>
			[SerializeField, HideInInspector]
			string m_guid = null;

			/// <summary>set layer name</summary>
			public void SetName(string name) { m_name = name; }
			/// <summary>set layer time scale</summary>
			public void SetScale(float scale) { m_timeScale = scale; }
			/// <summary>add layer children guid</summary>
			public void AddChildren(string guid) { m_childrensGuid.Add(guid); }
		}
	}
}