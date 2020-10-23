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
		public class BehaviorTreeCreateWindowAAA : PopupWindowContent
		{
			static int m_focusCounter = 0;

			BehaviorTreeWindow m_window = null;
			string m_name = "";
			int m_focusNumber = 0;
			bool m_isSetResult = false;
			bool m_isInitFocus = false;

			public void SetParentWindow(BehaviorTreeWindow window)
			{
				m_window = window;
			}

			public override void OnOpen()
			{
			}

			public override void OnClose()
			{
				if (m_window != null && !m_isSetResult)
					m_window.SetCreateResult(BehaviorTreeWindow.CreateResult.Close);
			}	

			public override void OnGUI(Rect rect)
			{
				if (!m_isInitFocus) m_focusNumber = m_focusCounter++;

				GUILayout.Label("File name");
				GUI.SetNextControlName("BehaviorTreeCreateWindowFocusField" + m_focusNumber);
				m_name = GUILayout.TextField(m_name);

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
					EditorGUI.FocusTextInControl("BehaviorTreeCreateWindowFocusField" + m_focusNumber);
					m_isInitFocus = true;
				}
			}
		}
	}
}