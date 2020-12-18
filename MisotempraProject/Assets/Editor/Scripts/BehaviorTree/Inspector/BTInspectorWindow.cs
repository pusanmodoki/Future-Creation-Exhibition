using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>MisoTempra editor</summary>
namespace LocalEditor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		public class BTInspectorWindow : EditorWindow
		{
			public static BTInspectorWindow instance { get; private set; } = null;
			public BehaviorTreeWindow editorWindow { get; private set; } = null;
			public UnityEditor.Editor nodeEditor { get; private set; } = null;
			public UnityEditor.Editor blackboardEditor { get; private set; } = null;
			public Vector2 mousePosition { get; private set; } = default;

			Vector2 m_scrollPosition = Vector2.zero;
			string m_nodeName;
			string m_guid;
			bool m_isFoldoutBehavior = true; 
			bool m_isFoldoutBlackboard = true;

			[MenuItem("Window/Behavior tree")]
			public static void Open()
			{
				if (instance == null)
				{
					//インスタンス作成
					instance = GetWindow<BTInspectorWindow>();
					instance.titleContent = new GUIContent("BTInspector");
				}

				//表示
				instance.Show();
			}

			public void RegisterNodeEditorGUI(BehaviorTreeWindow editorWindow, string nodeName, string guid)
			{
				if (this.editorWindow != editorWindow)
					blackboardEditor = null;

				this.editorWindow = editorWindow;
				nodeEditor = editorWindow.nodeView.nodeScriptableEditor;
				m_nodeName = nodeName;
				m_guid = guid;
			}
			public void UnregisterNodeEditorGUI(BehaviorTreeWindow editorWindow)
			{
				if (this.editorWindow == editorWindow && nodeEditor == editorWindow.nodeView.nodeScriptableEditor)
				{
					nodeEditor = null;
					if (blackboardEditor == null) this.editorWindow = null;
					Repaint();
				}
			}

			public void RegisterBlackboardEditorGUI(BehaviorTreeWindow editorWindow)
			{
				if (this.editorWindow != editorWindow)
					nodeEditor = null;

				this.editorWindow = editorWindow;
				blackboardEditor = editorWindow.nodeView.blackboardScriptableEditor;
			}
			public void UnregisterBlackboardEditorGUI(BehaviorTreeWindow editorWindow)
			{
				if (this.editorWindow == editorWindow)
				{
					blackboardEditor = null;
					if (nodeEditor == null) this.editorWindow = null;
					Repaint();
				}
			}

			void OnEnable()
			{
				wantsMouseMove = true;
				EditorApplication.playModeStateChanged += change =>
				{
					editorWindow = null;
					blackboardEditor = null;
					nodeEditor = null;
				};
			}
			void OnDisable()
			{
				if (nodeEditor != null && nodeEditor.target != null)
					nodeEditor.serializedObject?.ApplyModifiedProperties();
				if (blackboardEditor != null && blackboardEditor.target != null)
					blackboardEditor.serializedObject?.ApplyModifiedProperties();

				editorWindow = null;
				blackboardEditor = null;
				nodeEditor = null;
			}

			void OnGUI()
			{
				if (Event.current.type == EventType.MouseMove)
					mousePosition = Event.current.mousePosition + position.position;

				m_scrollPosition = EditorGUILayout.BeginScrollView(m_scrollPosition);
				DrawNodeGUI();
				DrawBlackbordGUI();
				EditorGUILayout.EndScrollView();
			}

			void DrawNodeGUI()
			{
				if (editorWindow == null || nodeEditor == null ||
					nodeEditor.target == null || nodeEditor.serializedObject == null)
				{
					nodeEditor = null;

					EditorGUILayout.HelpBox("Not behavior node selected.", MessageType.Info);
					return;
				}

				m_isFoldoutBehavior = EditorGUILayout.Foldout(m_isFoldoutBehavior, "Behavior Node", true);
				if (m_isFoldoutBehavior)
				{
					using (new EditorGUI.IndentLevelScope())
					{
						GUIStyle style = new GUIStyle() { wordWrap = true };
						EditorGUILayout.LabelField(new GUIContent("File name:: " + editorWindow.fileName 
							+ "\nNode name:: " + m_nodeName + "\nNode guid:: " + m_guid), style);

						GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3));
						EditorGUILayout.Space();

						EditorGUI.BeginChangeCheck();

						nodeEditor.OnInspectorGUI();

						if (EditorGUI.EndChangeCheck())
							editorWindow.nodeView.CheckSaveReady();
					}
				}
			}
			void DrawBlackbordGUI()
			{
				if (editorWindow == null || blackboardEditor == null 
					|| blackboardEditor.target == null || blackboardEditor.serializedObject == null)
				{
					blackboardEditor = null;

					EditorGUILayout.HelpBox("Not blackbord selected.", MessageType.Info);
					return;
				}

				m_isFoldoutBlackboard = EditorGUILayout.Foldout(m_isFoldoutBlackboard, "Blackboard", true);
				if (m_isFoldoutBlackboard)
				{
					using (new EditorGUI.IndentLevelScope())
					{
						EditorGUI.BeginChangeCheck();

						blackboardEditor.OnInspectorGUI();

						if (EditorGUI.EndChangeCheck())
							editorWindow.nodeView.CheckSaveReady();
					}
				}
			}
		}
	}
}