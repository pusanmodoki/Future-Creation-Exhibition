using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using CashContainer = AI.BehaviorTree.CashContainer;
using Composite = AI.BehaviorTree.Node.Composite;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		public static class BTClassMediator
		{
			public enum NodeType
			{
				Sequence,
				Selector,
				RandomSelector,
				Parallel,
				SimpleParallel,
				Task
			}

			public static readonly string cTaskNodeName = "Task";
			public static readonly string cRootNodeName = "Root";

			public static string[] compositeNodeNames
			{
				get
				{
					if (m_compositeNodeNames == null)
					{
						m_compositeNodeNames = new string[5];
						m_compositeNodeNames[0] = "Sequence";
						m_compositeNodeNames[1] = "Selector";
						m_compositeNodeNames[2] = "Random selector";
						m_compositeNodeNames[3] = "Parallel";
						m_compositeNodeNames[4] = "Simple parallel";
					}
					return m_compositeNodeNames;
				}
			}
			static string[] m_compositeNodeNames = null;

			public static Dictionary<string, System.Type> cashTypes
			{
				get
				{
					if (m_cashTypes == null)
					{
						m_cashTypes = new Dictionary<string, System.Type>();
						m_cashTypes.Add(compositeNodeNames[0], typeof(CashContainer.CompositeCashContainer));
						m_cashTypes.Add(compositeNodeNames[1], typeof(CashContainer.CompositeCashContainer));
						m_cashTypes.Add(compositeNodeNames[2], typeof(CashContainer.RandomCashContainer));
						m_cashTypes.Add(compositeNodeNames[3], typeof(CashContainer.ParallelCashContainer));
						m_cashTypes.Add(compositeNodeNames[4], typeof(CashContainer.ParallelCashContainer));
						m_cashTypes.Add(cTaskNodeName, typeof(CashContainer.TaskCashContainer));
					}
					return m_cashTypes;
				}
			}
			static Dictionary<string, System.Type> m_cashTypes = null;

			public static Dictionary<string, System.Type> nodeTypes
			{
				get
				{
					if (m_nodeTypes == null)
					{
						m_nodeTypes = new Dictionary<string, System.Type>();
						m_nodeTypes.Add(compositeNodeNames[0], typeof(EditNode.BTEditSequenceNode));
						m_nodeTypes.Add(compositeNodeNames[1], typeof(EditNode.BTEditSelectorNode));
						m_nodeTypes.Add(compositeNodeNames[2], typeof(EditNode.BTEditRandomSelectorNode));
						m_nodeTypes.Add(compositeNodeNames[3], typeof(EditNode.BTEditParallelNode));
						m_nodeTypes.Add(compositeNodeNames[4], typeof(EditNode.BTEditSimpleParallelNode));
						m_nodeTypes.Add(cTaskNodeName, typeof(EditNode.BTEditTaskNode));
					}
					return m_nodeTypes;
				}
			}
			static Dictionary<string, System.Type> m_nodeTypes = null;

			public static Dictionary<string, System.Type> classTypes
			{
				get
				{
					if (m_classTypes == null)
					{
						m_classTypes = new Dictionary<string, System.Type>();
						m_classTypes.Add(compositeNodeNames[0], typeof(Composite.SequenceNode));
						m_classTypes.Add(compositeNodeNames[1], typeof(Composite.SelectorNode));
						m_classTypes.Add(compositeNodeNames[2], typeof(Composite.RandomSelectorNode));
						m_classTypes.Add(compositeNodeNames[3], typeof(Composite.ParallelNode));
						m_classTypes.Add(compositeNodeNames[4], typeof(Composite.SimpleParallelNode));
						m_classTypes.Add(cTaskNodeName, typeof(AI.BehaviorTree.BaseTask));
					}
					return m_classTypes;
				}
			}
			static Dictionary<string, System.Type> m_classTypes = null;

			public static Dictionary<string, System.Type> scriptableObjects
			{
				get
				{
					if (m_scriptableObjects == null)
					{
						m_scriptableObjects = new Dictionary<string, System.Type>();
						m_scriptableObjects.Add(compositeNodeNames[0], typeof(ScriptableObject.BTCompositeScriptableObject));
						m_scriptableObjects.Add(compositeNodeNames[1], typeof(ScriptableObject.BTCompositeScriptableObject));
						m_scriptableObjects.Add(compositeNodeNames[2], typeof(ScriptableObject.BTRandomScriptableObject));
						m_scriptableObjects.Add(compositeNodeNames[3], typeof(ScriptableObject.BTParallelScriptableObject));
						m_scriptableObjects.Add(compositeNodeNames[4], typeof(ScriptableObject.BTParallelScriptableObject));
						m_scriptableObjects.Add(cTaskNodeName, typeof(ScriptableObject.BTTaskScriptableObject));
						m_scriptableObjects.Add(cRootNodeName, typeof(ScriptableObject.BTRootScriptableObject));
					}
					return m_scriptableObjects;
				}
			}
			static Dictionary<string, System.Type> m_scriptableObjects = null;

			public static UnityEditor.Editor CreateEditor(BehaviorTreeNodeView view, Node node, ScriptableObject.Detail.BTBaseScriptableObject scriptableObject)
			{
				UnityEditor.Editor result = null;
				string title = node.userData as string;

				if (title == cRootNodeName)
					result = UnityEditor.Editor.CreateEditor(scriptableObject as ScriptableObject.BTRootScriptableObject);
				else if (title == cTaskNodeName)
					result = UnityEditor.Editor.CreateEditor(scriptableObject as ScriptableObject.BTTaskScriptableObject);
				else if (title == m_compositeNodeNames[0] || title == m_compositeNodeNames[1])
					result = UnityEditor.Editor.CreateEditor(scriptableObject as ScriptableObject.BTCompositeScriptableObject);
				else if (title == m_compositeNodeNames[2])
					result = UnityEditor.Editor.CreateEditor(scriptableObject as ScriptableObject.BTRandomScriptableObject);
				else if (title == m_compositeNodeNames[3] || title == m_compositeNodeNames[4])
					result = UnityEditor.Editor.CreateEditor(scriptableObject as ScriptableObject.BTParallelScriptableObject);
				else return null;

				if (result != null)
					(result as NodeEditor.BTBaseNodeEditor).Initialize(view);

				return result;
			}
		}
	}
}