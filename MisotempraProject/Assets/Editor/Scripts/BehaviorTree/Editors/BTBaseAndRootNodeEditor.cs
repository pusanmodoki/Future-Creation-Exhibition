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
				public struct Propertys
				{
					public SerializedProperty guid { get; private set; }
					public SerializedProperty memo { get; private set; }
					public SerializedProperty nodeName { get; private set; }
					public SerializedProperty childrenNodesGuid { get; private set; }
					public SerializedProperty decoratorClasses { get; private set; }
					public SerializedProperty serviceClasses { get; private set; }
					public SerializedProperty task { get; private set; }
					public SerializedProperty finishMode { get; private set; }
					public SerializedProperty probabilitys { get; private set; }

					public Propertys(BTBaseNodeEditor thisEditor)
					{
						m_this = thisEditor;
						guid = m_this.thisContainer.FindPropertyRelative("m_guid");
						memo = m_this.thisContainer.FindPropertyRelative("m_memo");
						nodeName = m_this.thisContainer.FindPropertyRelative("m_nodeName");
						childrenNodesGuid = m_this.thisContainer.FindPropertyRelative("m_childrenNodesGuid");
						decoratorClasses = m_this.thisContainer.FindPropertyRelative("m_decoratorClasses");
						serviceClasses = m_this.thisContainer.FindPropertyRelative("m_serviceClasses");
						task = m_this.thisContainer.FindPropertyRelative("m_task");
						finishMode = m_this.thisContainer.FindPropertyRelative("m_finishMode");
						probabilitys = m_this.thisContainer.FindPropertyRelative("m_probabilitys");
					}

					BTBaseNodeEditor m_this;
				}
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
						  elements: m_contents,				//要素
						  elementType: typeof(Content),	//要素の種類
						  draggable: true,						//ドラッグして要素を入れ替えられるか
						  displayHeader: true,					//ヘッダーを表示するか
						  displayAddButton: false,			//要素追加用の+ボタンを表示するか
						  displayRemoveButton: false		//要素削除用の-ボタンを表示するか
						);

						list.onReorderCallback += ReorderCallback;
						list.drawHeaderCallback += (rect) => EditorGUI.LabelField(rect, "Children node prioritys");
						list.drawElementCallback =(rect, index, isActive, isFocused) => {
							  EditorGUI.LabelField(rect, "name: " + m_contents[index].nodeName, "guid: " + m_contents[index].guid);
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
				public struct DrawFunctions
				{
					public DrawFunctions(BTBaseNodeEditor thisEditor)
					{
						m_this = thisEditor;
					}
					public void DrawMemo()
					{
						m_this.propertys.memo.stringValue = EditorGUILayout.TextField("Memo (edit only)",
							m_this.propertys.memo.stringValue);
					}
					public void DrawChildrensList()
					{
						m_this.m_childrensList.list.DoLayoutList();
					}
					public void DrawDecorators()
					{
						//あと配線問題だけなおせばかいけつ
					}
					public void DrawServices()
					{

					}
					public void DrawTask()
					{

					}
					public void DrawFinishMode()
					{
						EditorGUILayout.PropertyField(m_this.propertys.finishMode);
					}
					public void DrawProbabilitys()
					{

					}
					
					BTBaseNodeEditor m_this;
				}

				public SerializedProperty thisContainer { get; set; } = null;
				public SerializedProperty cashContainers { get; set; } = null;
				public Propertys propertys { get; set; } = default;
				public DrawFunctions functions { get; set; } = default;
				public BehaviorTreeNodeView thisView { get; set; } = default;

				ChildrensList m_childrensList = null;

				public void Initialize(BehaviorTreeNodeView view)
				{
					thisView = view;
					if (propertys.childrenNodesGuid != null) m_childrensList = new ChildrensList(this);
				}

				protected void OnEnable()
				{
					thisContainer = serializedObject.FindProperty("m_thisContainer");
					cashContainers = serializedObject.FindProperty("m_cashContainers");
					propertys = new Propertys(this);
					functions = new DrawFunctions(this);
				}
			}

			[CustomEditor(typeof(ScriptableObject.BTRootScriptableObject))]
			public class BTRootNodeEditor : BTBaseNodeEditor
			{
				public override void OnInspectorGUI()
				{
					serializedObject.Update();

					functions.DrawMemo();
					functions.DrawChildrensList();

					serializedObject.ApplyModifiedProperties();
				}
			}
		}
	}
}