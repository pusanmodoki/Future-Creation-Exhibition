using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>MisoTempra editor</summary>
namespace LocalEditor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		/// <summary>Editor classes</summary>
		namespace NodeEditor
		{
			[CustomPropertyDrawer(typeof(AI.BehaviorTree.CashContainer.Detail.ServiceInfomations))]
			public class BTServiceDrawer : PropertyDrawer
			{
				public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
				{
					position.height = EditorGUIUtility.singleLineHeight;
					EditorGUI.LabelField(position, property.FindPropertyRelative("m_className").stringValue);
					position.position = position.position + new Vector2(0, EditorGUIUtility.singleLineHeight + 2);

					var useProperty = property.FindPropertyRelative("m_callInterval");
					useProperty.floatValue = EditorGUI.Slider(position, " >call interval", useProperty.floatValue, 0.0f, 10.0f);
				}
			}
		}
	}
}