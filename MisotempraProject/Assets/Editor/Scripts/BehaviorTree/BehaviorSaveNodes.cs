using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using AI.BehaviorTree;

	[System.Serializable]
public class BehaviorSaveNode
{
	[System.Serializable]
	public struct TaskNodeInfo
	{
		public TaskNodeInfo(string taskClassName)
		{
			m_taskClassName = taskClassName;
		}
		public string taskClassName { get { return m_taskClassName; } }

		[SerializeField]
		string m_taskClassName;
	}
	[System.Serializable]
	public struct CompositeNodeInfo
	{
		public CompositeNodeInfo(string className)
		{
			m_className = className;
			m_serviceClasses = new List<string>();
			m_childrenNodesGuid = new List<string>();
			m_finishMode = BehaviorBaseCompositeNode.ParallelFinishMode.Null;

			serviceClasses = new ReadOnlyCollection<string>(m_serviceClasses);
			childrenNodesGuid = new ReadOnlyCollection<string>(m_childrenNodesGuid);
		}
		public CompositeNodeInfo(string className, BehaviorBaseCompositeNode.ParallelFinishMode finishMode)
		{
			m_className = className;
			m_serviceClasses = new List<string>();
			m_childrenNodesGuid = new List<string>();
			m_finishMode = finishMode;

			serviceClasses = new ReadOnlyCollection<string>(m_serviceClasses);
			childrenNodesGuid = new ReadOnlyCollection<string>(m_childrenNodesGuid);
		}

		public string className { get { return m_className; } }
		public BehaviorBaseCompositeNode.ParallelFinishMode finishMode { get { return m_finishMode; } }
		public ReadOnlyCollection<string> serviceClasses { get; private set; }
		public ReadOnlyCollection<string> childrenNodesGuid { get; private set; }

		[SerializeField]
		string m_className;
		[SerializeField]
		List<string> m_serviceClasses;
		[SerializeField]
		List<string> m_childrenNodesGuid;
		[SerializeField]
		BehaviorBaseCompositeNode.ParallelFinishMode m_finishMode;
	}

	public BehaviorSaveNode(string name, string guid, Rect windowRect, TaskNodeInfo taskNodeInfo)
	{
		m_name = name;
		m_guid = guid;
		m_decoratorClasses = new List<string>();
		m_windowRect = windowRect;
		m_isTaskNode = true;
		m_taskNodeInfo = taskNodeInfo;
		m_compositeNodeInfo = default;
		decoratorClasses = new ReadOnlyCollection<string>(m_decoratorClasses);
	}
	public BehaviorSaveNode(string name, string guid, Rect windowRect, CompositeNodeInfo compositeNodeInfo)
	{
		m_name = name;
		m_guid = guid;
		m_decoratorClasses = new List<string>();
		m_windowRect = windowRect;
		m_isTaskNode = false;
		m_taskNodeInfo = default;
		m_compositeNodeInfo = compositeNodeInfo;
		decoratorClasses = new ReadOnlyCollection<string>(m_decoratorClasses);
	}


	public string name { get { return m_name; } }
	public string guid { get { return m_guid; } }
	public Rect windowRect { get { return m_windowRect; } }
	public bool isTaskNode { get { return m_isTaskNode; } }
	public TaskNodeInfo taskNodeInfo { get { return m_taskNodeInfo; } }
	public CompositeNodeInfo compositeNodeInfo { get { return m_compositeNodeInfo; } }
	public ReadOnlyCollection<string> decoratorClasses { get; private set; } = null;

	[SerializeField]
	string m_name = "";
	[SerializeField]
	string m_guid = "";
	[SerializeField]
	List<string> m_decoratorClasses = new List<string>();
	[SerializeField]
	Rect m_windowRect = Rect.zero;
	[SerializeField]
	bool m_isTaskNode = false;
	[SerializeField]
	TaskNodeInfo m_taskNodeInfo = default;
	[SerializeField]
	CompositeNodeInfo m_compositeNodeInfo = default;

	public void SetWindowRect(Rect set) { m_windowRect = set; }
}
