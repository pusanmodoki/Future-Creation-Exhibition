using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using CashContainer = AI.BehaviorTree.CashContainer;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		/// <summary>Sub windows</summary>
		namespace SubWindow
		{
			public class FindClassWindowProvider : UnityEngine.ScriptableObject, ISearchWindowProvider
			{
				NodeEditor.ReorderableLists.ClassList m_list;
				System.Type m_findType;

				public void Initialize(NodeEditor.ReorderableLists.ClassList list, System.Type findType)
				{
					m_list = list;
					m_findType = findType;
				}

				List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
				{
					List<SearchTreeEntry> entries = new List<SearchTreeEntry>();
					
					entries.Add(new SearchTreeGroupEntry(new GUIContent("Find classes")));

					foreach(var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
					{
						foreach(var type in assembly.GetTypes())
						{
							if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(m_findType))
							{
								entries.Add(new SearchTreeEntry(new GUIContent(type.Name)) { level = 1, userData = type.FullName });
							}
						}
					}

					return entries;
				}

				bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
				{
					if (searchTreeEntry == null) return true;

					m_list.AddCallback(searchTreeEntry.userData as string);
					return true;
				}
			}
		}
	}
}