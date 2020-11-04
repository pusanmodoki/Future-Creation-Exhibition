using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		/// <summary>Editor classes</summary>
		namespace NodeEditor
		{
			[CustomEditor(typeof(ScriptableObject.BTCompositeScriptableObject))]
			public class BTCompositeNodeEditor : BTBaseNodeEditor
			{
				public override void OnInspectorGUI()
				{
					serializedObject.Update();

					functions.DrawMemo();
					functions.DrawDecorators();
					functions.DrawServices();
					functions.DrawChildrensList();

					serializedObject.ApplyModifiedProperties();
				}
			}
			[CustomEditor(typeof(ScriptableObject.BTParallelScriptableObject))]
			public class BTParallelNodeEditor : BTBaseNodeEditor
			{
				public override void OnInspectorGUI()
				{
					serializedObject.Update();

					functions.DrawMemo();
					functions.DrawFinishMode();
					functions.DrawDecorators();
					functions.DrawServices();

					serializedObject.ApplyModifiedProperties();
				}
			}
			[CustomEditor(typeof(ScriptableObject.BTRandomScriptableObject))]
			public class BTRandomNodeEditor : BTBaseNodeEditor
			{
				public override void OnInspectorGUI()
				{
					serializedObject.Update();

					functions.DrawMemo();
					functions.DrawDecorators();
					functions.DrawServices();
					functions.DrawProbabilitys();

					serializedObject.ApplyModifiedProperties();
				}
			}
			[CustomEditor(typeof(ScriptableObject.BTTaskScriptableObject))]
			public class BTTaskNodeEditor : BTBaseNodeEditor
			{
				public override void OnInspectorGUI()
				{
					serializedObject.Update();

					functions.DrawMemo();
					functions.DrawDecorators();
					functions.DrawServices();
					functions.DrawTask();

					serializedObject.ApplyModifiedProperties();
				}
			}
		}
	}
}