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
			public class BTNewNodeWindowProvider : UnityEngine.ScriptableObject, ISearchWindowProvider
			{
				BehaviorTreeNodeView m_view;

				public void Initialize(BehaviorTreeNodeView graphView)
				{
					m_view = graphView;
				}

				List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
				{
					List<SearchTreeEntry> entries = new List<SearchTreeEntry>();

					if (m_view.fileName == null || m_view.fileName.Length == 0)
					{
						entries.Add(new SearchTreeGroupEntry(new GUIContent("Not loaded…")));
						return entries;
					}

					entries.Add(new SearchTreeGroupEntry(new GUIContent("Node classes")));

					entries.Add(new SearchTreeGroupEntry(new GUIContent("Composite")) { level = 1 });
					foreach (var name in BTClassMediator.compositeNodeNames)
						entries.Add(new SearchTreeEntry(new GUIContent(name)) { level = 2, userData = BTClassMediator.nodeTypes[name] });

					entries.Add(new SearchTreeGroupEntry(new GUIContent("Task")) { level = 1 });
					entries.Add(new SearchTreeEntry(new GUIContent(BTClassMediator.cTaskNodeName)) { level = 2, userData = BTClassMediator.nodeTypes[BTClassMediator.cTaskNodeName] });

					return entries;
				}

				bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
				{
					if (searchTreeEntry == null) return true;

					Node node = (Node)System.Activator.CreateInstance((System.Type)searchTreeEntry.userData, m_view);
					node.SetPosition(m_view.LocalMousePositionToNodePosition(context, node.GetPosition()));
					node.name = searchTreeEntry.content.text;
					node.userData = searchTreeEntry.content.text;
					m_view.AddElement(node);
	
					var cashType = BTClassMediator.cashTypes[searchTreeEntry.content.text];		
					m_view.AddCash((CashContainer.Detail.BaseCashContainer)System.Activator.CreateInstance(cashType), 
						node, searchTreeEntry.content.text,
						BTClassMediator.classTypes[searchTreeEntry.content.text].FullName,
						BTClassMediator.nodeTypes[searchTreeEntry.content.text].FullName,
						m_view.LocalMousePositionToNodePosition(context, node.GetPosition()).position);
					
					return true;
				}
			}
		}
	}
}