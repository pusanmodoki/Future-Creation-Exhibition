using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using AI.BehaviorTree.CashContainer;
using AI.BehaviorTree.CashContainer.Detail;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		public static class BehaviorTreeFileAccessor
		{
			public static void CreateEmptyFile(this BehaviorTreeNodeView nodeView, string name, Vector2 position)
			{
				nodeView.cashContainers.Clear();
				nodeView.cashContainersKeyGuid.Clear();
				nodeView.cashContainersKeyNode.Clear();
				nodeView.ClearGraph();

				var nodeList = new List<BaseCashContainer>();
				nodeList.Add(new RootCashContainer("Root", 
					typeof(AI.BehaviorTree.BehaviorCompositeSequenceNode).FullName,
					typeof(EditNode.BTEditRootNode).FullName, position));
				nodeView.BuildNodeView(nodeList);

				var saveList = nodeView.cashContainers;
				FileAccess.FileAccessor.SaveObject(AI.BehaviorTree.BehaviorTree.dataSavePath, name, ref saveList, AI.BehaviorTree.BehaviorTree.cFileBeginMark);

				nodeView.fileName = name;
			}

			public static void Save(this BehaviorTreeNodeView nodeView)
			{
				if (nodeView.fileName == null)
					throw new System.NullReferenceException("File not loaded.");

				var saveList = nodeView.cashContainers;
				FileAccess.FileAccessor.SaveObject(AI.BehaviorTree.BehaviorTree.dataSavePath, nodeView.fileName, 
					ref saveList, AI.BehaviorTree.BehaviorTree.cFileBeginMark);
			}

			public static void Load(this BehaviorTreeNodeView nodeView, string name)
			{
				nodeView.cashContainers.Clear();
				nodeView.cashContainersKeyGuid.Clear();
				nodeView.cashContainersKeyNode.Clear();
				nodeView.ClearGraph();

				List<BaseCashContainer> loadList = null;
				FileAccess.FileAccessor.LoadObject(AI.BehaviorTree.BehaviorTree.dataSavePath, name, out loadList, AI.BehaviorTree.BehaviorTree.cFileBeginMark);
				nodeView.fileName = name;

				nodeView.BuildNodeView(loadList);
			}
		}
	}
}