using System.Collections;
using System.Collections.Generic;
using AI.BehaviorTree.CashContainer.Detail;
using AI.BehaviorTree.Node.Detail;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		namespace Node
		{
			public class TaskNode : NotRootNode
			{
				struct TaskInfo
				{
					public TaskInfo(System.Type classType, string jsonData)
					{
						this.classType = classType;
						this.jsonData = jsonData;
					}

					public System.Type classType { get; private set; }
					public string jsonData { get; private set; }
				}


				public BaseTask task { get; private set; } = null;
				static Dictionary<string, TaskInfo> m_taskDataKeyGuid = new Dictionary<string, TaskInfo>();

				public override EnableResult OnEnable()
				{
					if (!isAllTrueDecorators)
						return EnableResult.Failed;

					aiAgent.RegisterTask(this);
					foreach (var e in services) e.OnEnable();

					return task.OnEnale();
				}

				public override void OnDisable(UpdateResult result)
				{
					aiAgent.UnregisterTask();
					task.OnQuit(result);
				}

				public override UpdateResult Update(AIAgent agent, Blackboard blackboard)
				{
					if (!isAllTrueDecorators)
						return UpdateResult.Failed;

					foreach (var e in services) e.Update(agent, blackboard);
					return task.Update();
				}

				public void FixedUpdate()
				{
					task.FixedUpdate();
				}

				public override void Load(BaseCashContainer container, Blackboard blackboard)
				{
					var cast = container as CashContainer.TaskCashContainer;
					LoadDecoratorAndService(cast);

					System.Type type = System.Type.GetType(cast.taskClassName);
					m_taskDataKeyGuid.Add(guid, new TaskInfo(type, cast.taskToJson));
					task = (BaseTask)JsonUtility.FromJson(cast.taskToJson, type);
				}

				public override BaseNode Clone(AIAgent agent, BehaviorTree behaviorTree)
				{
					TaskNode result = new TaskNode();
					result.CloneBase(behaviorTree, this);
					result.CloneDecoratorAndService(this);

					var taskInfo = m_taskDataKeyGuid[guid];
					result.task = (BaseTask)JsonUtility.FromJson(taskInfo.jsonData, taskInfo.classType);
					result.task.InitializeBase(behaviorTree, result.aiAgent);

					return result;
				}
			}
		}
	}
}