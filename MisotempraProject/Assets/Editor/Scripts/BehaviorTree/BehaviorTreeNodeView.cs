using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using Editor.BehaviorTree.SubWindow;
using AI.BehaviorTree.CashContainer;
using AI.BehaviorTree.CashContainer.Detail;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		public class BehaviorTreeNodeView : NodeView.BaseNodeView
		{
			public List<BaseCashContainer> cashContainers { get; private set; } = new List<BaseCashContainer>();
			public Dictionary<string, BaseCashContainer> cashContainersKeyGuid { get; private set; } = new Dictionary<string, BaseCashContainer>();
			public Dictionary<Node, BaseCashContainer> cashContainersKeyNode { get; private set; } = new Dictionary<Node, BaseCashContainer>();
			public string fileName { get { return m_thisWindow.fileName; } set { m_thisWindow.fileName = value; } } 

			public ScriptableObject.Detail.BTBaseScriptableObject scriptableObject { get { return m_scriptableObject; } }
			public UnityEditor.Editor scriptableEditor { get { return m_scriptableEditor; } }

			static readonly Vector2 m_cRootPosition = Vector2.zero;

			protected ScriptableObject.Detail.BTBaseScriptableObject m_scriptableObject = null;
			protected UnityEditor.Editor m_scriptableEditor = null;

			Node m_selectNode = null;
			BehaviorTreeWindow m_thisWindow = null;

			string m_reloadGuid = null;
			bool m_isDelayCallCreateCallback = false;

			//protected override bool canDuplicateSelection => false;

			public void AddCash(BaseCashContainer cash, Node node,
				string nodeName, string className, string editNodeClassName, Vector2 position)
			{
				cashContainers.Add(cash);
				cashContainers.Back().Initialize(nodeName, className, editNodeClassName, position);

				cashContainersKeyGuid.Add(cash.guid, cash);
				cashContainersKeyNode.Add(node, cash);
			}

			public void ChangeChildrenOrder(string parentGuid)
			{
				m_reloadGuid = parentGuid;
			}

			public Rect LocalMousePositionToNodePosition(SearchWindowContext context, Rect rect)
			{
				var worldMousePosition = m_thisWindow.rootVisualElement.ChangeCoordinatesTo(
					m_thisWindow.rootVisualElement.parent, context.screenMousePosition - m_thisWindow.position.position);
				rect.position = contentViewContainer.WorldToLocal(worldMousePosition);
				return rect;
			}

			public BehaviorTreeNodeView(BehaviorTreeWindow thisWindow) : base("BehaviorTreeUSS")
			{
				m_thisWindow = thisWindow;
				EditorApplication.update += Update;

				nodeCreationRequest += context =>
				{	
					var searchWindowProvider = UnityEngine.ScriptableObject.CreateInstance<BTNewNodeWindowProvider>();
					searchWindowProvider.Initialize(this);
					SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindowProvider);
				};

				graphViewChanged += GraphViewChangeCallback;


				if (fileName != null && fileName.Length > 0)
					this.Load(fileName);
			}
			GraphViewChange GraphViewChangeCallback(GraphViewChange graphViewChange)
			{
				if (graphViewChange.edgesToCreate != null)
				{
					foreach(var edge in graphViewChange.edgesToCreate)
					{
						int index = 0;

						var root = cashContainersKeyNode[edge.output.node] as RootCashContainer;
						if (root != null)
						{
							root.childrenNodesGuid.Add(cashContainersKeyNode[edge.input.node].guid);
							index = root.childrenNodesGuid.Count;
						}
						var composite = cashContainersKeyNode[edge.output.node] as CompositeCashContainer;
						if (composite != null)
						{
							composite.childrenNodesGuid.Add(cashContainersKeyNode[edge.input.node].guid);
							index = composite.childrenNodesGuid.Count;
						}

						(cashContainersKeyNode[edge.input.node] as NotRootCashContainer).parentGuid 
							= cashContainersKeyNode[edge.output.node].guid;
						edge.input.node.title = index + ": "+ cashContainersKeyNode[edge.input.node].nodeName;
					}
				}

				if (graphViewChange.elementsToRemove != null)
				{
					foreach(var element in graphViewChange.elementsToRemove)
					{
						if (element == null) continue;

						var node = element as Node;
						if (node != null)
						{
							string parentGuid = (cashContainersKeyNode[node] as NotRootCashContainer).parentGuid;
							string deleteGuid = cashContainersKeyNode[node].guid;
							cashContainersKeyGuid.Remove(cashContainersKeyNode[node].guid);
							cashContainers.Remove(cashContainersKeyNode[node]);
							cashContainersKeyNode.Remove(node);

							if (parentGuid == null || parentGuid.Length == 0)
								continue;

							var root = cashContainersKeyGuid[parentGuid] as RootCashContainer;
							if (root != null)
								root.childrenNodesGuid.Remove(deleteGuid);

							var composite = cashContainersKeyGuid[parentGuid] as CompositeCashContainer;
							if (composite != null)
								composite.childrenNodesGuid.Remove(deleteGuid);

							ReloadChildrensTitle(parentGuid);
						}

						var edge = element as Edge;
						if (edge != null)
						{
							var root = cashContainersKeyNode[edge.output.node] as RootCashContainer;
							if (root != null)
								root.childrenNodesGuid.Remove(cashContainersKeyNode[edge.input.node].guid);

							var composite = cashContainersKeyNode[edge.output.node] as CompositeCashContainer;
							if (composite != null)
								composite.childrenNodesGuid.Remove(cashContainersKeyNode[edge.input.node].guid);

							(cashContainersKeyNode[edge.input.node] as NotRootCashContainer).parentGuid = "";
							ReloadChildrensTitle(cashContainersKeyNode[edge.output.node].guid);
						}
					}
				}

				if (graphViewChange.movedElements != null)
				{
					foreach (var element in graphViewChange.movedElements)
					{
						if (element == null) continue;

						var node = element as Node;
						if (node != null) cashContainersKeyNode[node].position = node.GetPosition().position;
					}
				}
				return graphViewChange;
			}

			public override List<Port> GetCompatiblePorts(Port startAnchor, NodeAdapter nodeAdapter)
			{
				var compatiblePorts = new List<Port>();

				compatiblePorts.AddRange(ports.ToList().Where(port =>
				{
					if (port == null)
						return false;

					// 同じノードには繋げない
					if (startAnchor.node == port.node)
						return false;

					// Input同士、Output同士は繋げない
					if (port.direction == startAnchor.direction)
						return false;

					// ポートの型が一致していない場合は繋げない
					if (port.portType != startAnchor.portType)
						return false;

					return true;
				}));

				return compatiblePorts;
			}

			public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
			{
				base.BuildContextualMenu(evt);
				
				evt.menu.AppendAction("Save", SaveCallback,
					action => fileName != null && fileName.Length > 0 ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled,
					evt.mousePosition);

				evt.menu.AppendAction("Load", LoadCallback,
					action => System.IO.Directory.GetFiles(AI.BehaviorTree.BehaviorTree.dataSavePath).Length > 0 
						? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled,
					evt.mousePosition);

				evt.menu.AppendAction("Create", CreateCallback, action => DropdownMenuAction.Status.Normal, evt.mousePosition);
			}

			public void LoadCallback(DropdownMenuAction action)
			{
				var searchWindowProvider = UnityEngine.ScriptableObject.CreateInstance<BTLoadWindowProvider>();
				searchWindowProvider.Initialize(this);
				SearchWindow.Open(new SearchWindowContext((Vector2)action.userData), searchWindowProvider);
			}
			public void SaveCallback(DropdownMenuAction action)
			{
				try { this.Save(); }
				catch(System.Exception e)
				{
					Debug.LogError(e.Message + "\n" + e.Source);
					return;
				}
				Debug.Log("Behavior tree (" + fileName + ") Save completed.");
			}
			public void CreateCallback(DropdownMenuAction action)
			{
				var content = new BTCreateWindow();
				content.Initialize(this);

				// 開く
				UnityEditor.PopupWindow.Show(Rect.zero, content);
			}

			public void DoLoadCallback(string name)
			{
				if (name == null)
					m_isDelayCallCreateCallback = true;
				else
					this.Load(name);
			}
			public void DoCreateCallback(string name)
			{
				if (name == null) return;

				this.CreateEmptyFile(name, m_cRootPosition);
			}

			void Update()
			{
				if (m_isDelayCallCreateCallback)
				{
					m_isDelayCallCreateCallback = false;
					CreateCallback(null);
				}

				Node oldSelect = m_selectNode;
				m_selectNode = null;
				foreach (var node in nodes.ToList())
					if (node.selected) { m_selectNode = node; break; }

				if (oldSelect != m_selectNode)
				{
					if (m_selectNode == null)
					{
						m_thisWindow.UnregisterEditorGUI();
					}
					else
					{
						m_scriptableObject = UnityEngine.ScriptableObject.CreateInstance(
							BTClassMediator.scriptableObjects[cashContainersKeyNode[m_selectNode].nodeName]) 
							as ScriptableObject.Detail.BTBaseScriptableObject;
						m_scriptableObject.Initialize(cashContainersKeyNode[m_selectNode], cashContainers);

						m_scriptableEditor = BTClassMediator.CreateEditor(this, m_selectNode,  m_scriptableObject);

						m_thisWindow.RegisterEditorGUI();
					}
				}

				if (m_reloadGuid != null)
				{
					ReloadChildrensTitle(m_reloadGuid);
					m_reloadGuid = null;
				}
			}

			public void BuildNodeView(List<BaseCashContainer> loadList)
			{
				Dictionary<string, Node> nodesKeyGuid = new Dictionary<string, Node>();

				for (int i = 0; i < loadList.Count; ++i)
				{
					Node node = (Node)(System.Activator.CreateInstance(System.Type.GetType(loadList[i].editNodeClassName), this));
					node.userData = loadList[i].nodeName;
					AddElement(node);

					Rect position = node.GetPosition();
					position.position = loadList[i].position;
					node.SetPosition(position);

					cashContainers.Add(loadList[i]);
					cashContainersKeyGuid.Add(loadList[i].guid, loadList[i]);
					cashContainersKeyNode.Add(node, loadList[i]);

					nodesKeyGuid.Add(loadList[i].guid, node);
				}
				TitleBuild(nodesKeyGuid, cashContainers[0].guid, 0, true);

				foreach(var cash in cashContainers)
				{
					var root = cash as RootCashContainer;
					var composite = cash as CompositeCashContainer;
					List<string> useChildrensGuid = null;

					if (root != null) useChildrensGuid = root.childrenNodesGuid;
					else if (composite != null) useChildrensGuid = composite.childrenNodesGuid;
					else continue;

					foreach (var children in useChildrensGuid)
					{
						var edge = new Edge()
						{
							output = nodesKeyGuid[cash.guid].outputContainer.ElementAt(0) as Port,
							input = nodesKeyGuid[children].inputContainer.ElementAt(0) as Port
						};
						edge.input.Connect(edge);
						edge.output.Connect(edge);
						Add(edge);
					}
				}
			}
			void TitleBuild(Dictionary<string, Node> nodesKeyGuid, string guid, int index, bool isRoot)
			{
				if (!isRoot)
					nodesKeyGuid[guid].title = index + ": " + cashContainersKeyGuid[guid].nodeName;

				List<string> childrens = null;
				{
					var cast = cashContainersKeyGuid[guid] as RootCashContainer;
					if (cast != null) childrens = cast.childrenNodesGuid;
				}
				if (childrens != null)
				{
					var cast = cashContainersKeyGuid[guid] as CompositeCashContainer;
					if (cast != null) childrens = cast.childrenNodesGuid;
				}

				if (childrens == null) return;

				for (int i = 0; i < childrens.Count; ++i)
				{
					TitleBuild(nodesKeyGuid, childrens[i], i + 1, false);
				}
			}
			void ReloadChildrensTitle(string guid)
			{
				List<string> childrens = null;
				{
					var cast = cashContainersKeyGuid[guid] as RootCashContainer;
					if (cast != null) childrens = cast.childrenNodesGuid;
				}
				if (childrens != null)
				{
					var cast = cashContainersKeyGuid[guid] as CompositeCashContainer;
					if (cast != null) childrens = cast.childrenNodesGuid;
				}

				for (int i = 0; i < childrens.Count; ++i)
				{
					var key = cashContainersKeyNode.First(x => x.Value.guid == childrens[i]).Key;
					key.title = (i + 1) + ": " + cashContainersKeyNode[key].nodeName;
				}
			}
		}	
	}
}
