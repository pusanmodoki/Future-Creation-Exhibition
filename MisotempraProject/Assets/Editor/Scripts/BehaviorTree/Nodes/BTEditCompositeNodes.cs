using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		/// <summary>Behavior tree editor edit ndoe</summary>
		namespace EditNode
		{
			public class BTEditSequenceNode : BTEditBaseNode
			{
				public BTEditSequenceNode(BehaviorTreeNodeView view) : base(view)
				{
					title = "Sequence";
					capabilities -= Capabilities.Renamable;

					var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Port));
					inputPort.portName = m_cInputName;
					inputContainer.Add(inputPort);

					var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(Port));
					outputPort.portName = m_cOutputName;
					outputContainer.Add(outputPort);
				}
			}
			public class BTEditSelectorNode : BTEditBaseNode
			{
				public BTEditSelectorNode(BehaviorTreeNodeView view) : base(view)
				{
					title = "Selector";
					capabilities -= Capabilities.Renamable;

					var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Port));
					inputPort.portName = m_cInputName;
					inputContainer.Add(inputPort);

					var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(Port));
					outputPort.portName = m_cOutputName;
					outputContainer.Add(outputPort);
				}
			}
			public class BTEditRandomSelectorNode : BTEditBaseNode
			{
				public BTEditRandomSelectorNode(BehaviorTreeNodeView view) : base(view)
				{
					title = "Random selector";
					capabilities -= Capabilities.Renamable;

					var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Port));
					inputPort.portName = m_cInputName;
					inputContainer.Add(inputPort);

					var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(Port));
					outputPort.portName = m_cOutputName;
					outputContainer.Add(outputPort);
				}
			}
			public class BTEditParallelNode : BTEditBaseNode
			{
				public BTEditParallelNode(BehaviorTreeNodeView view) : base(view)
				{
					title = "Parallel";
					capabilities -= Capabilities.Renamable;

					var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Port));
					inputPort.portName = m_cInputName;
					inputContainer.Add(inputPort);

					var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(Port));
					outputPort.portName = m_cOutputName;
					outputContainer.Add(outputPort);
				}
			}
			public class BTEditSimpleParallelNode : BTEditBaseNode
			{
				public BTEditSimpleParallelNode(BehaviorTreeNodeView view) : base(view)
				{
					title = "Simple parallel";
					capabilities -= Capabilities.Renamable;

					var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Port));
					inputPort.portName = m_cInputName;
					inputContainer.Add(inputPort);

					var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(Port));
					outputPort.portName = m_cOutputName;
					outputContainer.Add(outputPort);
				}
			}
		}
	}
}
