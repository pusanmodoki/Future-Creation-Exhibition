using System.Collections;
using UnityEngine;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		///<summary>Write editor classes</summary>
		namespace ServiceScriptableObjects
		{
			public class BaseServiceScriptableObject : UnityEngine.ScriptableObject {}

			public class ServiceScriptableObjectClassNameTestS1 : BaseServiceScriptableObject
			{
				public Test_Uemura.TestS1 service { get { return m_service; } }
				[SerializeField]
				Test_Uemura.TestS1 m_service = null;
				public void Initialize(Test_Uemura.TestS1 initialize) { m_service = initialize; }
			}
			public class ServiceScriptableObjectClassNameTestS2 : BaseServiceScriptableObject
			{
				public Test_Uemura.TestS2 service { get { return m_service; } }
				[SerializeField]
				Test_Uemura.TestS2 m_service = null;
				public void Initialize(Test_Uemura.TestS2 initialize) { m_service = initialize; }
			}

			public static class ServiceScriptableObjectClassMediator
			{
				public static void CreateEditorAndScriptableObject(AI.BehaviorTree.BaseService service, 
					out UnityEditor.Editor editor, out UnityEngine.ScriptableObject scriptableObject, string serviceTypeFullName)
				{
					if (serviceTypeFullName == typeof(Test_Uemura.TestS1).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<ServiceScriptableObjectClassNameTestS1>();
						(scriptableObject as ServiceScriptableObjectClassNameTestS1).Initialize(service as Test_Uemura.TestS1);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as ServiceScriptableObjectClassNameTestS1);
						return;
					}
					if (serviceTypeFullName == typeof(Test_Uemura.TestS2).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<ServiceScriptableObjectClassNameTestS2>();
						(scriptableObject as ServiceScriptableObjectClassNameTestS2).Initialize(service as Test_Uemura.TestS2);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as ServiceScriptableObjectClassNameTestS2);
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