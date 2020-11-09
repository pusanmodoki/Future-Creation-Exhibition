using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AI
{
	namespace BehaviorTree
	{
		namespace Detail
		{
			[System.Serializable]
			public class BehaviorFileName
			{
				public string fileName { get { return m_fileName; } }

				[SerializeField]
				string m_fileName = "";

				public static implicit operator string(BehaviorFileName name)
				{
					return name.fileName;
				}
			}
		}
	}
}
#if UNITY_EDITOR
/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		[UnityEditor.CustomPropertyDrawer(typeof(AI.BehaviorTree.Detail.BehaviorFileName))]
		class BehaviorFileNameDrawer : UnityEditor.PropertyDrawer
		{
			string[] m_selects = null;
			int m_selectIndex = 0;
			bool m_isInit = false;

			public BehaviorFileNameDrawer() : base()
			{
				List<string> temp = new List<string>();

				string[] path = System.IO.Directory.GetFiles(AI.BehaviorTree.BehaviorTree.dataSavePath);
				foreach (var e in path)
				{
					if (FileAccess.FileAccessor.IsExistMark(e, AI.BehaviorTree.BehaviorTree.cFileBeginMark))
					{
						int findBegin = Mathf.Max(e.LastIndexOf('\\'), e.LastIndexOf('/')) + 1;
						int findLast = e.LastIndexOf('.');
						string name = e.Substring(findBegin, findLast - findBegin);

						temp.Add(name);
					}
				}

				m_selects = temp.ToArray();
			}

			public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
			{
				if (!m_isInit)
				{
					m_isInit = true;
					var name = property.FindPropertyRelative("m_fileName").stringValue;
					for (int i = 0; i < m_selects.Length; ++i)
					{
						if (m_selects[i] == name)
						{
							m_selectIndex = i;
							break;
						}
					}
				}

				property.serializedObject.Update();
				m_selectIndex = EditorGUI.Popup(position, "Behavior file", m_selectIndex, m_selects);

				if (GUI.changed)
				{
					property.FindPropertyRelative("m_fileName").stringValue = m_selects[m_selectIndex];
					property.serializedObject.ApplyModifiedProperties();
				}
			}
		}
	}
}
#endif