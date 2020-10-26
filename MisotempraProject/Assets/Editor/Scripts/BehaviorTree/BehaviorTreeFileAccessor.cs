using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using Editor.BehaviorTree.CashContainer.Detail;

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
				nodeView.ClearGraph();

				var node = new Node();
				var container = new CashContainer.RootCashContainer("root", typeof(BehaviorTreeRootNode).Name, position);
				Rect setPosition = node.GetPosition();

				node.title = "root";
				setPosition.position = position;
				node.SetPosition(setPosition);

				nodeView.AddElement(node);
				nodeView.cashContainers.Add(container);
				nodeView.cashContainersKeyGuid.Add(nodeView.cashContainers.Back().guid, nodeView.cashContainers.Back());

				var saveList = nodeView.cashContainers;
				FileAccess.FileAccessor.SaveObject(AI.BehaviorTree.BehaviorTree.dataSavePath, name, ref saveList, AI.BehaviorTree.BehaviorTree.cFileBeginMark);
			}

			public static void Save()
			{

			}

			public static void Load(this BehaviorTreeNodeView nodeView, string name)
			{
				List<BaseCashContainer> loadList = new List<BaseCashContainer>();
				//FileAccess.FileAccessor.LoadObject(AI.BehaviorTree.BehaviorTree.dataSavePath, name, )
			}
		}
	}
}