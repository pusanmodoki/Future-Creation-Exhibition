using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

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
				public class ClassList
				{
					public UnityEditorInternal.ReorderableList list { get; private set; } = null;

					public ClassList(BTBaseNodeEditor editor, SerializedProperty useProperty, System.Type baseType, string title)
					{
						m_this = editor;
						m_object = editor.serializedObject;
						m_useProperty = useProperty;
						m_title = title;
						
						list = new UnityEditorInternal.ReorderableList(
							m_this.serializedObject,
							useProperty,
							draggable: true,                      //ドラッグして要素を入れ替えられるか
							displayHeader: true,                 //ヘッダーを表示するか
							displayAddButton: true,			//要素追加用の+ボタンを表示するか
							displayRemoveButton: true       //要素削除用の-ボタンを表示するか
						);

						InitializeList(baseType);
					}

					protected virtual void InitializeList(System.Type baseType)
					{
						list.drawHeaderCallback += (rect) => EditorGUI.LabelField(rect, m_title);
						list.drawElementCallback = (rect, index, isActive, isFocused) =>
						{
							EditorGUI.LabelField(rect, m_useProperty.GetArrayElementAtIndex(index).stringValue);
						};

						list.onAddCallback += list =>
						{
							var searchWindowProvider = UnityEngine.ScriptableObject.CreateInstance<SubWindow.FindClassWindowProvider>();
							searchWindowProvider.Initialize(this, baseType);

							SearchWindow.Open(new SearchWindowContext(m_this.thisView.mousePosition), searchWindowProvider);
						};
					}

					public virtual void AddCallback(string name)
					{
						m_object.Update();
						m_useProperty.ArrayAddEmpty();
						m_useProperty.ArrayBack().stringValue = name;
						m_object.ApplyModifiedProperties();
					}

					protected BTBaseNodeEditor m_this;
					protected SerializedObject m_object;
					protected SerializedProperty m_useProperty;
					protected string m_title;
				}
			}
		}
	}
}
