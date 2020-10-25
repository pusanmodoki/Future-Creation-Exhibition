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
		/// <summary>File access windows</summary>
		namespace FileAccessWindow
		{
			public class BehaviorTreeCreateWindow : PopupWindowContent
			{
				public static readonly Vector2 cWindowSize = new Vector2(700, 160);

				BehaviorTreeNodeView m_view = null;
				string m_name = "";
				bool m_isInitFocus = false;
				bool m_isPushOK = false;

				public void Initialize(BehaviorTreeNodeView view)
				{
					m_view = view;
					
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

					string path = Application.streamingAssetsPath + "/AI/" + m_name + ".dat";
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
						var position = editorWindow.position;
						position.position = new Vector2(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2) - GetWindowSize() * 0.5f;
						editorWindow.position = position;

						EditorGUI.FocusTextInControl("BehaviorTreeCreateWindowFocusField");
						m_isInitFocus = true;
					}
				}
			}
		}
	}
}