using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>Node view(Graph view)</summary>
	namespace NodeView
	{
		public class BaseNodeView : GraphView
		{
			public BaseNodeView(string styleResourceName) : base()
			{
				if (styleResourceName != null && styleResourceName.Length > 0)
					styleSheets.Add(Resources.Load<StyleSheet>(styleResourceName));

				// 親のUIにしたがって拡大縮小を行う設定
				style.flexGrow = 1;
				style.flexShrink = 1;

				// ズーム倍率の上限設定
				SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

				// 背景の設定
				var gridBackground = new GridBackground();
				Insert(0, gridBackground);
				gridBackground.StretchToParentSize();

				// UIElements上でのドラッグ操作などの検知
				this.AddManipulator(new ContentZoomer());
				this.AddManipulator(new SelectionDragger());
				this.AddManipulator(new ContentDragger());
				this.AddManipulator(new RectangleSelector());
			}
			public override List<Port> GetCompatiblePorts(Port startAnchor, NodeAdapter nodeAdapter)
			{
				return ports.ToList();
			}

			public void ClearGraph()
			{
				nodes.ToList().ForEach(RemoveElement);
				ports.ToList().ForEach(RemoveElement);
				edges.ToList().ForEach(RemoveElement);
			}
		}
	}
}