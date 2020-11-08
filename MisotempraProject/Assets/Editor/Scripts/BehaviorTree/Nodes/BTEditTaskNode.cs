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
			public class BTEditTaskNode : BTEditBaseNode
			{
				public BTEditTaskNode(BehaviorTreeNodeView view) : base(view)
				{
					title = "Task";
					capabilities -= Capabilities.Renamable;

					var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Port));
					inputPort.portName = "In";
					inputContainer.Add(inputPort);
				}
			}
		}
	}
}