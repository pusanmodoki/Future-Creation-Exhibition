using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		/// <summary>Editor classes</summary>
		namespace NodeEditor
		{
			/// <summary>ReorderableList classes</summary>
			namespace ReorderableLists
			{
				public class ServiceList : ClassList
				{
					public ServiceList(BTBaseNodeEditor editor, SerializedProperty useProperty, System.Type baseType, string title)
						: base(editor, useProperty, baseType, title)
					{
					}

					protected override void InitializeList(Type baseType)
					{
						list.drawHeaderCallback += (rect) => EditorGUI.LabelField(rect, m_title);
						list.elementHeight = EditorGUIUtility.singleLineHeight * 2 + 4;
						list.drawElementCallback = (rect, index, isActive, isFocused) =>
						{
							EditorGUI.PropertyField(rect, m_useProperty.GetArrayElementAtIndex(index));
						};

						list.onAddCallback += list =>
						{
							var searchWindowProvider = UnityEngine.ScriptableObject.CreateInstance<SubWindow.FindClassWindowProvider>();
							searchWindowProvider.Initialize(this, baseType);

							SearchWindow.Open(new SearchWindowContext(m_this.thisView.mousePosition), searchWindowProvider);
						};
					}

					public override void AddCallback(string name)
					{
						m_object.Update();
						m_useProperty.ArrayAddEmpty();
						m_useProperty.ArrayBack().FindPropertyRelative("m_className").stringValue = name;
						m_useProperty.ArrayBack().FindPropertyRelative("m_callInterval").floatValue = 0.5f;
						m_object.ApplyModifiedProperties();
					}
				}
			}
		}
	}
}
