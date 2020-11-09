using System.Collections;
using UnityEngine;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		///<summary>Write editor classes</summary>
		namespace DecoratorScriptableObjects
		{
			public class BaseDecoratorScriptableObject : UnityEngine.ScriptableObject {}

			public class DecoratorScriptableObjectClassNameTestD1 : BaseDecoratorScriptableObject
			{
				public Test_Uemura.TestD1 decorator { get { return m_decorator; } }
				[SerializeField]
				Test_Uemura.TestD1 m_decorator = null;
				public void Initialize(Test_Uemura.TestD1 initialize) { m_decorator = initialize; }
			}
			public class DecoratorScriptableObjectClassNameTestD2 : BaseDecoratorScriptableObject
			{
				public Test_Uemura.TestD2 decorator { get { return m_decorator; } }
				[SerializeField]
				Test_Uemura.TestD2 m_decorator = null;
				public void Initialize(Test_Uemura.TestD2 initialize) { m_decorator = initialize; }
			}
			public class DecoratorScriptableObjectClassNameToPlayerDistance : BaseDecoratorScriptableObject
			{
				public AI.BehaviorTree.Decorator.ToPlayerDistance decorator { get { return m_decorator; } }
				[SerializeField]
				AI.BehaviorTree.Decorator.ToPlayerDistance m_decorator = null;
				public void Initialize(AI.BehaviorTree.Decorator.ToPlayerDistance initialize) { m_decorator = initialize; }
			}

			public static class DecoratorScriptableObjectClassMediator
			{
				public static void CreateEditorAndScriptableObject(AI.BehaviorTree.BaseDecorator decorator, 
					out UnityEditor.Editor editor, out UnityEngine.ScriptableObject scriptableObject, string decoratorTypeFullName)
				{
					if (decoratorTypeFullName == typeof(Test_Uemura.TestD1).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<DecoratorScriptableObjectClassNameTestD1>();
						(scriptableObject as DecoratorScriptableObjectClassNameTestD1).Initialize(decorator as Test_Uemura.TestD1);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as DecoratorScriptableObjectClassNameTestD1);
						return;
					}
					if (decoratorTypeFullName == typeof(Test_Uemura.TestD2).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<DecoratorScriptableObjectClassNameTestD2>();
						(scriptableObject as DecoratorScriptableObjectClassNameTestD2).Initialize(decorator as Test_Uemura.TestD2);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as DecoratorScriptableObjectClassNameTestD2);
						return;
					}
					if (decoratorTypeFullName == typeof(AI.BehaviorTree.Decorator.ToPlayerDistance).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<DecoratorScriptableObjectClassNameToPlayerDistance>();
						(scriptableObject as DecoratorScriptableObjectClassNameToPlayerDistance).Initialize(decorator as AI.BehaviorTree.Decorator.ToPlayerDistance);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as DecoratorScriptableObjectClassNameToPlayerDistance);
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