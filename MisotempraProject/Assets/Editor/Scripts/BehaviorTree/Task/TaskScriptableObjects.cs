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

			public class TaskScriptableObjectClassNameTestTask : BaseTaskScriptableObject
			{
				public TestTask task { get { return m_task; } }
				[SerializeField]
				TestTask m_task = null;
				public void Initialize(TestTask initialize) { m_task = initialize; }
			}
			public class TaskScriptableObjectClassNametest1 : BaseTaskScriptableObject
			{
				public test1 task { get { return m_task; } }
				[SerializeField]
				test1 m_task = null;
				public void Initialize(test1 initialize) { m_task = initialize; }
			}

			public static class TaskScriptableObjectClassMediator
			{
				public static void CreateEditorAndScriptableObject(AI.BehaviorTree.BaseTask task, 
					out UnityEditor.Editor editor, out UnityEngine.ScriptableObject scriptableObject, string taskTypeFullName)
				{
					if (taskTypeFullName == typeof(TestTask).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<TaskScriptableObjectClassNameTestTask>();
						(scriptableObject as TaskScriptableObjectClassNameTestTask).Initialize(task as TestTask);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as TaskScriptableObjectClassNameTestTask);
						return;
					}
					if (taskTypeFullName == typeof(test1).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<TaskScriptableObjectClassNametest1>();
						(scriptableObject as TaskScriptableObjectClassNametest1).Initialize(task as test1);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as TaskScriptableObjectClassNametest1);
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