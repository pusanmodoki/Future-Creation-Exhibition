using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using CashContainer = AI.BehaviorTree.CashContainer;

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
				public class Propertys
				{
					public SerializedProperty guid { get; private set; }
					public SerializedProperty memo { get; private set; }
					public SerializedProperty nodeName { get; private set; }
					public SerializedProperty childrenNodesGuid { get; private set; }
					public SerializedProperty decoratorClasses { get; private set; }
					public SerializedProperty serviceClasses { get; private set; }
					public SerializedProperty taskToJson { get; private set; }
					public SerializedProperty taskClassName { get; private set; }
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
						taskToJson = m_this.thisContainer.FindPropertyRelative("m_taskToJson");
						taskClassName = m_this.thisContainer.FindPropertyRelative("m_taskClassName");
						finishMode = m_this.thisContainer.FindPropertyRelative("m_finishMode");
						probabilitys = m_this.thisContainer.FindPropertyRelative("m_probabilitys");
					}

					BTBaseNodeEditor m_this;
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
						m_this.m_decoratorsList.list.DoLayoutList();
						if (m_this.m_decoratorProperty != null)
						{
							EditorGUILayout.LabelField("Select decorator inspector");

							//m_this.m_decoratorEditor.serializedObject.Update();
							m_this.m_decoratorEditor.OnInspectorGUI();
							//m_this.m_decoratorEditor.serializedObject.ApplyModifiedProperties();

							m_this.m_decoratorProperty.stringValue = JsonUtility.ToJson(m_this.m_decorator);
							m_this.m_decoratorProperty.serializedObject.ApplyModifiedProperties();

							EditorGUILayout.Space();
						}
					}
					public void DrawServices()
					{
						m_this.m_servicesList.list.DoLayoutList();
						if (m_this.m_serviceProperty != null)
						{
							EditorGUILayout.LabelField("Select service inspector");

							//m_this.m_serviceEditor.serializedObject.Update();
							m_this.m_serviceEditor.OnInspectorGUI();
							//m_this.m_serviceEditor.serializedObject.ApplyModifiedProperties();

							m_this.m_serviceProperty.stringValue = JsonUtility.ToJson(m_this.m_service);
							m_this.m_serviceProperty.serializedObject.ApplyModifiedProperties();

							EditorGUILayout.Space();
						}
					}
					public void DrawTask()
					{
						string name = m_this.propertys.taskClassName.stringValue;
						bool isEmpty = name == null || name.Length == 0;

						if (isEmpty) name = "Empty!! press set button→";

						EditorGUILayout.Space();
						GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
						using (new EditorGUILayout.HorizontalScope())
						{
							EditorGUILayout.LabelField("Task class:  " + name);
							if (GUILayout.Button("set"))
							{
								var searchWindowProvider = CreateInstance<SubWindow.TaskClassWindowProvider>();
								searchWindowProvider.Initialize(m_this);
								var position = BTInspectorWindow.instance != null ? BTInspectorWindow.instance.mousePosition : m_this.thisView.mousePosition;
								SearchWindow.Open(new SearchWindowContext(position), searchWindowProvider);
							}
						}
						if (m_this.m_task == null)
							return;

						m_this.m_taskEditor.serializedObject.Update();
						m_this.m_taskEditor.OnInspectorGUI();
						m_this.m_taskEditor.serializedObject.ApplyModifiedProperties();

						GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
						m_this.propertys.taskToJson.stringValue = JsonUtility.ToJson(m_this.m_task);
					}
					public void DrawFinishMode()
					{
						EditorGUILayout.PropertyField(m_this.propertys.finishMode);
					}
					public void DrawProbabilitys()
					{
						m_this.m_probabilitysList.list.DoLayoutList();
					}
					
					BTBaseNodeEditor m_this;
				}

				public SerializedProperty thisContainer { get; set; } = null;
				public SerializedProperty cashContainers { get; set; } = null;
				public Propertys propertys { get; set; } = default;
				public DrawFunctions functions { get; set; } = default;
				public BehaviorTreeNodeView thisView { get; set; } = default;

				ReorderableLists.ChildrensList m_childrensList = null;
				ReorderableLists.ServiceList m_servicesList = null;
				ReorderableLists.DecoratorList	 m_decoratorsList = null;
				ReorderableLists.ProbabilityList m_probabilitysList = null;

				AI.BehaviorTree.BaseTask m_task = null;
				UnityEngine.ScriptableObject m_taskScriptableObject = null;
				UnityEditor.Editor m_taskEditor = null;

				AI.BehaviorTree.BaseService m_service = null;
				UnityEngine.ScriptableObject m_serviceScriptableObject = null;
				UnityEditor.Editor m_serviceEditor = null;
				SerializedProperty m_serviceProperty = null;

				AI.BehaviorTree.BaseDecorator m_decorator = null;
				UnityEngine.ScriptableObject m_decoratorScriptableObject = null;
				UnityEditor.Editor m_decoratorEditor = null;
				SerializedProperty m_decoratorProperty = null;


				public void Initialize(BehaviorTreeNodeView view)
				{
					thisView = view;
					if (propertys.childrenNodesGuid != null)
						m_childrensList = new ReorderableLists.ChildrensList(this);
					if (propertys.serviceClasses != null)
						m_servicesList = new ReorderableLists.ServiceList(this, propertys.serviceClasses, typeof(AI.BehaviorTree.BaseService), "Services");
					if (propertys.decoratorClasses != null)
						m_decoratorsList = new ReorderableLists.DecoratorList(this, propertys.decoratorClasses, typeof(AI.BehaviorTree.BaseDecorator), "Decorators");
					if (propertys.probabilitys != null)
						m_probabilitysList = new ReorderableLists.ProbabilityList(this);

					if (propertys.taskClassName != null && propertys.taskClassName.stringValue.Length > 0)
					{
						System.Type type = TypeExtension.FindTypeInAllAssembly(propertys.taskClassName.stringValue);

						if (propertys.taskToJson == null || propertys.taskToJson.stringValue.Length == 0)
						{
							m_task = (AI.BehaviorTree.BaseTask)System.Activator.CreateInstance(type);
							TaskScriptableObjects.TaskScriptableObjectClassMediator.CreateEditorAndScriptableObject(
								m_task, out m_taskEditor, out m_taskScriptableObject, propertys.taskClassName.stringValue);
						}
						else
						{
							if (type != null)
							{
								m_task = (AI.BehaviorTree.BaseTask)JsonUtility.FromJson(propertys.taskToJson.stringValue, type);
								TaskScriptableObjects.TaskScriptableObjectClassMediator.CreateEditorAndScriptableObject(
									m_task, out m_taskEditor, out m_taskScriptableObject, propertys.taskClassName.stringValue);
							}
							else
							{
								serializedObject.Update();
								propertys.taskClassName.stringValue = "";
								serializedObject.ApplyModifiedProperties();
							}
						}
					}
				}

				public void SelectService(string className, SerializedProperty property)
				{
					m_serviceProperty = property;

					if (property.stringValue.Length == 0)
						m_service = (AI.BehaviorTree.BaseService)System.Activator.CreateInstance(TypeExtension.FindTypeInAllAssembly(className));
					else
						m_service = (AI.BehaviorTree.BaseService)JsonUtility.FromJson(property.stringValue, TypeExtension.FindTypeInAllAssembly(className));

					ServiceScriptableObjects.ServiceScriptableObjectClassMediator.CreateEditorAndScriptableObject(
						m_service, out m_serviceEditor, out m_serviceScriptableObject, className);
				}
				public void UnselectService()
				{
					m_service = null;
					m_serviceProperty = null;
					m_serviceScriptableObject = null;
					m_serviceEditor = null;
				}

				public void SelectDecorator(string className, SerializedProperty property)
				{
					m_decoratorProperty = property;

					if (property.stringValue.Length == 0)
						m_decorator = (AI.BehaviorTree.BaseDecorator)System.Activator.CreateInstance(TypeExtension.FindTypeInAllAssembly(className));
					else
						m_decorator = (AI.BehaviorTree.BaseDecorator)JsonUtility.FromJson(property.stringValue, TypeExtension.FindTypeInAllAssembly(className));

					DecoratorScriptableObjects.DecoratorScriptableObjectClassMediator.CreateEditorAndScriptableObject(
						m_decorator, out m_decoratorEditor, out m_decoratorScriptableObject, className);
				}
				public void UnselectDecorator()
				{
					m_decorator = null;
					m_decoratorProperty = null;
					m_decoratorScriptableObject = null;
					m_decoratorEditor = null;
				}

				public void CreateTaskCallback(SearchTreeEntry searchTreeEntry)
				{
					serializedObject.Update();
					propertys.taskClassName.stringValue = (searchTreeEntry.userData as System.Type).FullName;
					serializedObject.ApplyModifiedProperties();

					m_task = (AI.BehaviorTree.BaseTask)System.Activator.CreateInstance(
						 TypeExtension.FindTypeInAllAssembly(propertys.taskClassName.stringValue));
					TaskScriptableObjects.TaskScriptableObjectClassMediator.CreateEditorAndScriptableObject(
						m_task, out m_taskEditor, out m_taskScriptableObject, propertys.taskClassName.stringValue);
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
					EditorGUILayout.Space(15.0f);

					serializedObject.ApplyModifiedProperties();

					EditorUtility.SetDirty(target);
				}
			}
		}
	}
}