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
			[CustomEditor(typeof(ScriptableObject.BTRootScriptableObject))]
			public class BTBaseNodeEditor : UnityEditor.Editor
			{
				public override void OnInspectorGUI()
				{
					base.OnInspectorGUI();
					GUILayout.Button("SS");
				}
			}

		}
	}
}