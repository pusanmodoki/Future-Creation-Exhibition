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
		public class BehaviorTreeCreateWindow : PopupWindowContent
		{
			BehaviorTreeWindow m_window = null;
			string m_name = "";
			bool m_isSetResult = false;
			bool m_isInitFocus = false;

			public void SetParentWindow(BehaviorTreeWindow window)
			{
				m_window = window;
			}
			public override Vector2 GetWindowSize()
			{
				return new Vector2(700, 130);
			}
			public override void OnOpen()
			{
				var position = editorWindow.position;
				position.position = new Vector2(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2) - GetWindowSize() * 0.5f;
				editorWindow.position = position;
			}

			public override void OnClose()
			{
				if (m_window != null && !m_isSetResult)
					m_window.SetCreateResult(BehaviorTreeWindow.CreateResult.Close);
			}

			public override void OnGUI(Rect rect)
			{
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
						m_isSetResult = true;
						m_window.SetCreateName(m_name);
						m_window.SetCreateResult(BehaviorTreeWindow.CreateResult.Create);
						editorWindow.Close();
					}

					if (GUILayout.Button("Cancel"))
					{
						m_isSetResult = true;
						m_window.SetCreateResult(BehaviorTreeWindow.CreateResult.Close);
						editorWindow.Close();
					}
					GUILayout.EndHorizontal();
				}

				if (!m_isInitFocus)
				{
					EditorGUI.FocusTextInControl("BehaviorTreeCreateWindowFocusField");
					m_isInitFocus = true;
				}
			}
		}
	}
}