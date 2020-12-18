using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;

/// <summary>MisoTempra editor</summary>
namespace LocalEditor
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
					BTBaseNodeEditor m_editor;

					public ServiceList(BTBaseNodeEditor editor, SerializedProperty useProperty, System.Type baseType, string title)
						: base(editor, useProperty, baseType, title)
					{
						m_editor = editor;
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

						list.onSelectCallback += select =>
						{
							var element = m_useProperty.GetArrayElement(list.index);
							string className = element.FindPropertyRelative("m_className").stringValue;
							if (className.Length > 0)
							{
								m_editor.SelectService(className, element.FindPropertyRelative("m_jsonData"));
							}
						};

						list.onRemoveCallback += list =>
						{
							list.serializedProperty.DeleteArrayElementAtIndex(list.index);
							m_editor.UnselectService();
						};
					}

					public override void AddCallback(string name)
					{
						m_object.Update();
						m_useProperty.ArrayAddEmpty();
						var back = m_useProperty.ArrayBack();
						back.FindPropertyRelative("m_className").stringValue = name;
						back.FindPropertyRelative("m_callInterval").floatValue = 0.5f;
						back.FindPropertyRelative("m_guid").stringValue = System.Guid.NewGuid().ToString();
						back.FindPropertyRelative("m_jsonData").stringValue = JsonUtility.ToJson(
							System.Activator.CreateInstance(TypeExtension.FindTypeInAllAssembly(name)));
						m_object.ApplyModifiedProperties();
					}
				}
			}
		}
	}
}
