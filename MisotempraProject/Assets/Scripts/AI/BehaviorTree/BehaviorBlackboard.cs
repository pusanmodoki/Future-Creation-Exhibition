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
				public object this[string key] { get { return m_this.m_keys[key.GetHashCode()].obj; } set { m_this.m_keys[key.GetHashCode()].obj = value; } }
				public object this[int key] { get { return m_this.m_keys[key].obj; } set { m_this.m_keys[key].obj = value; } }
				public AllKeyList(Blackboard board) { m_this = board; }

				Blackboard m_this;
			}
			public class KeyListStruct<T> where T : struct
			{
				public T this[string key] { get { return (T)m_this.m_keys[key.GetHashCode()].obj; } set { m_this.m_keys[key.GetHashCode()].obj = value; } }
				public T this[int key] { get { return (T)m_this.m_keys[key].obj; } set { m_this.m_keys[key].obj = value; } }
				public KeyListStruct(Blackboard board) { m_this = board; }

				Blackboard m_this;
			}
			public class KeyListClass<T> where T : class
			{
				public T this[string key] { get { return m_this.m_keys[key.GetHashCode()].obj as T; } set { m_this.m_keys[key.GetHashCode()].obj = value; } }
				public T this[int key] { get { return m_this.m_keys[key].obj as T; } set { m_this.m_keys[key].obj = value; } }

				public KeyListClass(Blackboard board) { m_this = board; }

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
			public struct Memo
			{
				public string this[string key] { get { return m_this.m_keys[key.GetHashCode()].memo; } }
				public string this[int key] { get { return m_this.m_keys[key].memo; } }

				public Memo(Blackboard board) { m_this = board; }
				Blackboard m_this;
			}
			public struct IsShared
			{
				public bool this[string key] { get { return m_this.m_keys[key.GetHashCode()].isShared; } }
				public bool this[int key] { get { return m_this.m_keys[key].isShared; } }

				public IsShared(Blackboard board) { m_this = board; }
				Blackboard m_this;
			}

			public static readonly string[] cKeyClassNames = new string[14]
			{
				typeof(GameObject).FullName,
				typeof(Transform).FullName,
				typeof(Component).FullName,
				typeof(AI.AIAgent).FullName,
				typeof(Quaternion).FullName,
				typeof(Vector2).FullName,
				typeof(Vector3).FullName,
				typeof(Vector4).FullName,
				typeof(string).FullName,
				typeof(System.Enum).FullName,
				typeof(int).FullName,
				typeof(float).FullName,
				typeof(double).FullName,
				typeof(object).FullName,
			};

			public AllKeyList allKeys { get; private set; }
			public KeyListClass<GameObject> gameObjects { get; private set; }
			public KeyListClass<Transform> transforms { get; private set; }
			public KeyListClass<Component> components { get; private set; }
			public KeyListClass<AI.AIAgent> aiAgents { get; private set; }
			public KeyListStruct<Quaternion> quaternions { get; private set; }
			public KeyListStruct<Vector2> vectors2 { get; private set; }
			public KeyListStruct<Vector3> vectors3 { get; private set; }
			public KeyListStruct<Vector4> vectors4 { get; private set; }
			public KeyListClass<string> strings { get; private set; }
			public KeyListClass<System.Enum> enums { get; private set; }
			public KeyListStruct<int> ints { get; private set; }
			public KeyListStruct<float> floats { get; private set; }
			public KeyListStruct<double> doubles { get; private set; }
			public KeyListClass<object> objects { get; private set; }
			public Memo memos { get; private set; }
			public IsShared isShareds { get; private set; }
			public string instanceKey { get; private set; }
			public bool isFirstInstance { get; private set; }

			public static int GetIntKey(string key) { return key.GetHashCode(); }
			public void OnDestroy()
			{
				if (m_instances != null && m_instances.ContainsKey(instanceKey))
					m_instances[instanceKey].Remove(this);
			}

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

			void NewPropertys()
			{
				allKeys = new AllKeyList(this);
				gameObjects = new KeyListClass<GameObject>(this);
				transforms = new KeyListClass<Transform>(this);
				components = new KeyListClass<Component>(this);
				aiAgents = new KeyListClass<AIAgent>(this);
				quaternions = new KeyListStruct<Quaternion>(this);
				vectors2 = new KeyListStruct<Vector2>(this);
				vectors3 = new KeyListStruct<Vector3>(this);
				vectors4 = new KeyListStruct<Vector4>(this);
				strings = new KeyListClass<string>(this);
				enums = new KeyListClass<System.Enum>(this);
				ints = new KeyListStruct<int>(this);
				floats = new KeyListStruct<float>(this);
				doubles = new KeyListStruct<double>(this);
				objects = new KeyListClass<object>(this);
				memos = new Memo(this);
				isShareds = new IsShared(this);
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