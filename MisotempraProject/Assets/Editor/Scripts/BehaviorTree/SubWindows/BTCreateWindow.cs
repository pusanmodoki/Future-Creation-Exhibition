using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		/// <summary>Sub windows</summary>
		namespace SubWindow
		{
			public class BTCreateWindow : PopupWindowContent
			{
				public static readonly Vector2 cWindowSize = new Vector2(700, 160);

				BehaviorTreeNodeView m_view = null;
				Vector2 m_position = Vector2.zero;
				string m_name = "";
				string m_path = "";
				bool m_isInitFocus = false;
				bool m_isPushOK = false;

				public void Initialize(BehaviorTreeNodeView view, Vector2 position, string usePath)
				{
					m_view = view;
					m_position = position;
					m_path = usePath;
				}

				public override Vector2 GetWindowSize()
				{
					return cWindowSize;
				}

				public override void OnClose()
				{
					if (!m_isPushOK) m_name = null;
					m_view.DoCreateCallback(m_name);
				}

				public override void OnGUI(Rect rect)
				{
					GUILayout.Label("Create Behavior file");
					GUILayout.Space(10.0f);
					GUILayout.Label("File name");
					GUI.SetNextControlName("BehaviorTreeCreateWindowFocusField");
					m_name = GUILayout.TextField(m_name, 30);

					string path = m_path + m_name + ".dat";
					GUILayout.Space(10.0f);
					GUILayout.Label("Path: " + path);
					GUILayout.Space(10.0f);

					if (m_name == null || m_name.Length == 0)
						EditorGUILayout.HelpBox("File name empty!!", MessageType.Error);
					else if (m_name.Contains("\\") | m_name.Contains("/") | m_name.Contains(":")
						| m_name.Contains("*") | m_name.Contains("?") | m_name.Contains("\"")
						| m_name.Contains("<") | m_name.Contains(">") | m_name.Contains("|"))
						EditorGUILayout.HelpBox("Invalid file name!!\nInvalid chars: \\, /, :, *, ?, \", <, >, |", MessageType.Error);
					else if (System.IO.File.Exists(path))
						EditorGUILayout.HelpBox("File already exists!!", MessageType.Error);
					else
					{
						GUILayout.BeginHorizontal();

						if (GUILayout.Button("OK"))
						{
							m_isPushOK = true;
							editorWindow.Close();
						}

						if (GUILayout.Button("Cancel"))
							editorWindow.Close();

						GUILayout.EndHorizontal();
					}

					if (!m_isInitFocus)
					{
						if (m_position.x < 0.0f) m_position.x = 0.0f;
						if (m_position.y < 0.0f) m_position.y = 0.0f;
						if (m_position.x + editorWindow.position.width > Screen.currentResolution.width)
							m_position.x = Screen.currentResolution.width - editorWindow.position.width * 1.3f;
						if (m_position.y + editorWindow.position.height > Screen.currentResolution.height)
							m_position.y = Screen.currentResolution.height - editorWindow.position.height * 1.3f;
						
						editorWindow.position = new Rect(m_position, editorWindow.position.size);
						EditorGUI.FocusTextInControl("BehaviorTreeCreateWindowFocusField");
						m_isInitFocus = true;
					}
				}
			}
		}
	}
}