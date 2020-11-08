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
		public class BTInspectorWindow : EditorWindow
		{
			public static BTInspectorWindow instance { get; private set; } = null;
			public Vector2 mousePosition { get; private set; } = default;

			BehaviorTreeWindow m_editorWindow = null;
			UnityEditor.Editor m_nodeEditor = null;
			UnityEditor.Editor m_blackboardEditor = null;
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
				if (m_editorWindow != editorWindow)
					m_blackboardEditor = null;

				m_editorWindow = editorWindow;
				m_nodeEditor = editorWindow.nodeView.nodeScriptableEditor;
				m_nodeName = nodeName;
				m_guid = guid;
			}
			public void UnregisterNodeEditorGUI(BehaviorTreeWindow editorWindow)
			{
				if (m_editorWindow == editorWindow && m_nodeEditor == editorWindow.nodeView.nodeScriptableEditor)
				{
					m_nodeEditor = null;
					if (m_blackboardEditor == null) m_editorWindow = null;
					Repaint();
				}
			}

			public void RegisterBlackboardEditorGUI(BehaviorTreeWindow editorWindow)
			{
				if (m_editorWindow != editorWindow)
					m_nodeEditor = null;

				m_editorWindow = editorWindow;
				m_blackboardEditor = editorWindow.nodeView.blackboardScriptableEditor;
			}
			public void UnregisterBlackboardEditorGUI(BehaviorTreeWindow editorWindow)
			{
				if (m_editorWindow == editorWindow)
				{
					m_blackboardEditor = null;
					if (m_nodeEditor == null) m_editorWindow = null;
					Repaint();
				}
			}

			void OnEnable()
			{
				wantsMouseMove = true;
			}
			void OnDisable()
			{
				m_editorWindow = null;
				m_blackboardEditor = null;
				m_nodeEditor = null;
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
				if (m_editorWindow == null || m_nodeEditor == null)
				{
					m_nodeEditor = null;

					EditorGUILayout.HelpBox("Not behavior node selected.", MessageType.Info);
					return;
				}

				m_isFoldoutBehavior = EditorGUILayout.Foldout(m_isFoldoutBehavior, "Behavior Node", true);
				if (m_isFoldoutBehavior)
				{
					using (new EditorGUI.IndentLevelScope())
					{
						GUIStyle style = new GUIStyle() { wordWrap = true };
						EditorGUILayout.LabelField(new GUIContent("File name:: " + m_editorWindow.fileName 
							+ "\nNode name:: " + m_nodeName + "\nNode guid:: " + m_guid), style);

						GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3));
						EditorGUILayout.Space();

						m_nodeEditor.OnInspectorGUI();
					}
				}
			}
			void DrawBlackbordGUI()
			{
				if (m_editorWindow == null || m_blackboardEditor == null)
				{
					m_blackboardEditor = null;

					EditorGUILayout.HelpBox("Not blackbord selected.", MessageType.Info);
					return;
				}

				m_isFoldoutBlackboard = EditorGUILayout.Foldout(m_isFoldoutBlackboard, "Blackboard", true);
				if (m_isFoldoutBlackboard)
				{
					using (new EditorGUI.IndentLevelScope())
					{
						m_blackboardEditor.OnInspectorGUI();
					}
				}
			}
		}
	}
}