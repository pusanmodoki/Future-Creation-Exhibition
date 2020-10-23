using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		public class BehaviorTreeNodeView : NodeView.BaseNodeView
		{
			BehaviorTreeWindow m_thisWindow = null;

			//protected override bool canDuplicateSelection => false;

			public BehaviorTreeNodeView(BehaviorTreeWindow thisWindow) : base("BehaviorTreeUSS")
			{
				m_thisWindow = thisWindow;
			}
			public override List<Port> GetCompatiblePorts(Port startAnchor, NodeAdapter nodeAdapter)
			{
				return ports.ToList();
			}

			public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
			{
				base.BuildContextualMenu(evt);
				
				evt.menu.AppendAction("Save", SaveCallback,
					(Func<DropdownMenuAction, DropdownMenuAction.Status>)(a => true ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled),
					evt.mousePosition);

				evt.menu.AppendAction("Load", LoadCallback, 
					(Func<DropdownMenuAction, DropdownMenuAction.Status>)(a => true ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled),
					evt.mousePosition);

				evt.menu.AppendAction("Create", SaveCallback,
					(Func<DropdownMenuAction, DropdownMenuAction.Status>)(a => true ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled),
					evt.mousePosition);
			}

			void LoadCallback(DropdownMenuAction action)
			{

			}
			void SaveCallback(DropdownMenuAction action)
			{
				// クリックした位置を視点とするRectを作る
				// 本来のポップアップの用途として使う場合はボタンのRectを渡す
				var mouseRect = new Rect((Vector2)action.userData, Vector2.one);
				// PopupWindowContentを生成
				var content = new BehaviorTreeCreateWindow();

				// 開く
				content.SetParentWindow(m_thisWindow);
				UnityEditor.PopupWindow.Show(mouseRect, content);
			}
		}	
	}
}