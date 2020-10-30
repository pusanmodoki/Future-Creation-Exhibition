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
		/// <summary>Behavior tree editor edit ndoe</summary>
		namespace EditNode
		{
			public class BTEditBaseNode : NodeView.BaseNode
			{
				protected BehaviorTreeNodeView m_view { get; private set; } = null;

				protected static readonly string m_cInputName = "Input";
				protected static readonly string m_cOutputName = "Output";

				public BTEditBaseNode(BehaviorTreeNodeView view) : base()
				{
					m_view = view;
				}
			}

			public class BTEditRootNode : BTEditBaseNode
			{
				public BTEditRootNode(BehaviorTreeNodeView view) : base(view)
				{
					title = "Root";

					capabilities -= Capabilities.Deletable;
					capabilities -= Capabilities.Renamable;

					var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(Port));
					outputPort.portName = m_cOutputName;
					outputContainer.Add(outputPort);
				}
			}
		}
	}
}