using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using AI.BehaviorTree;

namespace AI
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		namespace CashContainer
		{
			namespace Detail
			{
				[System.Serializable]
				public class BaseCashContainer
				{
					public string nodeName { get { return m_nodeName; } set { m_nodeName = value; } }
					public string className { get { return m_className; } }
					public string editNodeClassName { get { return m_editNodeClassName; } }
					public string guid { get { return m_guid; } }
					public List<string> decoratorClasses { get { return m_decoratorClasses; } }
					public Vector2 position { get { return m_position; } set { m_position = value; } }

					public virtual bool isSaveReady { get { return false; } }

					public void Initialize(string nodeName, string className, string editNodeClassName, Vector2 position)
					{
						m_nodeName = nodeName;
						m_className = className;
						m_editNodeClassName = editNodeClassName;
						m_guid = System.Guid.NewGuid().ToString();
						m_position = position;
					}

					[SerializeField]
					string m_nodeName = "";
					[SerializeField]
					string m_className = "";
					[SerializeField]
					string m_editNodeClassName = "";
					[SerializeField]
					string m_guid = "";
					[SerializeField]
					Vector2 m_position = Vector2.zero;
					[SerializeField]
					protected List<string> m_decoratorClasses = new List<string>();
				}
			}
			[System.Serializable]
			public class RootCashContainer : Detail.BaseCashContainer
			{
				public List<string> childrenNodesGuid { get { return m_childrenNodesGuid; } }
				public string parentGuid { get { return m_parentGuid; } set { m_parentGuid = value; } }
				
				public override bool isSaveReady { get { return true; } }

				[SerializeField]
				string m_parentGuid = "";
				[SerializeField]
				List<string> m_childrenNodesGuid = new List<string>();
			}

			[System.Serializable]
			public abstract class NotRootCashContainer : Detail.BaseCashContainer
			{
				public string parentGuid { get { return m_parentGuid; } set { m_parentGuid = value; } }

				public override bool isSaveReady { get { return m_parentGuid != null && m_parentGuid.Length > 0; } }
				
				[SerializeField]
				string m_parentGuid = "";
			}

			[System.Serializable]
			public class TaskCashContainer : NotRootCashContainer
			{
				public string taskClassName { get { return m_taskClassName; } set { m_taskClassName = value; } }
				public BaseTask task { get { return m_task; } set { m_task = value; } }
				
				[SerializeField]
				string m_taskClassName = "";
				[SerializeField]
				BaseTask m_task = null;
			}

			[System.Serializable]
			public class CompositeCashContainer : NotRootCashContainer
			{
				public List<string> serviceClasses { get { return m_serviceClasses; } }
				public List<string> childrenNodesGuid { get { return m_childrenNodesGuid; } }
				
				[SerializeField]
				List<string> m_serviceClasses = new List<string>();
				[SerializeField]
				List<string> m_childrenNodesGuid = new List<string>();
			}

			[System.Serializable]
			public class ParallelCashContainer : CompositeCashContainer
			{
				public BehaviorBaseCompositeNode.ParallelFinishMode finishMode { get { return m_finishMode; } set { m_finishMode = value; } }
				
				[SerializeField]
				BehaviorBaseCompositeNode.ParallelFinishMode m_finishMode = BehaviorBaseCompositeNode.ParallelFinishMode.Null;
			}

			[System.Serializable]
			public class RandomCashContainer : CompositeCashContainer
			{
				public List<float> probabilitys { get { return m_probabilitys; } }
				
				[SerializeField]
				List<float> m_probabilitys = new List<float>();
			}
		}
	}
}