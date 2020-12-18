using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AI
{
	namespace BehaviorTree
	{
		namespace Detail
		{
			[System.Serializable]
			public class BehaviorFileName
			{
				public string fileName { get { return m_fileName; } }

				[SerializeField]
				string m_fileName = "";

				public static implicit operator string(BehaviorFileName name)
				{
					return name.fileName;
				}
			}
		}
	}
}
#if UNITY_EDITOR
/// <summary>MisoTempra editor</summary>
namespace LocalEditor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		[UnityEditor.CustomPropertyDrawer(typeof(AI.BehaviorTree.Detail.BehaviorFileName))]
		class BehaviorFileNameDrawer : UnityEditor.PropertyDrawer
		{
			static List<BehaviorFileNameDrawer> m_instances = new List<BehaviorFileNameDrawer>();
			static System.IO.FileSystemWatcher m_watcher = null;
			static string[] m_selects = null;

			string m_selectName = "";
			int m_selectIndex = 0;
			bool m_isInit = false;

			public BehaviorFileNameDrawer() : base()
			{
				m_instances.Add(this);

				if (m_watcher == null)
				{
					m_watcher = new System.IO.FileSystemWatcher();
					m_watcher.Path = AI.BehaviorTree.BehaviorTree.dataSavePath;
					m_watcher.IncludeSubdirectories = false;
					m_watcher.NotifyFilter =
						System.IO.NotifyFilters.FileName
						| System.IO.NotifyFilters.DirectoryName
						| System.IO.NotifyFilters.LastWrite;

					m_watcher.Changed += ChangeFile;
					m_watcher.Created += ChangeFile;
					m_watcher.Deleted += ChangeFile;
					m_watcher.Renamed += ChangeFile;

					m_watcher.EnableRaisingEvents = true;
				}
			}
			~BehaviorFileNameDrawer()
			{
				m_instances.Remove(this);
			}

			public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
			{
				property.serializedObject.Update();
				SerializedProperty fileName = property.FindPropertyRelative("m_fileName");

				if (!m_isInit)
				{
					UpdateSelects(false);
					UpdateThisSelect(fileName.stringValue);
					m_isInit = true;
					return;
				}

				m_selectIndex = EditorGUI.Popup(position, "Behavior file", m_selectIndex, m_selects);

				if (GUI.changed)
				{
					fileName.stringValue = m_selects[m_selectIndex];
					property.serializedObject.ApplyModifiedProperties();
				}
			}

			void UpdateThisSelect(string name)
			{
				for (int i = 0; i < m_selects.Length; ++i)
				{
					if (m_selects[i] == name)
					{
						m_selectIndex = i;
						m_selectName = name;
						return;
					}
				}
			}

			static void UpdateSelects(bool allUpdate)
			{
				List<string> temp = new List<string>();

				string[] path = System.IO.Directory.GetFiles(AI.BehaviorTree.BehaviorTree.dataSavePath);
				foreach (var e in path)
				{
					if (FileAccess.FileAccessor.IsExistMark(e, AI.BehaviorTree.BehaviorTree.cFileBeginMark))
					{
						int findBegin = Mathf.Max(e.LastIndexOf('\\'), e.LastIndexOf('/')) + 1;
						int findLast = e.LastIndexOf('.');
						string name = e.Substring(findBegin, findLast - findBegin);

						temp.Add(name);
					}
				}

				m_selects = temp.ToArray();

				if (allUpdate)
				{
					foreach (var instance in m_instances)
						instance.UpdateThisSelect(instance.m_selectName);
				}
			}
			
			static void ChangeFile(object sender, System.IO.FileSystemEventArgs e)
			{
				UpdateSelects(true);
			}
		}
	}
}
#endif