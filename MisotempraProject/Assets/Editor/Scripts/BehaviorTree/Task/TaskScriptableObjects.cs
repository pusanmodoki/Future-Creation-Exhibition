using System.Collections;
using UnityEngine;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		///<summary>Write editor classes</summary>
		namespace TaskScriptableObjects
		{
			public class BaseTaskScriptableObject : UnityEngine.ScriptableObject {}

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

			public static class TaskScriptableObjectClassMediator
			{
				public static void CreateEditorAndScriptableObject(AI.BehaviorTree.BaseTask task, 
					out UnityEditor.Editor editor, out UnityEngine.ScriptableObject scriptableObject, string taskTypeFullName)
				{
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
					scriptableObject = null;
					editor = null;
					return;
				}
			}
		}
	}
}