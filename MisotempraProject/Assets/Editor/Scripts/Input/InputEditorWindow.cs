using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using InputManagement;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>Input editor</summary>
	namespace Input
	{
		public class InputEditorWindow : EditorWindow
		{
			public static InputEditorWindow instance { get; private set; } = null;

			static bool m_isAddSaveCallback = false;

			InputCashContainer m_cashContainer = null;
			InputScriptableObject m_inputScriptableObject = null;
			Vector2 m_scrollPosition = Vector2.zero;
			bool m_isFoldoutKeyCode = false;
			bool m_isFoldoutKeyCodeNormalKeys = false;
			bool[] m_isFoldoutKeyCodeJoysticks = null;
			bool m_isFoldoutAxisCode = false;

			/// <summary>Open</summary>
			[MenuItem("Window/Input Management")]
			static void Open()
			{
				//インスタンス作成
				if (instance == null)
				{
					var window = CreateInstance<InputEditorWindow>();
					window.titleContent = new GUIContent("GameInput Editor");

					//表示
					window?.Show();
				}
				else
					instance.Show();
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

				Load();
			}
			void OnDisable()
			{
				if (instance == this)
					Save(true);
			}

			void OnGUI()
			{
				bool isChangedKey = false, isChangedAxis = false;

				m_scrollPosition = EditorGUILayout.BeginScrollView(m_scrollPosition);

				EditorGUILayout.Space();
				{
					GUIStyle style = new GUIStyle(EditorStyles.label);
					style.fontSize = 20;
					style.fontStyle = FontStyle.Bold;
					EditorGUILayout.LabelField("Game Input Management", style);
				}

				EditorGUILayout.Space(20);
				m_isFoldoutKeyCode = EditorGUILayout.Foldout(m_isFoldoutKeyCode, "Enable key codes", true);
				if (m_isFoldoutKeyCode)
				{
					using (new EditorGUI.IndentLevelScope())
					{
						OnGUIKeyCodes(out isChangedKey);
					}
				}

				EditorGUILayout.Space(10);
				m_isFoldoutAxisCode = EditorGUILayout.Foldout(m_isFoldoutAxisCode, "Enable input axes", true);
				if (m_isFoldoutAxisCode)
				{
					using (new EditorGUI.IndentLevelScope())
					{
						OnGUIAxes(out isChangedAxis);
					}
				}

				if (isChangedAxis | isChangedKey)
				{
					Save(false);
				}

				EditorGUILayout.Space(10);
				using (new EditorGUILayout.HorizontalScope())
				{
					GUILayout.FlexibleSpace();

					GUIStyle style = new GUIStyle(EditorStyles.miniButton);
					style.fontSize = 14;

					if (GUILayout.Button("          Reload          ", style))
					{
						m_cashContainer = null;

						try
						{
							Load();
							Debug.Log("GameInput Editor reload completed.");
						}
						catch (System.Exception e)
						{
							Debug.LogError("GameInput Editor reload failed.\n message: " + e.Message);
						}
					}

					if (GUILayout.Button("          Save(Auto-save is enabled.)          ", style))
						Save(true);
				}

				EditorGUILayout.EndScrollView();
			}

			void Load()
			{
				if (!File.Exists($"{Application.streamingAssetsPath }/Input/{GameInput.cFileName}.dat"))
				{
					m_cashContainer = new InputCashContainer();
					m_cashContainer.EditFirstInitializeEnums();
				}
				else
				{
					FileAccess.FileAccessor.LoadObject(Application.streamingAssetsPath + "/Input", GameInput.cFileName,
						out m_cashContainer, GameInput.cFileBeginMark);
					m_cashContainer.EditBuildAxes();
				}

				LoadInputManager("ProjectSettings/InputManager.asset");

				{
					var temp = m_isFoldoutKeyCodeJoysticks;
					m_isFoldoutKeyCodeJoysticks = new bool[m_cashContainer.joystickIndexes.Length];

					for (int i = 0; i < m_isFoldoutKeyCodeJoysticks.Length; ++i)
						m_isFoldoutKeyCodeJoysticks[i] = temp != null && temp.Length > i ? temp[i] : false;
				}

				m_inputScriptableObject = CreateInstance<InputScriptableObject>();
				m_inputScriptableObject.Initialize(m_cashContainer);
			}
			public void LoadInputManager(string path)
			{
				// InputManagerの設定情報読み込み
				var serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath(path)[0]);
				m_cashContainer.EditReloadAxes(serializedObject.FindProperty("m_Axes"));
			}

			/// <summary>EditorApplication用コールバック</summary>
			static void SaveCallaback(PlayModeStateChange change)
			{
				//プレイモードになった場合セーブを行う
				if (change == PlayModeStateChange.ExitingEditMode)
					instance?.Save(true);
			}

			void Save(bool isDrawLog)
			{
				if (m_cashContainer == null) return;

				m_cashContainer.SaveConvert();
				try
				{
					FileAccess.FileAccessor.SaveObject(Application.streamingAssetsPath + "/Input",
						GameInput.cFileName, ref m_cashContainer, GameInput.cFileBeginMark);
					if (isDrawLog) Debug.Log("GameInput Editor save completed.");
				}
				catch(System.Exception e)
				{
					Debug.LogError("GameInput Editor save failed.\n message: " + e.Message);
				}
			}

			void OnGUIKeyCodes(out bool isChanged)
			{
				isChanged = false;

				m_isFoldoutKeyCodeNormalKeys = 
					EditorGUILayout.Foldout(m_isFoldoutKeyCodeNormalKeys, "Normal key codes", true);

				if (m_isFoldoutKeyCodeNormalKeys)
				{
					using (new EditorGUI.IndentLevelScope())
					{
						using (new EditorGUILayout.HorizontalScope())
						{
							bool temp = false;
							for (int group = 0; group < 3; ++group)
							{
								using (new EditorGUILayout.VerticalScope())
								{
									for (int i = group + 1; i < m_cashContainer.joystickIndexes[0]; i += 3)
									{
										temp = EditorGUILayout.Toggle(m_cashContainer.enumNames[i], m_cashContainer.isEnableEnums[i]);
										if (m_cashContainer.isEnableEnums[i] != temp)
										{
											Undo.RecordObject(m_inputScriptableObject, "Input changed");
											m_cashContainer.isEnableEnums[i] = temp;
											isChanged = true;
										}
									}
								}
							}
						}
					}
				}

				for (int i = 0; i < m_cashContainer.joystickIndexes.Length; ++i)
				{
					m_isFoldoutKeyCodeJoysticks[i] = EditorGUILayout.Foldout(
						m_isFoldoutKeyCodeJoysticks[i], "Joystick codes[" + i + "]", true);
					if (!m_isFoldoutKeyCodeJoysticks[i]) continue;

					using (new EditorGUI.IndentLevelScope())
					{
						int forEnd = i != m_cashContainer.joystickIndexes.Length - 1 ?
							m_cashContainer.joystickIndexes[i + 1] : m_cashContainer.enumNames.Length - 1;

						using (new EditorGUILayout.HorizontalScope())
						{
							bool temp = false;
							for (int group = 0; group < 3; ++group)
							{
								using (new EditorGUILayout.VerticalScope())
								{
									for (int k = m_cashContainer.joystickIndexes[i] + group; k < forEnd; k += 3)
									{
										temp = EditorGUILayout.Toggle(m_cashContainer.enumNames[k], m_cashContainer.isEnableEnums[k]);

										if (m_cashContainer.isEnableEnums[k] != temp)
										{
											Undo.RecordObject(m_inputScriptableObject, "Input changed");
											m_cashContainer.isEnableEnums[k] = temp;
											isChanged = true;
										}
									}
								}
							}
						}
					}
				}
			}

			void OnGUIAxes(out bool isChanged)
			{
				isChanged = false;

				using (new EditorGUILayout.HorizontalScope())
				{
					bool temp = false, read = false;
					AxisMode temp2 = default, read2 = default;
					for (int group = 0; group < 2; ++group)
					{
						using (new EditorGUILayout.VerticalScope())
						{
							for (int i = group; i < m_cashContainer.isEnableAxes.Count; i += 2)
							{
								using (new EditorGUILayout.HorizontalScope())
								{
									read = m_cashContainer.isEnableAxes[m_cashContainer.axisNames[i]];
									temp = EditorGUILayout.ToggleLeft(m_cashContainer.axisNames[i], read);
									if (read != temp)
									{
										Undo.RecordObject(m_inputScriptableObject, "Input changed");
										m_cashContainer.isEnableAxes[m_cashContainer.axisNames[i]] = temp;
										isChanged = true;
									}

									if (temp)
									{
										read2 = m_cashContainer.axisModes[m_cashContainer.axisNames[i]];
										temp2 = (AxisMode)EditorGUILayout.EnumFlagsField(read2);
										if (read2 != temp2)
										{
											Undo.RecordObject(m_inputScriptableObject, "Input changed");
											m_cashContainer.axisModes[m_cashContainer.axisNames[i]] = temp2;
											isChanged = true;
										}
									}
									else
										EditorGUILayout.LabelField("Axis disabled.");
								}
							}
						}
					}
				}
			}

		}
	}
}