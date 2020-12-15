using System.Collections;
using UnityEngine;

/// <summary>MisoTempra editor</summary>
namespace LocalEditor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		///<summary>Write editor classes</summary>
		namespace DecoratorScriptableObjects
		{
			public class BaseDecoratorScriptableObject : UnityEngine.ScriptableObject {}

			public class DecoratorScriptableObjectClassNameDeath_Decorator : BaseDecoratorScriptableObject
			{
				public Death_Decorator decorator { get { return m_decorator; } }
				[SerializeField]
				Death_Decorator m_decorator = null;
				public void Initialize(Death_Decorator initialize) { m_decorator = initialize; }
			}
			public class DecoratorScriptableObjectClassNamepractice2_Decorator : BaseDecoratorScriptableObject
			{
				public practice2_Decorator decorator { get { return m_decorator; } }
				[SerializeField]
				practice2_Decorator m_decorator = null;
				public void Initialize(practice2_Decorator initialize) { m_decorator = initialize; }
			}
			public class DecoratorScriptableObjectClassNamepractice4_decorator : BaseDecoratorScriptableObject
			{
				public practice4_decorator decorator { get { return m_decorator; } }
				[SerializeField]
				practice4_decorator m_decorator = null;
				public void Initialize(practice4_decorator initialize) { m_decorator = initialize; }
			}
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
			public class DecoratorScriptableObjectClassNameAlreadyPatrol : BaseDecoratorScriptableObject
			{
				public AI.BehaviorTree.Decorator.AlreadyPatrol decorator { get { return m_decorator; } }
				[SerializeField]
				AI.BehaviorTree.Decorator.AlreadyPatrol m_decorator = null;
				public void Initialize(AI.BehaviorTree.Decorator.AlreadyPatrol initialize) { m_decorator = initialize; }
			}
			public class DecoratorScriptableObjectClassNameDiscoverPlayer : BaseDecoratorScriptableObject
			{
				public AI.BehaviorTree.Decorator.DiscoverPlayer decorator { get { return m_decorator; } }
				[SerializeField]
				AI.BehaviorTree.Decorator.DiscoverPlayer m_decorator = null;
				public void Initialize(AI.BehaviorTree.Decorator.DiscoverPlayer initialize) { m_decorator = initialize; }
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
					if (decoratorTypeFullName == typeof(Death_Decorator).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<DecoratorScriptableObjectClassNameDeath_Decorator>();
						(scriptableObject as DecoratorScriptableObjectClassNameDeath_Decorator).Initialize(decorator as Death_Decorator);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as DecoratorScriptableObjectClassNameDeath_Decorator);
						return;
					}
					if (decoratorTypeFullName == typeof(practice2_Decorator).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<DecoratorScriptableObjectClassNamepractice2_Decorator>();
						(scriptableObject as DecoratorScriptableObjectClassNamepractice2_Decorator).Initialize(decorator as practice2_Decorator);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as DecoratorScriptableObjectClassNamepractice2_Decorator);
						return;
					}
					if (decoratorTypeFullName == typeof(practice4_decorator).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<DecoratorScriptableObjectClassNamepractice4_decorator>();
						(scriptableObject as DecoratorScriptableObjectClassNamepractice4_decorator).Initialize(decorator as practice4_decorator);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as DecoratorScriptableObjectClassNamepractice4_decorator);
						return;
					}
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
					if (decoratorTypeFullName == typeof(AI.BehaviorTree.Decorator.AlreadyPatrol).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<DecoratorScriptableObjectClassNameAlreadyPatrol>();
						(scriptableObject as DecoratorScriptableObjectClassNameAlreadyPatrol).Initialize(decorator as AI.BehaviorTree.Decorator.AlreadyPatrol);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as DecoratorScriptableObjectClassNameAlreadyPatrol);
						return;
					}
					if (decoratorTypeFullName == typeof(AI.BehaviorTree.Decorator.DiscoverPlayer).FullName)
					{
						scriptableObject = UnityEngine.ScriptableObject.CreateInstance<DecoratorScriptableObjectClassNameDiscoverPlayer>();
						(scriptableObject as DecoratorScriptableObjectClassNameDiscoverPlayer).Initialize(decorator as AI.BehaviorTree.Decorator.DiscoverPlayer);
						editor = UnityEditor.Editor.CreateEditor(scriptableObject as DecoratorScriptableObjectClassNameDiscoverPlayer);
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