using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;


namespace AI
{
	namespace BehaviorTree
	{
		public class Blackboard
		{
			public class AllKeyList
			{
				public KeyContent this[string key] { get { return m_this.m_keys[key.GetHashCode()]; } }
				public KeyContent this[int key] { get { return m_this.m_keys[key]; } }
				public AllKeyList(Blackboard board) { m_this = board; }

				Blackboard m_this;
			}
			public class KeyList<T>
			{
				public T this[string key] { get { return m_this.GetValue<T>(key); } set { m_this.SetValue(key, value); } }
				public T this[int key] { get { return m_this.GetValue<T>(key); } set { m_this.SetValue(key, value); } }

				public KeyList(Blackboard board) { m_this = board; }

				Blackboard m_this;
			}
			public class KeyContent
			{
				public object obj { get; set; }
				public string memo { get; private set; }
				public bool isShared { get; private set; }
				public int classIndex { get; private set; }

				public KeyContent(object obj, string memo, bool isShared, int classIndex)
				{
					this.obj = obj;
					this.memo = memo;
					this.isShared = isShared;
					this.classIndex = classIndex;
				}
			}
			public static readonly List<string> cDefaultKeys = new List<string>
			{
				"Animator",
				"DamageController"
			};

			/// <summary>キーリスト</summary>
			public AllKeyList allKeys { get; private set; }
			/// <summary>gameobjects</summary>
			public KeyList<GameObject> gameObjects { get; private set; }
			/// <summary>transforms</summary>
			public KeyList<Transform> transforms { get; private set; }
			/// <summary>components</summary>
			public KeyList<Component> components { get; private set; }
			/// <summary>ファイル名</summary>
			public string instanceKey { get; private set; }
			/// <summary>同じファイル名で一番最初に生成された場合true</summary>
			public bool isFirstInstance { get; private set; }

			/// <summary>keyをhash変換する, 計算負荷軽減用</summary>
			public static int GetHashCode(string key) { return key.GetHashCode(); }

			/// <summary>値を取得する</summary>
			public T GetValue<T>(string key)
			{
				int hash = key.GetHashCode();

#if UNITY_EDITOR
				var type = typeof(T);
				Debug.Assert(m_keys.ContainsKey(hash), "Blackboard->keyが登録されていません key: " + key);
				Debug.Assert(CastCheck<T>(hash, type), "Blackboard->キャストに失敗しました。 " +
					"\nkey: " + key + "generic type: " + type.FullName + "cast type:" + cKeyClassNames[m_keys[hash].classIndex]);
#endif

				return GetValue<T>(hash);
			}
			/// <summary>値を取得する</summary>
			public T GetValue<T>(int key)
			{
#if UNITY_EDITOR
				var type = typeof(T);
				Debug.Assert(m_keys.ContainsKey(key), "Blackboard->keyが登録されていません key: " + key);
				Debug.Assert(CastCheck<T>(key, type), "Blackboard->キャストに失敗しました。 " +
					"\nkey: " + key + "generic type: " + type.FullName + "cast type:" + cKeyClassNames[m_keys[key].classIndex]);
#endif
				return m_keys.ContainsKey(key) ? (T)m_keys[key].obj: default;
			}
			/// <summary>値を設定する</summary>
			public void SetValue<T>(string key, T value)
			{
				int hash = key.GetHashCode();

#if UNITY_EDITOR
				var type = typeof(T);
				Debug.Assert(m_keys.ContainsKey(hash), "Blackboard->keyが登録されていません key: " + key);
				Debug.Assert(CastCheck<T>(hash, type), "Blackboard->キャストに失敗しました。 " +
					"\nkey: " + key + "generic type: " + type.FullName + "cast type:" + cKeyClassNames[m_keys[hash].classIndex]);
#endif

				SetValue(hash, value);
			}
			/// <summary>値を設定する</summary>
			public void SetValue<T>(int key, T value)
			{
#if UNITY_EDITOR
				var type = typeof(T);
				Debug.Assert(m_keys.ContainsKey(key), "Blackboard->keyが登録されていません key: " + key);
				Debug.Assert(CastCheck<T>(key, type), "Blackboard->キャストに失敗しました。 " +
					"\nkey: " + key + "generic type: " + type.FullName + "cast type:" + cKeyClassNames[m_keys[key].classIndex]);
#endif
				m_keys[key].obj = value;
			}

			public static readonly System.Type[] cKeyClassTypes = new System.Type[14]
			{
				typeof(GameObject), typeof(Transform),
				typeof(Component), typeof(AI.AIAgent),
				typeof(Quaternion), typeof(Vector2),
				typeof(Vector3), typeof(Vector4),
				typeof(string), typeof(System.Enum),
				typeof(int), typeof(float),
				typeof(double), typeof(object),
			};
			public static readonly string[] cKeyClassNames = new string[14]
			{
				typeof(GameObject).FullName, typeof(Transform).FullName,
				typeof(Component).FullName, typeof(AI.AIAgent).FullName,
				typeof(Quaternion).FullName, typeof(Vector2).FullName,
				typeof(Vector3).FullName, typeof(Vector4).FullName,
				typeof(string).FullName, typeof(System.Enum).FullName,
				typeof(int).FullName, typeof(float).FullName,
				typeof(double).FullName, typeof(object).FullName,
			};
			public enum ClassIndexes : int
			{
				GameObject,
				Transform,
				Component,
				AIAgent,
				Quaternion,
				Vector2,
				Vector3,
				Vector4,
				String,
				Enum,
				Int,
				Float,
				Double,
				Object
			}

