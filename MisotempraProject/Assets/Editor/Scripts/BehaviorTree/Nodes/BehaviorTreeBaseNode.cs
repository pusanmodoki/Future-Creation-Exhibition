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
		public class BehaviorTreeBaseNode : NodeView.BaseNode
		{
			public BehaviorTreeBaseNode() : base("Sample")
			{
			}
		}
	}
}