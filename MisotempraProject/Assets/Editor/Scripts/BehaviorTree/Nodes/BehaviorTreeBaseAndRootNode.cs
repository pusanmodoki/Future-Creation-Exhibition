using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		public class BehaviorTreeBaseNode : NodeView.BaseNode
		{
			protected BehaviorTreeNodeView m_view { get; private set; } = null;

			public BehaviorTreeBaseNode(BehaviorTreeNodeView view) : base()
			{
				m_view = view;
			}
		}

		public class BehaviorTreeRootNode : BehaviorTreeBaseNode
		{
			public BehaviorTreeRootNode(BehaviorTreeNodeView view) : base(view)
			{
				title = "root";

				capabilities -= Capabilities.Deletable;
				capabilities -= Capabilities.Renamable;

				var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(Port));
				outputPort.portName = "Out";
				outputContainer.Add(outputPort);
			}
		}
	}
}