using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Graphs;
using FileAccess;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		public class BehaviorTreeWindow : EditorWindow
		{
			public enum CreateResult
			{
				Null,
				Close,
				Create,
			}

			static readonly Vector2 m_cWindowSize = new Vector2(100.0f, 100.0f);
			static readonly Vector2 m_cSelectFileFieldPosition = new Vector2(10, 10);
			static readonly int m_cMaxFileNameLength = 30;

			/// <summary>static instance</summary>
			public static List<BehaviorTreeWindow> instances { get; private set; } = new List<BehaviorTreeWindow>();
			static List<string> m_allFileNames = new List<string>();
			static Dictionary<string, string> m_allFileConvertNames = new Dictionary<string, string>();
			static Dictionary<string, bool> m_usingFiles = new Dictionary<string, bool>();
			static string[] m_selectFileContents = null;
			/// <summary>EditorApplication.playModeStateChangedにSaveCallbackを追加したか</summary>
			static bool m_isAddSaveCallback = false;

			public string fileName { get; private set; } = "";
			List<BehaviorSaveNode> m_nodes = new List<BehaviorSaveNode>();
			BehaviorTreeNodeView m_nodeView = null;
			string m_createName = "";
			CreateResult m_createResult = CreateResult.Null;
			int m_selectFileIndex = 0;
			int m_oldSelectFileIndex = 0;
			bool m_isDeleteFile = false;
			bool m_isCreateInput = false;
			Vector2 m_selectFileFieldSize = Vector2.zero;

			public void SetCreateName(string name) { m_createName = name; }
			public void SetCreateResult(CreateResult result) { m_createResult = result; }
			public void SetDeleteFileFile() { m_isDeleteFile = true; }
			public static void ReloadFileInfomations()
			{
				m_allFileNames = new List<string>();
				m_allFileConvertNames = new Dictionary<string, string>();
				m_usingFiles = new Dictionary<string, bool>();

				string[] path = System.IO.Directory.GetFiles(AI.BehaviorTree.BehaviorTree.dataSavePath);
				foreach (var e in path)
				{
					if (FileAccessor.IsExistMark(e, AI.BehaviorTree.BehaviorTree.cFileBeginMark))
						m_allFileNames.Add(e);
				}

				for (int i = 0; i < m_allFileNames.Count; ++i)
				{
					int findBegin = Mathf.Max(m_allFileNames[i].LastIndexOf('\\'), m_allFileNames[i].LastIndexOf('/')) + 1;
					int findLast = m_allFileNames[i].LastIndexOf('.');
					m_allFileConvertNames.Add(m_allFileNames[i], m_allFileNames[i].Substring(findBegin, findLast - findBegin));

					m_usingFiles.Add(m_allFileConvertNames[m_allFileNames[i]], false);
				}
				foreach (var e in instances)
				{
					if (m_usingFiles.ContainsKey(e.fileName))
						m_usingFiles[e.fileName] = true;
				}
			}
			void ReloadSelectFileContents()
			{
				List<string> result = new List<string>();

				if (m_allFileNames.Count == 0)
					result.Add("File not found. Select add file...");
				else
				{
					result.Add("　　");

					foreach (var e in m_allFileConvertNames)
						if (!m_usingFiles[e.Value] || e.Value == fileName) result.Add(e.Value);
				}
				result.Add("Add file...");
				m_selectFileContents = result.ToArray();
			}

			/// <summary>Open</summary>
			[MenuItem("Window/Misotempra/Behavior tree")]
			static void Open()
			{
				//インスタンス作成
				instances.Add(CreateInstance<BehaviorTreeWindow>());
				//コールバック追加
				if (!m_isAddSaveCallback)
				{
					EditorApplication.playModeStateChanged += SaveCallaback;
					m_isAddSaveCallback = true;
				}

				//表示
				instances[instances.Count - 1].Show();
			}

			void OnEnable()
			{
				//名前リスト作成
				ReloadFileInfomations();
				ReloadSelectFileContents();

				CalculateSizes();

				m_nodeView = new BehaviorTreeNodeView(this);
				rootVisualElement.Add(m_nodeView);
			}

			/// <summary>OnGUI</summary>
			void OnGUI()
			{
				//if (m_isCreateInput)
				//{
				//	switch (m_createResult)
				//	{
				//		case CreateResult.Null: return;
				//		case CreateResult.Close:
				//			if (m_allFileNames.Count == 0) m_selectFileIndex = 0;
				//			else m_selectFileIndex = m_oldSelectFileIndex;
				//			break;
				//		case CreateResult.Create:
				//			CreateEmptyFile(m_createName);
				//			ReloadFileInfomations();
				//			ReloadSelectFileContents();
				//			m_selectFileIndex = 0;
				//			for (int i = 0; i < m_selectFileContents.Length; ++i)
				//				if (fileName == m_selectFileContents[i])
				//				{
				//					m_selectFileIndex = i;
				//					break;
				//				}
				//			break;
				//		default: return;
				//	}

				//	m_isCreateInput = false;
				//}

				//m_oldSelectFileIndex = m_selectFileIndex;
				//m_selectFileIndex = EditorGUI.Popup(
				//	new Rect(m_cSelectFileFieldPosition, m_selectFileFieldSize),
				//	m_selectFileIndex, m_selectFileContents);

				//if (GUI.changed)
				//{
				//	m_nodes = null;

				//	if (m_selectFileIndex == 0) return;
				//	else if (m_selectFileIndex == m_selectFileContents.Length - 1)
				//	{
				//		if (m_isCreateInput) return;

				//		m_isCreateInput = true;
				//		m_createResult = CreateResult.Null;
				//		var window = EditorWindow.GetWindow<BehaviorTreeCreateWindow>(true, "BehaviorTreeCreateWindow");
				//		window.SetParentWindow(this);
				//		return;
				//	}
				//	else
				//		Load(m_allFileConvertNames[m_allFileNames[m_selectFileIndex]]);
				//}

				//if (m_nodes != null) Draw();
			}
			/// <summary>OnDestroy</summary>
			void OnDestroy()
			{
				//インスタンスがあればSave->削除
				if (instances.Contains(this))
				{
					Save();
					instances.Remove(this);
				}
			}

			void CalculateSizes()
			{
				//m_selectFileFieldSize = EditorStyles.popup.GetSize(new string('A', m_cMaxFileNameLength));


			}

			/// <summary>セーブを行う</summary>
			void Save()
			{
				if (fileName == null || fileName.Length == 0 || m_isDeleteFile) return;

				//セーブ
				FileAccessor.SaveObject(AI.BehaviorTree.BehaviorTree.dataSavePath, fileName, ref m_nodes, AI.BehaviorTree.BehaviorTree.cFileBeginMark);
			}
			/// <summary>EditorApplication用コールバック</summary>
			static void SaveCallaback(PlayModeStateChange change)
			{
				//プレイモードになった場合セーブを行う
				if (change == PlayModeStateChange.EnteredPlayMode)
				{
					foreach (var e in instances)
						e.Save();
				}
			}


			/// <summary>ロードを行う</summary>
			void Load(string fileName)
			{
				this.fileName = fileName;

				FileAccessor.LoadObject(AI.BehaviorTree.BehaviorTree.dataSavePath, fileName, out m_nodes, AI.BehaviorTree.BehaviorTree.cFileBeginMark);
			}

			void CreateEmptyFile(string fileName)
			{
				//リストクリア
				m_nodes = new List<BehaviorSaveNode>();
				this.fileName = fileName;

				m_nodes.Add(new BehaviorSaveNode("root", System.Guid.NewGuid().ToString(),
					new Rect(new Vector2(this.position.size.x * 0.5f, m_selectFileFieldSize.y + 10.0f),
						EditorStyles.popup.GetSize("root") * new Vector2(2.0f, 2.0f)),
					new BehaviorSaveNode.CompositeNodeInfo("AI.BehaviorTree.BehaviorCompositeSelectorNode")));

				Save();
			}

			void Draw()
			{
				BeginWindows();
				for (int i = 0; i < m_nodes.Count; ++i)
					m_nodes[i].SetWindowRect(GUI.Window(i, m_nodes[i].windowRect,
						DrawNodeWindow, m_nodes[i].name));
				EndWindows();
			}

			void DrawNodeWindow(int id)
			{

				GUI.DragWindow();
			}
		}
	}
}