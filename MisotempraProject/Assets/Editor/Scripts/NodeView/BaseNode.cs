using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>Node view(Graph view)</summary>
	namespace NodeView
	{
		public class BaseNode : Node
		{
			public BaseNode(string title)
			{
				this.title = title;

				var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Port));
				inputContainer.Add(inputPort);

				var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(Port));
				outputContainer.Add(outputPort);
			}
		}
	}
}