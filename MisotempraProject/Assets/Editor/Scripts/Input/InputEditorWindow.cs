using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>Input editor</summary>
	namespace Input
	{
		public class InputEditorWindow : EditorWindow
		{
			public static InputEditorWindow instance { get; private set; } = null;

			static string m_cFileName = "InputManagement";
			static bool m_isAddSaveCallback = false;

			InputCashContainer m_cashContainer = new InputCashContainer();
			Vector2 m_scrollPosition = Vector2.zero;

			/// <summary>Open</summary>
			[MenuItem("Window/Input Management")]
			static void Open()
			{
				//インスタンス作成
				if (instance == null)
				{
					var window = CreateInstance<InputEditorWindow>();
					window.titleContent = new GUIContent("TimeLayer Editor");

					//ロード->表示
					window?.Load();
					window?.Show();
				}
				else
				{
					instance.Show();
				}
			}

			void OnEnable()
			{
				instance = this;

				//コールバック追加
				if (!m_isAddSaveCallback)
				{
					EditorApplication.playModeStateChanged += SaveCallaback;
					m_isAddSaveCallback = true;
				}
			}

			void OnGUI()
			{
				using (var scrollViewScope = new GUILayout.ScrollViewScope(m_scrollPosition))
				{
					m_scrollPosition = scrollViewScope.scrollPosition;

				//	for (int i = 0; i < m_enumNames.Length - 1; ++i)
				//	{
				//		if (i % 3 == 0) EditorGUILayout.BeginHorizontal();
				//		EditorGUILayout.Toggle(m_enumNames[i + 1], false);
				//		if (i % 3 == 2) EditorGUILayout.EndHorizontal();
				//	}
				//	if ((m_enumNames.Length - 1) % 3 != 2)
				//		EditorGUILayout.EndHorizontal();
				}
			}

			void Load()
			{
				//var s = $"{}"
				//if (!File.Exists(Application.streamingAssetsPath + "/Input/" + m_cFileName ))

				//FileAccess.FileAccessor.LoadObject(Application.streamingAssetsPath + "/Input", m_cFileName)
				//LoadInputManager("ProjectSettings/InputManager.asset");
			}
			public void LoadInputManager(string path)
			{
				// InputManagerの設定情報読み込み
				var serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath(path)[0]);
				m_cashContainer.ReloadAxes(serializedObject.FindProperty("m_Axes"));
			}

			/// <summary>EditorApplication用コールバック</summary>
			static void SaveCallaback(PlayModeStateChange change)
			{
				//プレイモードになった場合セーブを行う
				if (change == PlayModeStateChange.ExitingEditMode)
					instance?.Save();
			}

			void Save()
			{

			}
		}
	}
}