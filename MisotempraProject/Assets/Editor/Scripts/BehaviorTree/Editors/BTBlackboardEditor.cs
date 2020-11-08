using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEditor;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		[CustomEditor(typeof(ScriptableObject.BTBlackboardScriptableObject))]
		public class BTBlackboardEditor : UnityEditor.Editor
		{
			static readonly string[] m_cSelectClasseNames = new string[15]
			{
				"GameObject",
				"Transform",
				"Component",
				"AIAgent",
				"Quaternion",
				"Vector2",
				"Vector3",
				"Vector4",
				"string",
				"enum",
				"int",
				"float",
				"double",
				"Unity Object",
				"C# object"
			};


			SerializedProperty m_classeNameIndexes = null;
			SerializedProperty m_keys = null;
			SerializedProperty m_memos = null;
			SerializedProperty m_isStatics = null;

			[SerializeField]
			List<bool> m_isFoldouts = new List<bool>();

			Dictionary<string, List<SerializedProperty>> m_keyNamesDictionary = new Dictionary<string, List<SerializedProperty>>();

			void OnEnable()
			{
				SerializedProperty container = serializedObject.FindProperty("m_cashContainer");
				m_classeNameIndexes = container.FindPropertyRelative("m_classeNameIndexes");
				m_keys = container.FindPropertyRelative("m_keys");
				m_memos = container.FindPropertyRelative("m_memos");
				m_isStatics = container.FindPropertyRelative("m_isStatics");

				m_keyNamesDictionary.Clear();
				for (int i = 0; i < m_classeNameIndexes.arraySize; ++i)
				{
					if (m_isFoldouts.Count < m_classeNameIndexes.arraySize)
						m_isFoldouts.Add(false);

					var element = m_keys.GetArrayElement(i);
					string key = element.stringValue;

					if (!m_keyNamesDictionary.ContainsKey(key))
						m_keyNamesDictionary.Add(key, new List<SerializedProperty>());
					m_keyNamesDictionary[key].Add(element);
				}
			}

			public override void OnInspectorGUI()
			{
				List<SerializedProperty> keysTemp = new List<SerializedProperty>();

				serializedObject.Update();
				for (int i = 0; i < m_classeNameIndexes.arraySize; ++i)
				{
					var key = m_keys.GetArrayElementAtIndex(i);
					string title = "Element[" + i + "] :  ";
					string keyString = key.stringValue;
					bool isKeyEmpty = keyString.Length == 0;
					bool isExists = m_keyNamesDictionary[keyString].Count > 1;
					GUIStyle style = EditorStyles.foldout;

					keysTemp.Add(key);
					title += isKeyEmpty ?
						"Key name empty!!!" : isExists ?
						"Key name duplicate!!!" : keyString;

					if (isKeyEmpty | isExists)
					{
						style.normal = new GUIStyleState() { textColor = Color.red };
						style.onNormal = style.normal;
					}
					else
					{
						style.normal = new GUIStyleState() { textColor = Color.black };
						style.onNormal = style.normal;
					}

					using (new EditorGUILayout.HorizontalScope())
					{
						m_isFoldouts[i] = EditorGUILayout.Foldout(m_isFoldouts[i], title, true, style);
						GUILayout.FlexibleSpace();
						if (GUILayout.Button(" Delete "))
						{
							m_classeNameIndexes.DeleteArrayElementAtIndex(i);
							m_keys.DeleteArrayElementAtIndex(i);
							m_isStatics.DeleteArrayElementAtIndex(i);
							m_memos.DeleteArrayElementAtIndex(i);
							m_isFoldouts.RemoveAt(i);

							--i;
							continue;
						}
					}
					if (m_isFoldouts[i])
					{
						using (new EditorGUI.IndentLevelScope())
						{
							keyString = EditorGUILayout.TextField("Key name: ", keyString);
							if (GUI.changed)
								m_keys.GetArrayElementAtIndex(i).stringValue = keyString;

							m_memos.GetArrayElementAtIndex(i).stringValue =
								EditorGUILayout.TextField("Memo: ", m_memos.GetArrayElementAtIndex(i).stringValue);

							m_classeNameIndexes.GetArrayElement(i).intValue =
								EditorGUILayout.Popup("Type: ", m_classeNameIndexes.GetArrayElement(i).intValue, m_cSelectClasseNames);

							m_isStatics.GetArrayElement(i).boolValue =
								EditorGUILayout.Toggle("IsStatic: ", m_isStatics.GetArrayElement(i).boolValue);
						}
					}
				}

				using (new EditorGUILayout.HorizontalScope())
				{
					GUILayout.FlexibleSpace();
					if (GUILayout.Button("  New key  "))
						AddElement(keysTemp);
				}

				{
					GUIStyle style = EditorStyles.foldout;
					style.onNormal = style.normal = new GUIStyleState();
					serializedObject.ApplyModifiedProperties();
				}

				m_keyNamesDictionary.Clear();
				foreach(var property in keysTemp)
				{
					if (!m_keyNamesDictionary.ContainsKey(property.stringValue))
						m_keyNamesDictionary.Add(property.stringValue, new List<SerializedProperty>());
					m_keyNamesDictionary[property.stringValue].Add(property);
				}
			}

			void AddElement(List<SerializedProperty> keysTemp)
			{
				m_classeNameIndexes.ArrayAddEmpty();
				m_keys.ArrayAddEmpty();
				m_isStatics.ArrayAddEmpty();
				m_memos.ArrayAddEmpty();
				m_isFoldouts.Add(false);

				m_classeNameIndexes.ArrayBack().intValue = 0;
				m_keys.ArrayBack().stringValue = "";
				m_isStatics.ArrayBack().boolValue = false;
				m_memos.ArrayBack().stringValue = "";

				keysTemp.Add(m_keys.ArrayBack());
			}
		}
	}
}