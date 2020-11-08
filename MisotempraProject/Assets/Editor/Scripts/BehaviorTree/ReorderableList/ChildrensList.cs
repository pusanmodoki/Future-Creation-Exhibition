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
			/// <summary>ReorderableList classes</summary>
			namespace ReorderableLists
			{
				public class ChildrensList
				{
					public struct Content
					{
						public Content(string name, string guid)
						{
							m_name = name;
							m_guid = guid;
						}
						public string nodeName { get { return m_name; } }
						public string guid { get { return m_guid; } }
						[SerializeField]
						string m_name;
						[SerializeField]
						string m_guid;
					}

					public UnityEditorInternal.ReorderableList list { get; private set; } = null;

					public ChildrensList(BTBaseNodeEditor editor)
					{
						m_this = editor;
						m_this.propertys.childrenNodesGuid.ForElements(property =>
						{
							m_guids.Add(property.stringValue);
							m_contents.Add(new Content(m_this.thisView.cashContainersKeyGuid[property.stringValue].nodeName, property.stringValue));
						});

						list = new UnityEditorInternal.ReorderableList(
						  elements: m_contents,             //要素
						  elementType: typeof(Content), //要素の種類
						  draggable: true,                      //ドラッグして要素を入れ替えられるか
						  displayHeader: true,                  //ヘッダーを表示するか
						  displayAddButton: false,          //要素追加用の+ボタンを表示するか
						  displayRemoveButton: false        //要素削除用の-ボタンを表示するか
						);

						list.elementHeight = EditorGUIUtility.singleLineHeight * 2 + 4;
						list.onReorderCallback += ReorderCallback;
						list.drawHeaderCallback += (rect) => EditorGUI.LabelField(rect, "Children node prioritys");
						list.drawElementCallback = (rect, index, isActive, isFocused) => {
							rect.height = EditorGUIUtility.singleLineHeight;
							EditorGUI.LabelField(rect, "name: " + m_contents[index].nodeName);
							rect.position = rect.position + new Vector2(0.0f, EditorGUIUtility.singleLineHeight + 2.0f);
							EditorGUI.LabelField(rect, "guid: " + m_contents[index].guid);
						};
					}

					BTBaseNodeEditor m_this;
					List<string> m_guids = new List<string>();
					List<Content> m_contents = new List<Content>();

					public void Reload()
					{
						if (m_guids.Count == m_this.propertys.childrenNodesGuid.arraySize)
						{
							bool isNotChange = true;
							for (int i = 0; i < m_this.propertys.childrenNodesGuid.arraySize && isNotChange; ++i)
								isNotChange &= m_guids.Contains(m_this.propertys.childrenNodesGuid.GetArrayElementAtIndex(i).stringValue);

							if (isNotChange) return;
						}

						m_guids.Clear();
						m_contents.Clear();

						m_this.propertys.childrenNodesGuid.ForElements(property =>
						{
							m_guids.Add(property.stringValue);
							m_contents.Add(new Content(m_this.thisView.cashContainersKeyGuid[property.stringValue].nodeName, property.stringValue));
						});
					}

					void ReorderCallback(UnityEditorInternal.ReorderableList list)
					{
						for (int i = 0; i < m_contents.Count; ++i)
						{
							for (int k = 0; k < m_this.propertys.childrenNodesGuid.arraySize; ++k)
							{
								if (m_this.propertys.childrenNodesGuid.GetArrayElementAtIndex(k).stringValue == m_contents[i].guid)
								{
									if (i != k)
										m_this.propertys.childrenNodesGuid.MoveArrayElement(k, i);
									break;
								}
							}
						}

						m_this.thisView.ChangeChildrenOrder(m_this.propertys.guid.stringValue);
					}
				}
			}
		}
	}
}