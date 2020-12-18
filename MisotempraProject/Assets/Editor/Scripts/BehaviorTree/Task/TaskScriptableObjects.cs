using System.Collections;
using UnityEngine;

/// <summary>MisoTempra editor</summary>
namespace LocalEditor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		///<summary>Write editor classes</summary>
		namespace TaskScriptableObjects
		{
			public class BaseTaskScriptableObject : UnityEngine.ScriptableObject {}

			public class TaskScriptableObjectClassNameAttack : BaseTaskScriptableObject
			{
				public Attack task { get { return m_task; } }
				[SerializeField]
				Attack m_task = null;
				public void Initialize(Attack initialize) { m_task = initialize; }
			}
			public class TaskScriptableObjectClassNameMoveTo : BaseTaskScriptableObject
			{
				public MoveTo task { get { return m_task; } }
				[SerializeField]
				MoveTo m_task = null;
				public void Initialize(MoveTo initialize) { m_task = initialize; }
			}
			public class TaskScriptableObjectClassNamedeath : BaseTaskScriptableObject
			{
				public death task { get { return m_task; } }
				[SerializeField]
				death m_task = null;
				public void Initialize(death initialize) { m_task = initialize; }
			}
			public class TaskScriptableObjectClassNamepractice3 : BaseTaskScriptableObject
			{
				public practice3 task { get { return m_task; } }
				[SerializeField]
				practice3 m_task = null;
				public void Initialize(practice3 initialize) { m_task = initialize; }
			}
			public class TaskScriptableObjectClassNamepractice4 : BaseTaskScriptableObject
			{
				public practice4 task { get { return m_task; } }
				[SerializeField]
				practice4 m_task = null;
				public void Initialize(practice4 initialize) { m_task = initialize; }
			}
			public class TaskScriptableObjectClassNamepractice5 : BaseTaskScriptableObject
			{
				public practice5 task { get { return m_task; } }
				[SerializeField]
				practice5 m_task = null;
				public void Initialize(practice5 initialize) { m_task = initialize; }
			}
			public class TaskScriptableObjectClassNamesubsepuent : BaseTaskScriptableObject
			{
				public subsepuent task { get { return m_task; } }
				[SerializeField]
				subsepuent m_task = null;
				public void Initialize(subsepuent initialize) { m_task = initialize; }
			}
			public class TaskScriptableObjectClassNametackle : BaseTaskScriptableObject
			{
				public tackle task { get { return m_task; } }
				[SerializeField]
				tackle m_task = null;
				public void Initialize(tackle initialize) { m_task = initialize; }
			}
			public class TaskScriptableObjectClassNametest1 : BaseTaskScriptableObject
			{
				public test1 task { get { return m_task; } }
				[SerializeField]
				test1 m_task = null;
				public void Initialize(test1 initialize) { m_task = initialize; }
			}
			public class TaskScriptableObjectClassNamewark : BaseTaskScriptableObject
			{
				public wark task { get { return m_task; } }
				[SerializeField]
				wark m_task = null;
				public void Initialize(wark initialize) { m_task = initialize; }
			}
			public class TaskScriptableObjectClassNameTestTask : BaseTaskScriptableObject
			{
				public TestTask task { get { return m_task; } }
				[SerializeField]
				TestTask m_task = null;
				public void Initialize(TestTask initialize) { m_task = initialize; }
			}
			public class TaskScriptableObjectClassNameDeath : BaseTaskScriptableObject
			{
				public AI.BehaviorTree.Task.Death task { get { return m_task; } }
				[SerializeField]
				AI.BehaviorTree.Task.Death m_task = null;
				public void Initialize(AI.BehaviorTree.Task.Death initialize) { m_task = initialize; }
			}
			public class TaskScriptableObjectClassNameMoveTo0 : BaseTaskScriptableObject
			{
				public AI.BehaviorTree.Task.MoveTo task { get { return m_task; } }
				[SerializeField]
				AI.BehaviorTree.Task.MoveTo m_task = null;
				public void Initialize(AI.BehaviorTree.Task.MoveTo initialize) { m_task = initialize; }
			}
			public class TaskScriptableObjectClassNamePatrolMove : BaseTaskScriptableObject
			{
				public AI.BehaviorTree.Task.PatrolMove task { get { return m_task; } }
				[SerializeField]
				AI.BehaviorTree.Task.PatrolMove m_task = null;
				public void Initialize(AI.BehaviorTree.Task.PatrolMove initialize) { m_task = initialize; }
			}
			public class TaskScriptableObjectClassNameToPatrolPoint : BaseTaskScriptableObject
			{
				public AI.BehaviorTree.Task.ToPatrolPoint task { get { return m_task; } }
				[SerializeField]
				AI.BehaviorTree.Task.ToPatrolPoint m_task = null;
				public void Initialize(AI.BehaviorTree.Task.ToPatrolPoint initialize) { m_task = initialize; }
			}
			public class TaskScriptableObjectClassNameAttack01 : BaseTaskScriptableObject
			{
				public AI.BehaviorTree.Task.Physics.Attack01 task { get { return m_task; } }
				[SerializeField]
				AI.BehaviorTree.Task.Physics.Attack01 m_task = null;
				public void Initialize(AI.BehaviorTree.Task.Physics.Attack01 initialize) { m_task = initialize; }
			}

			public static class TaskScriptableObjectClassMediator
			{
				public static void CreateEditorAndScriptableObject(AI.BehaviorTree.BaseTask task, 
					out UnityEditor.Editor editor, out UnityEngine.ScriptableObject scriptableObject, string taskTypeFullName)
				{
					if (taskTypeFullName == typeof(Attack).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<TaskScriptableObjectClassNameAttack>();
						(scriptableObject as TaskScriptableObjectClassNameAttack).Initialize(task as Attack);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as TaskScriptableObjectClassNameAttack);
						return;
					}
					if (taskTypeFullName == typeof(MoveTo).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<TaskScriptableObjectClassNameMoveTo>();
						(scriptableObject as TaskScriptableObjectClassNameMoveTo).Initialize(task as MoveTo);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as TaskScriptableObjectClassNameMoveTo);
						return;
					}
					if (taskTypeFullName == typeof(death).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<TaskScriptableObjectClassNamedeath>();
						(scriptableObject as TaskScriptableObjectClassNamedeath).Initialize(task as death);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as TaskScriptableObjectClassNamedeath);
						return;
					}
					if (taskTypeFullName == typeof(practice3).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<TaskScriptableObjectClassNamepractice3>();
						(scriptableObject as TaskScriptableObjectClassNamepractice3).Initialize(task as practice3);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as TaskScriptableObjectClassNamepractice3);
						return;
					}
					if (taskTypeFullName == typeof(practice4).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<TaskScriptableObjectClassNamepractice4>();
						(scriptableObject as TaskScriptableObjectClassNamepractice4).Initialize(task as practice4);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as TaskScriptableObjectClassNamepractice4);
						return;
					}
					if (taskTypeFullName == typeof(practice5).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<TaskScriptableObjectClassNamepractice5>();
						(scriptableObject as TaskScriptableObjectClassNamepractice5).Initialize(task as practice5);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as TaskScriptableObjectClassNamepractice5);
						return;
					}
					if (taskTypeFullName == typeof(subsepuent).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<TaskScriptableObjectClassNamesubsepuent>();
						(scriptableObject as TaskScriptableObjectClassNamesubsepuent).Initialize(task as subsepuent);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as TaskScriptableObjectClassNamesubsepuent);
						return;
					}
					if (taskTypeFullName == typeof(tackle).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<TaskScriptableObjectClassNametackle>();
						(scriptableObject as TaskScriptableObjectClassNametackle).Initialize(task as tackle);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as TaskScriptableObjectClassNametackle);
						return;
					}
					if (taskTypeFullName == typeof(test1).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<TaskScriptableObjectClassNametest1>();
						(scriptableObject as TaskScriptableObjectClassNametest1).Initialize(task as test1);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as TaskScriptableObjectClassNametest1);
						return;
					}
					if (taskTypeFullName == typeof(wark).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<TaskScriptableObjectClassNamewark>();
						(scriptableObject as TaskScriptableObjectClassNamewark).Initialize(task as wark);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as TaskScriptableObjectClassNamewark);
						return;
					}
					if (taskTypeFullName == typeof(TestTask).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<TaskScriptableObjectClassNameTestTask>();
						(scriptableObject as TaskScriptableObjectClassNameTestTask).Initialize(task as TestTask);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as TaskScriptableObjectClassNameTestTask);
						return;
					}
					if (taskTypeFullName == typeof(AI.BehaviorTree.Task.Death).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<TaskScriptableObjectClassNameDeath>();
						(scriptableObject as TaskScriptableObjectClassNameDeath).Initialize(task as AI.BehaviorTree.Task.Death);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as TaskScriptableObjectClassNameDeath);
						return;
					}
					if (taskTypeFullName == typeof(AI.BehaviorTree.Task.MoveTo).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<TaskScriptableObjectClassNameMoveTo0>();
						(scriptableObject as TaskScriptableObjectClassNameMoveTo0).Initialize(task as AI.BehaviorTree.Task.MoveTo);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as TaskScriptableObjectClassNameMoveTo0);
						return;
					}
					if (taskTypeFullName == typeof(AI.BehaviorTree.Task.PatrolMove).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<TaskScriptableObjectClassNamePatrolMove>();
						(scriptableObject as TaskScriptableObjectClassNamePatrolMove).Initialize(task as AI.BehaviorTree.Task.PatrolMove);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as TaskScriptableObjectClassNamePatrolMove);
						return;
					}
					if (taskTypeFullName == typeof(AI.BehaviorTree.Task.ToPatrolPoint).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<TaskScriptableObjectClassNameToPatrolPoint>();
						(scriptableObject as TaskScriptableObjectClassNameToPatrolPoint).Initialize(task as AI.BehaviorTree.Task.ToPatrolPoint);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as TaskScriptableObjectClassNameToPatrolPoint);
						return;
					}
					if (taskTypeFullName == typeof(AI.BehaviorTree.Task.Physics.Attack01).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<TaskScriptableObjectClassNameAttack01>();
						(scriptableObject as TaskScriptableObjectClassNameAttack01).Initialize(task as AI.BehaviorTree.Task.Physics.Attack01);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as TaskScriptableObjectClassNameAttack01);
						return;
					}
					scriptableObject = null;
					editor = null;
					return;
				}
			}
		}
	}
}