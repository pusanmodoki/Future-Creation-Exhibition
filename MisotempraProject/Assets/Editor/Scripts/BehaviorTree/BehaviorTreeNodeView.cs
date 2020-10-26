using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using Editor.BehaviorTree.FileAccessWindow;
using Editor.BehaviorTree.CashContainer.Detail;

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

			static readonly Vector2 m_cRootPosition = Vector2.zero;

			BehaviorTreeWindow m_thisWindow = null;
			SearchWindow m_searchWindow = null;

			double m_createCallbackSetTime = 0;
			bool m_isDelayCallCreateCallback = false;

			//protected override bool canDuplicateSelection => false;

			public BehaviorTreeNodeView(BehaviorTreeWindow thisWindow) : base("BehaviorTreeUSS")
			{
				m_thisWindow = thisWindow;
				EditorApplication.update += Update;

				// SampleGraphViewのメニュー周りのイベントを設定する処理
				nodeCreationRequest += context =>
				{
					// GraphViewの子要素として追加する
					AddElement(new BehaviorTreeBaseNode(this));
				};
				
				//AddElement(new BehaviorTreeRootNode(this));
			}
			public override List<Port> GetCompatiblePorts(Port startAnchor, NodeAdapter nodeAdapter)
			{
				return ports.ToList();
			}

			public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
			{
				base.BuildContextualMenu(evt);
				
				evt.menu.AppendAction("Save", SaveCallback,
					action => !false ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled,
					evt.mousePosition);

				evt.menu.AppendAction("Load", LoadCallback,
					action => System.IO.Directory.GetFiles(AI.BehaviorTree.BehaviorTree.dataSavePath).Length > 0 
						? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled,
					evt.mousePosition);

				evt.menu.AppendAction("Create", CreateCallback, action => DropdownMenuAction.Status.Normal, evt.mousePosition);
			}

			public void LoadCallback(DropdownMenuAction action)
			{
				var searchWindowProvider = ScriptableObject.CreateInstance<BehaviorTreeLoadWindowProvider>();
				searchWindowProvider.Initialize(this);
				SearchWindow.Open(new SearchWindowContext((Vector2)action.userData), searchWindowProvider);
			}
			public void SaveCallback(DropdownMenuAction action)
			{
			}
			public void CreateCallback(DropdownMenuAction action)
			{
				var content = new BehaviorTreeCreateWindow();
				content.Initialize(this);

				// 開く
				UnityEditor.PopupWindow.Show(Rect.zero, content);
			}

			public void DoLoadCallback(string path)
			{
				if (path == null)
				{
					m_createCallbackSetTime = EditorApplication.timeSinceStartup;
					m_isDelayCallCreateCallback = true;
				}
				else
				{

				}
			}
			public void DoSaveCallback()
			{
			}
			public void DoCreateCallback(string name)
			{
				if (name == null) return;

				this.CreateEmptyFile(name, m_cRootPosition);
			}

			void Update()
			{
				if (m_isDelayCallCreateCallback && 
					EditorApplication.timeSinceStartup - m_createCallbackSetTime > 0.1f)
				{
					m_isDelayCallCreateCallback = false;
					CreateCallback(null);
				}
			}
		}	
	}
}