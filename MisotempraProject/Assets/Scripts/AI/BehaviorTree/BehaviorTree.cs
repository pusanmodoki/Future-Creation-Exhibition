using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		public class BehaviorTree
		{
			/// <summary>Data save path</summary>
			public static string dataSavePath { get { return Application.streamingAssetsPath + "/AI/BehaviorFile"; } }
			public static readonly string cFileBeginMark = "<<B>>BehaviorTreeNodeDataBeginMark";
#if UNITY_EDITOR
			/// <summary>Data save path (Assetから, Editor用)</summary>
			public static readonly string cDUseEditorDataSavePath = "Assets/StreamingAssets/AI/BehaviorFile";
#endif

			public ReadOnlyCollection<Node.Detail.BaseNode> nodes { get; private set; } = null;
			public ReadOnlyDictionary<string, BaseTask> subsequentTasks { get; private set; } = null;
			public Node.RootNode rootNode { get; private set; } = null;
			public AIAgent aiAgent { get; private set; } = null;
			public Blackboard blackboard { get; private set; } = null;
			public string fileName { get; private set; } = "";
			public bool isLoaded { get { return m_masterDatum.ContainsKey(fileName); } }

			static Dictionary<string, BehaviorTree> m_masterDatum = new Dictionary<string, BehaviorTree>();

			List<Node.Detail.BaseNode> m_nodes = new List<Node.Detail.BaseNode>();
			Dictionary<string, Node.Detail.BaseNode> m_nodesKeyGuid = new Dictionary<string, Node.Detail.BaseNode>();

			Dictionary<string, BaseTask> m_subsequentTasks = new Dictionary<string, BaseTask>();
			List<CashContainer.Detail.SubsequentTaskInfomations> m_subsequentTaskInfos = null;
			Node.TaskNode m_nowTask = null;

			public void RegisterTask(Node.TaskNode node) { m_nowTask = node; }
			public void UnregisterTask() { m_nowTask = null; }

			public BehaviorTree()
			{
				nodes = new ReadOnlyCollection<Node.Detail.BaseNode>(m_nodes);
				subsequentTasks = new ReadOnlyDictionary<string, BaseTask>(m_subsequentTasks);
			}
			public BehaviorTree(string fileName, AIAgent aiAgent, BaseBlackboardInitialzier blackboardInitialzier)
			{
				nodes = new ReadOnlyCollection<Node.Detail.BaseNode>(m_nodes);
				subsequentTasks = new ReadOnlyDictionary<string, BaseTask>(m_subsequentTasks);
				this.aiAgent = aiAgent;
				if (m_masterDatum.ContainsKey(fileName))
					LoadMasterData(m_masterDatum[fileName], aiAgent, blackboardInitialzier);
				else
				{
					LoadBehaviorTree(fileName);
					if (m_masterDatum.ContainsKey(fileName))
						LoadMasterData(m_masterDatum[fileName], aiAgent, blackboardInitialzier);
				}
			}

			public void OnDestroy()
			{
				blackboard?.OnDestroy();
				rootNode?.OnDisable(UpdateResult.Success);
			}

			public void Update()
			{
				rootNode?.Update(aiAgent, blackboard);
			}
			public void FixedUpdate()
			{
				m_nowTask?.FixedUpdate();
			}
			public void ForceReschedule()
			{
				m_nowTask?.OnDisable(UpdateResult.ForceReschedule);
				m_nowTask = null;

				rootNode.OnEnable();
			}

			public static void LoadBehaviorTree(string fileName)
			{
				if (m_masterDatum.ContainsKey(fileName)) return;

				BehaviorTree instance = new BehaviorTree();
				m_masterDatum.Add(fileName, instance);

				List<CashContainer.Detail.BaseCashContainer> loadList = null;
				FileAccess.FileAccessor.LoadObject(dataSavePath, fileName, out loadList, cFileBeginMark);
				instance.fileName = fileName;

				{
					var root = (loadList[0] as CashContainer.RootCashContainer);
					var blackboardCash = root.blackbord;
					instance.blackboard = new Blackboard(fileName, blackboardCash.classNameIndexes,
						blackboardCash.keys, blackboardCash.memos, blackboardCash.isShareds);

					instance.m_subsequentTaskInfos = new List<CashContainer.Detail.SubsequentTaskInfomations>();
					foreach (var task in root.subsequentTasks)
						instance.m_subsequentTaskInfos.Add(task);
				}

				foreach (var container in loadList)
				{
					var node = System.Activator.CreateInstance(
						System.Type.GetType(container.className));

					instance.m_nodes.Add(node as Node.Detail.BaseNode);
					instance.m_nodes.Back().BaseInitialize(null, container);
					instance.m_nodes.Back().Load(container, instance.blackboard);

					instance.m_nodesKeyGuid.Add(container.guid, instance.m_nodes.Back());
				}
				instance.rootNode = instance.m_nodes[0] as Node.RootNode;

				foreach (var container in loadList)
				{
					var root = container as CashContainer.RootCashContainer;
					var composite = container as CashContainer.CompositeCashContainer;
					List<string> childrensGuid = null;

					if (root != null) childrensGuid = root.childrenNodesGuid;
					else if (composite != null) childrensGuid = composite.childrenNodesGuid;
					else continue;

					var node = instance.m_nodesKeyGuid[container.guid];
					foreach (var guid in childrensGuid)
					{
						var childrenNode = instance.m_nodesKeyGuid[guid];
						node.RegisterChildren(childrenNode);
						childrenNode.RegisterParent(node);
					}
				}
			}

			public void LoadMasterData(BehaviorTree masterData, AIAgent aiAgent, BaseBlackboardInitialzier blackboardInitialzier)
			{
				blackboard = new Blackboard(masterData.blackboard);
				if (blackboard.isFirstInstance)
					blackboardInitialzier?.InitializeFirstInstance(blackboard);
				blackboardInitialzier?.InitializeAllInstance(blackboard);

				foreach(var task in masterData.m_subsequentTaskInfos)
					m_subsequentTasks.Add(task.key,
						(BaseTask)JsonUtility.FromJson(task.jsonData, System.Type.GetType(task.className)));

				foreach (var node in masterData.m_nodes)
				{
					m_nodes.Add(node.Clone(aiAgent, this));
					m_nodesKeyGuid.Add(node.guid, m_nodes.Back());
				}
				foreach(var node in masterData.m_nodes)
				{
					var thisNode = m_nodesKeyGuid[node.guid];
					foreach (var children in node.childrenNodes)
					{
						var childrenNode = m_nodesKeyGuid[children.guid];

						thisNode.RegisterChildren(childrenNode);
						childrenNode.RegisterParent(thisNode);
					}
				}

				rootNode = m_nodes[0] as Node.RootNode;

				if (rootNode.OnEnable() == EnableResult.Failed)
					throw new System.InvalidOperationException("Behavior tree data copy failed.");
			}
		}
	}
}