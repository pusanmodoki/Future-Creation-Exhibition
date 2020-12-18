using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

/// <summary>MisoTempra editor</summary>
namespace LocalEditor
{
	namespace TypeName
	{
		[CustomPropertyDrawer(typeof(global::TypeName))]
		public class TypeNameEditor : PropertyDrawer
		{
			SerializedProperty m_typeName = null;
			bool m_isInit = false;

			public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
			{
				return EditorGUIUtility.singleLineHeight * 2.0f;
			}

			public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
			{
				position.height = EditorGUIUtility.singleLineHeight;

				property.serializedObject.Update();
				if (m_typeName == null) m_typeName = property.FindPropertyRelative("m_typeName");

				if (!m_isInit)
				{
					if (m_typeName.stringValue == null || m_typeName.stringValue.Length == 0)
						m_typeName.stringValue = typeof(int).FullName;

					m_isInit = true;
				}


				EditorGUI.LabelField(position, property.displayName);
				{
					Rect rect = position;
					rect.x += EditorGUIUtility.labelWidth;
					if (EditorGUI.ToggleLeft(rect, "<-Set Value", false))
					{
						var window = EditorWindow.CreateInstance<TypeNameDetailWindow>();
						window.Initialize(this);
						window.Show();
					}
				}
				using (new EditorGUI.IndentLevelScope())
				{
					position.x += 5.0f;
					position.y += EditorGUIUtility.singleLineHeight;
					GUI.enabled = false;
					EditorGUI.TextField(position, m_typeName.stringValue);
					GUI.enabled = true;
				}

				property.serializedObject.ApplyModifiedProperties();
			}

			public void ChangeName(string name)
			{
				m_typeName.serializedObject.Update();
				m_typeName.stringValue = name;
				m_typeName.serializedObject.ApplyModifiedProperties();
			}
		}
	}
}