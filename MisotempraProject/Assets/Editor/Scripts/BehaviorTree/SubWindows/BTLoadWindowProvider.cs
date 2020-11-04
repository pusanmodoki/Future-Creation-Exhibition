using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		/// <summary>Sub windows</summary>
		namespace SubWindow
		{
			public class BTLoadWindowProvider : UnityEngine.ScriptableObject, ISearchWindowProvider
			{
				BehaviorTreeNodeView m_view;

				public void Initialize(BehaviorTreeNodeView graphView)
				{
					m_view = graphView;
				}

				List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
				{
					var entries = new List<SearchTreeEntry>();
					entries.Add(new SearchTreeGroupEntry(new GUIContent("Behavior file")));
					
					entries.Add(new SearchTreeGroupEntry(new GUIContent("Load")) { level = 1 });

					string[] path = System.IO.Directory.GetFiles(AI.BehaviorTree.BehaviorTree.dataSavePath);
					foreach (var e in path)
					{
						if (FileAccess.FileAccessor.IsExistMark(e, AI.BehaviorTree.BehaviorTree.cFileBeginMark))
						{
							int findBegin = Mathf.Max(e.LastIndexOf('\\'), e.LastIndexOf('/')) + 1;
							int findLast = e.LastIndexOf('.');
							string name = e.Substring(findBegin, findLast - findBegin);

							foreach (var window in BehaviorTreeWindow.instances)
								if (window.fileName == name) continue;

							entries.Add(new SearchTreeEntry(new GUIContent(name, e)) { level = 2, userData = true });
						}
					}

					entries.Add(new SearchTreeGroupEntry(new GUIContent("New file")) { level = 1 });
					entries.Add(new SearchTreeEntry(new GUIContent("Open create window...")) { level = 2, userData = false });

					return entries;
				}

				bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
				{
					if ((bool)searchTreeEntry.userData)
						m_view.DoLoadCallback(searchTreeEntry.content.text);
					else
						m_view.DoLoadCallback(null);

					return true;
				}
			}
		}
	}
}