using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	[CustomEditor(typeof(AI.AIAgent))]
	public class AIAgentEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			EditorGUILayout.Space();
			if (GUILayout.Button("Open behavior file"))
			{
				bool isOpen = false;
				string name = serializedObject.FindProperty("m_fileName").FindPropertyRelative("m_fileName").stringValue;
				foreach (var instance in BehaviorTree.BehaviorTreeWindow.instances)
				{
					if (instance.fileName == name)
					{
						instance.Show();
						instance.Repaint();
						instance.Focus();
					}
				}
				if (!isOpen)
				{
					var window = BehaviorTree.BehaviorTreeWindow.NewOpen();
					window.nodeView.DoLoadCallback(name);
				}
			}
		}
	}
}