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
				nodeList.Add(new RootCashContainer());
				nodeList.Back().Initialize("Root",
					typeof(AI.BehaviorTree.Node.RootNode).FullName,
					typeof(EditNode.BTEditRootNode).FullName, position);
				nodeView.BuildNodeView(nodeList);
				nodeView.CreateBlackboardEditor();

				var saveList = nodeView.cashContainers;
				FileAccess.FileAccessor.SaveObject(AI.BehaviorTree.BehaviorTree.dataSavePath, name, ref saveList, AI.BehaviorTree.BehaviorTree.cFileBeginMark);

				nodeView.fileName = name;
			}

			public static void Save(this BehaviorTreeNodeView nodeView)
			{
				if (nodeView.fileName == null || nodeView.cashContainers == null)
					throw new System.NullReferenceException("File not loaded.");

				List<string> throws = new List<string>();
				bool isBlackbordThrow = false;

				if (!(nodeView.cashContainers[0] as RootCashContainer).isBlackboardSaveReady)
					isBlackbordThrow = true;

				foreach (var cash in nodeView.cashContainers)
					if (!cash.isSaveReady)
						throws.Add(cash.nodeName + "node");

				if (throws.Count > 0 || isBlackbordThrow)
				{
					string str = "Not save ready. Cause";

					if (isBlackbordThrow)
						str += ": Blackbord contents,\n";
					else str += "↓\n";

					for (int i = 0; i < throws.Count; ++i)
					{
						str += throws[i];
						if ((i + 1) % 3 == 0) str += "\n";
						else str = ",";
					}

					throw new System.InvalidOperationException(str);
				}


				var saveList = nodeView.cashContainers;
				FileAccess.FileAccessor.SaveObject(AI.BehaviorTree.BehaviorTree.dataSavePath, nodeView.fileName, 
					ref saveList, AI.BehaviorTree.BehaviorTree.cFileBeginMark);
			}
			public static bool ForceSave(this BehaviorTreeNodeView nodeView)
			{
				bool isResult = true;
				if (nodeView.fileName == null || nodeView.cashContainers == null)
					throw new System.NullReferenceException("File not loaded.");

				foreach (var cash in nodeView.cashContainers)
					if (!cash.isSaveReady)
					{
						isResult = false;
						break;
					}
				if (!(nodeView.cashContainers[0] as RootCashContainer).isBlackboardSaveReady)
					isResult = false;

				var saveList = nodeView.cashContainers;
				FileAccess.FileAccessor.SaveObject(AI.BehaviorTree.BehaviorTree.dataSavePath, nodeView.fileName,
					ref saveList, AI.BehaviorTree.BehaviorTree.cFileBeginMark);

				return isResult;
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
				nodeView.CreateBlackboardEditor();
			}
		}
	}
}