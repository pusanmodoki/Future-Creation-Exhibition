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

				BaseTask m_subsequentTask = null;

				public override EnableResult OnEnable()
				{
					if (!isAllTrueDecorators)
						return EnableResult.Failed;

					thisTree.RegisterTask(this);
					foreach (var e in services) e.OnEnable();

					return task.OnEnale();
				}

				public override void OnDisable(UpdateResult result)
				{
					if (result != UpdateResult.ForceReschedule)
					{
						m_subsequentTask?.OnQuit(result);
						thisTree.UnregisterTask();
					}
					else
					{
						if (m_subsequentTask == null)
							task.OnQuit(UpdateResult.ForceReschedule);
						else
							m_subsequentTask.OnQuit(UpdateResult.ForceReschedule);
					}

					m_subsequentTask = null;
				}

				public override UpdateResult Update(AIAgent agent, Blackboard blackboard)
				{
					if (!isAllTrueDecorators)
						return UpdateResult.Failed;
					if (m_subsequentTask != null)
						return m_subsequentTask.Update();

					foreach (var e in services) e.Update(agent, blackboard);

					var updateResult = task.Update();
					if (updateResult != UpdateResult.Run)
					{
						task.OnQuit(updateResult);

						if (task.subsequentTaskKey != null &&
							task.behaviorTree.subsequentTasks[task.subsequentTaskKey].OnEnale() == EnableResult.Success)
						{
							m_subsequentTask = task.behaviorTree.subsequentTasks[task.subsequentTaskKey];
							return UpdateResult.Run;
						}
						else return updateResult;
					}
					return updateResult;
				}

				public void FixedUpdate()
				{
					if (m_subsequentTask != null)
					{
						m_subsequentTask.FixedUpdate();
						return;
					}
					else task.FixedUpdate();
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