using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

/// <summary>MisoTempra editor</summary>
namespace LocalEditor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		/// <summary>Sub windows</summary>
		namespace SubWindow
		{
			public class TaskClassWindowProvider : UnityEngine.ScriptableObject, ISearchWindowProvider
			{
				NodeEditor.BTBaseNodeEditor m_editor = null;

				public void Initialize(NodeEditor.BTBaseNodeEditor editor)
				{
					m_editor = editor;
				}

				List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
				{
					List<SearchTreeEntry> entries = new List<SearchTreeEntry>();

					entries.Add(new SearchTreeGroupEntry(new GUIContent("Find classes")));

					foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
					{
						foreach (var type in assembly.GetTypes())
						{
							if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(AI.BehaviorTree.BaseTask)))
							{
								entries.Add(new SearchTreeEntry(new GUIContent(type.Name)) { level = 1, userData = type });
							}
						}
					}

					return entries;
				}

				bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
				{
					m_editor.CreateTaskCallback(searchTreeEntry);
					return true;
				}
			}
		}
	}
}