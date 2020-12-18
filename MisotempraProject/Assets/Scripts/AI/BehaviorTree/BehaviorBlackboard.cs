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
				public T this[string key] { get { return m_this.GetValue<T>(key.GetHashCode()); } set { m_this.SetValue(key.GetHashCode(), value); } }
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
			public void SetValue(string key, object value)
			{
				int hash = key.GetHashCode();
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


			const int m_cIndexGameObject = 0;
			const int m_cIndexTransform = 1;
			const int m_cIndexComponent = 2;
			const int m_cIndexAIAgent = 3;
			const int m_cIndexQuaternion = 4;
			const int m_cIndexVector2 = 5;
			const int m_cIndexVector3 = 6;
			const int m_cIndexVector4 = 7;
			const int m_cIndexString = 8;
			const int m_cIndexEnum = 9;
			const int m_cIndexInt = 10;
			const int m_cIndexFloat = 11;
			const int m_cIndexDouble = 12;
			const int m_cIndexObject = 13;

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
			public void OnDestroy()
			{
				if (m_instances != null && m_instances.ContainsKey(instanceKey))
					m_instances[instanceKey].Remove(this);
			}

			bool CastCheck<T>(int key, System.Type tType) 
			{
				return cKeyClassTypes[m_keys[key].classIndex] == tType
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
					case m_cIndexGameObject:
						m_keys.Add(key, new KeyContent(null, memo, isShared, index));
						break;
					case m_cIndexTransform:
						m_keys.Add(key, new KeyContent(null, memo, isShared, index));
						break;
					case m_cIndexComponent:
						m_keys.Add(key, new KeyContent(null, memo, isShared, index));
						break;
					case m_cIndexAIAgent:
						m_keys.Add(key, new KeyContent(null, memo, isShared, index));
						break;
					case m_cIndexQuaternion:
						m_keys.Add(key, new KeyContent(Quaternion.identity, memo, isShared, index));
						break;
					case m_cIndexVector2:
						m_keys.Add(key, new KeyContent(Vector2.zero, memo, isShared, index));
						break;
					case m_cIndexVector3:
						m_keys.Add(key, new KeyContent(Vector3.zero, memo, isShared, index));
						break;
					case m_cIndexVector4:
						m_keys.Add(key, new KeyContent(Vector4.zero, memo, isShared, index));
						break;
					case m_cIndexString:
						m_keys.Add(key, new KeyContent("", memo, isShared, index));
						break;
					case m_cIndexEnum:
						m_keys.Add(key, new KeyContent(null, memo, isShared, index));
						break;
					case m_cIndexInt:
						m_keys.Add(key, new KeyContent(0, memo, isShared, index));
						break;
					case m_cIndexFloat:
						m_keys.Add(key, new KeyContent(0.0f, memo, isShared, index));
						break;
					case m_cIndexDouble:
						m_keys.Add(key, new KeyContent(0, memo, isShared, index));
						break;
					case m_cIndexObject:
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