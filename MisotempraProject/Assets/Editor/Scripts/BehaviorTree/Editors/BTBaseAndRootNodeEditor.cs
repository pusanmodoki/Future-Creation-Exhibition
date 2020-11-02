using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		/// <summary>Editor classes</summary>
		namespace NodeEditor
		{
			public class BTBaseNodeEditor : UnityEditor.Editor
			{
				protected void DrawDecoratorProperty()
				{

				}
			}

			[CustomEditor(typeof(ScriptableObject.BTRootScriptableObject))]
			public class BTRootNodeEditor : BTBaseNodeEditor
			{
				SerializedProperty m_memo = null;

				void OnEnable()
				{
					m_memo = serializedObject.FindProperty("m_memo");
				}

				public override void OnInspectorGUI()
				{

					string temp = m_memo.stringValue;
					temp = EditorGUILayout.TextField(temp);
					if (GUI.changed)
					{
						m_memo.stringValue = temp;
					}

					GUILayout.Button("SS");
				}
			}
		}
	}
}