			static Dictionary<string, List<Blackboard>> m_instances = new Dictionary<string, List<Blackboard>>();
			Dictionary<int, KeyContent> m_keys = new Dictionary<int, KeyContent>();

			private Blackboard() { }
			public Blackboard(string instanceKey, List<int> classNameIndexes, List<string> keys,
				List<string> memos, List<bool> isShareds)
			{
				if (!m_instances.ContainsKey(instanceKey))
					m_instances.Add(instanceKey, new List<Blackboard>());
				m_instances[instanceKey].Add(this);

				Blackboard first = m_instances[instanceKey][0];
				bool isExists = m_instances[instanceKey].Count > 1;
				this.instanceKey = instanceKey;
				this.isFirstInstance = false;

				for (int i = 0; i < classNameIndexes.Count; ++i)
				{
					if (isShareds[i] && isExists)
					{
						int hash = keys[i].GetHashCode();
						m_keys.Add(hash, first.m_keys[hash]);
						continue;
					}

					NewClass(classNameIndexes[i], keys[i].GetHashCode(), memos[i], isShareds[i]);
				}

				NewPropertys();
			}
			public Blackboard(Blackboard clone)
			{
				instanceKey = clone.instanceKey;
				m_instances[instanceKey].Add(this);

				if (!m_instances[instanceKey][0].isFirstInstance)
				{
					isFirstInstance = true;
					m_instances[instanceKey][0].isFirstInstance = true;
				}
				else isFirstInstance = false;

				Blackboard first = m_instances[instanceKey][0];
				
				foreach (var key in first.m_keys)
				{
					if (key.Value.isShared)
					{
						m_keys.Add(key.Key, key.Value);
						continue;
					}

					NewClass(key.Value.classIndex, key.Key, key.Value.memo, key.Value.isShared);
				}

				NewPropertys();
			}
			public void RegisterKey<T>(string key, T obj, string memo, ClassIndexes classIndex)
			{
				m_keys.Add(key.GetHashCode(), new KeyContent(obj, memo, false, (int)classIndex));
			}
			public void OnDestroy()
			{
				if (m_instances != null && m_instances.ContainsKey(instanceKey))
					m_instances[instanceKey].Remove(this);
			}

			bool CastCheck<T>(int key, System.Type tType) 
			{
				return m_keys[key].classIndex == (int)ClassIndexes.Object
					|| cKeyClassTypes[m_keys[key].classIndex] == tType
					|| tType.IsSubclassOf(cKeyClassTypes[m_keys[key].classIndex]);
			}

			void NewPropertys()
			{
				allKeys = new AllKeyList(this);
				gameObjects = new KeyList<GameObject>(this);
				transforms = new KeyList<Transform>(this);
				components = new KeyList<Component>(this);
			}

			void NewClass(int index, int key, string memo, bool isShared)
			{
				switch (index)
				{
					case (int)ClassIndexes.GameObject:
						m_keys.Add(key, new KeyContent(null, memo, isShared, index));
						break;
					case (int)ClassIndexes.Transform:
						m_keys.Add(key, new KeyContent(null, memo, isShared, index));
						break;
					case (int)ClassIndexes.Component:
						m_keys.Add(key, new KeyContent(null, memo, isShared, index));
						break;
					case (int)ClassIndexes.AIAgent:
						m_keys.Add(key, new KeyContent(null, memo, isShared, index));
						break;
					case (int)ClassIndexes.Quaternion:
						m_keys.Add(key, new KeyContent(Quaternion.identity, memo, isShared, index));
						break;
					case (int)ClassIndexes.Vector2:
						m_keys.Add(key, new KeyContent(Vector2.zero, memo, isShared, index));
						break;
					case (int)ClassIndexes.Vector3:
						m_keys.Add(key, new KeyContent(Vector3.zero, memo, isShared, index));
						break;
					case (int)ClassIndexes.Vector4:
						m_keys.Add(key, new KeyContent(Vector4.zero, memo, isShared, index));
						break;
					case (int)ClassIndexes.String:
						m_keys.Add(key, new KeyContent("", memo, isShared, index));
						break;
					case (int)ClassIndexes.Enum:
						m_keys.Add(key, new KeyContent(null, memo, isShared, index));
						break;
					case (int)ClassIndexes.Int:
						m_keys.Add(key, new KeyContent(0, memo, isShared, index));
						break;
					case (int)ClassIndexes.Float:
						m_keys.Add(key, new KeyContent(0.0f, memo, isShared, index));
						break;
					case (int)ClassIndexes.Double:
						m_keys.Add(key, new KeyContent(0, memo, isShared, index));
						break;
					case (int)ClassIndexes.Object:
						m_keys.Add(key, new KeyContent(null, memo, isShared, index));
						break;
					default:
						m_keys.Add(key, new KeyContent(null, memo, isShared, index));
						break;
				}
			}
		}
	}
}