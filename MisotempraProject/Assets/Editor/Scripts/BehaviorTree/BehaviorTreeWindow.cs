using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Graphs;
using FileAccess;

/// <summary>MisoTempra editor</summary>
namespace LocalEditor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		public class BehaviorTreeWindow : EditorWindow
		{
			static readonly Vector2 m_cWindowSize = new Vector2(100.0f, 100.0f);
			static readonly Vector2 m_cSelectFileFieldPosition = new Vector2(10, 10);

			/// <summary>static instance</summary>
			public static List<BehaviorTreeWindow> instances { get; private set; } = new List<BehaviorTreeWindow>();
			/// <summary>EditorApplication.playModeStateChangedにSaveCallbackを追加したか</summary>
			static bool m_isAddSaveCallback = false;

			public BehaviorTreeNodeView nodeView { get; private set; } = null;
			public Vector2 mousePosition { get; private set; } = default;
			public string fileName { get { return m_fileName; } set { m_fileName = value; titleContent = new GUIContent("BTEditor: " + m_fileName); } }
			[SerializeField, HideInInspector]
			string m_fileName = "";

			public bool isDeleteFile { get; private set; } = false;
			public void SetTrueIsDeleteFile() { isDeleteFile = true; }

			public void RegisterNodeEditorGUI(string nodeName, string guid)
			{
				BTInspectorWindow.Open();
				BTInspectorWindow.instance.RegisterNodeEditorGUI(this, nodeName, guid);
				BTInspectorWindow.instance.RegisterBlackboardEditorGUI(this);
				BTInspectorWindow.instance.Repaint();
			}
			public void UnregisterNodeEditorGUI()
			{
				BTInspectorWindow.Open();
				BTInspectorWindow.instance.UnregisterNodeEditorGUI(this);
			}
			public void RegisterBlackboardEditorGUI()
			{
				BTInspectorWindow.Open();
				BTInspectorWindow.instance.RegisterBlackboardEditorGUI(this);
				BTInspectorWindow.instance.Repaint();
			}
			public void UnregisterBlackboardNodeEditorGUI()
			{
				BTInspectorWindow.Open();
				BTInspectorWindow.instance.UnregisterBlackboardEditorGUI(this);
			}

			/// <summary>Open</summary>
			[MenuItem("Window/Behavior tree Editor")]
			public static void Open()
			{
				//インスタンス作成
				var window = CreateInstance<BehaviorTreeWindow>();
				window.titleContent = new GUIContent("BTEditor");

				//表示
				window.Show();
			}

			public static BehaviorTreeWindow NewOpen()
			{
				//インスタンス作成
				var window = CreateInstance<BehaviorTreeWindow>();
				window.titleContent = new GUIContent("BTEditor");

				//表示
				window.Show();
				return window;
			}

			void OnEnable()
			{
				instances.Add(this);

				//コールバック追加
				if (!m_isAddSaveCallback)
				{
					EditorApplication.playModeStateChanged += SaveCallaback;
					m_isAddSaveCallback = true;
				}

				nodeView = new BehaviorTreeNodeView(this);
				rootVisualElement.Add(nodeView);

				wantsMouseMove = true;
			}

			void OnDisable()
			{
				//インスタンスがあれば削除
				if (instances.Contains(this))
					instances.Remove(this);

				if (isDeleteFile) return;

				bool isSaveResult = false;

				try { if (nodeView != null) isSaveResult = nodeView.ForceSave(); }
				catch (System.Exception) { return; }

				if (nodeView != null && isSaveResult)
					nodeView.DrawSaveCompletedLog();
				else if (nodeView != null)
					Debug.LogWarning("Behavior tree (" + fileName + ") invalid contents. Force save completed.");
			}

			void OnGUI()
			{
				if (Event.current.type == EventType.MouseMove)
					mousePosition = Event.current.mousePosition + position.position;
			}

			/// <summary>EditorApplication用コールバック</summary>
			static void SaveCallaback(PlayModeStateChange change)
			{
				foreach (var e in instances)
				{
					if (e.nodeView != null)
					{
						//プレイモードになった場合セーブを行う
						if (change == PlayModeStateChange.ExitingEditMode)
							e.nodeView.Save();
						//なんにしろEnterでリロード
						if (change == PlayModeStateChange.EnteredPlayMode
							|| change == PlayModeStateChange.EnteredEditMode)
							e.nodeView.Reload();
					}
				}
			}
		}
	}
}