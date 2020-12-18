using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	[CustomPropertyDrawer(typeof(AI.AIPatrolPoints))]
	public class AIPatrolPointsDrawer : PropertyDrawer
	{
		SerializedProperty m_property = null;
		SerializedProperty m_points = null;
		SerializedProperty m_installationHeights = null;

		[SerializeField]
		bool m_isFoldout = false;
		[SerializeField]
		bool m_isFoldoutPoints = false;
		[SerializeField]
		List<bool> m_isFoldoutPointData = new List<bool>();

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float result = EditorGUIUtility.singleLineHeight;
			if (m_isFoldout)
			{
				result += EditorGUIUtility.singleLineHeight;
				result += EditorGUI.GetPropertyHeight(m_installationHeights);
				result += EditorGUIUtility.singleLineHeight;
				if (m_isFoldoutPoints)
				{
					result += EditorGUIUtility.singleLineHeight * m_isFoldoutPointData.Count;
					for (int i = 0; i < m_isFoldoutPointData.Count; ++i)
					{
						if (m_isFoldoutPointData[i])
						{
							result += EditorGUIUtility.singleLineHeight * 2.0f;
							result += EditorGUIUtility.singleLineHeight * 2.0f * m_points.GetArrayElementAtIndex(i).FindPropertyRelative("nextInfos").arraySize;
						}
					}
				}
			}
			return result;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (m_property == null)
			{
				m_property = property;
				m_points = property.FindPropertyRelative("m_points");
				m_installationHeights = property.FindPropertyRelative("m_installationHeights");
			}

			position.height = EditorGUIUtility.singleLineHeight;
			m_isFoldout = EditorGUI.Foldout(position, m_isFoldout, property.displayName, true);
			if (!m_isFoldout) return;

			position.y += EditorGUIUtility.singleLineHeight;
			position.x += 15.0f;
			position.width -= 15.0f;
			{
				int buf = EditorGUI.IntField(position, "Num points", m_points.arraySize);
				while (buf < m_points.arraySize)
					m_points.DeleteArrayElementAtIndex(m_points.arraySize - 1);
				while (buf > m_points.arraySize)
					m_points.InsertArrayElementAtIndex(m_points.arraySize);
			}

			position.y += EditorGUIUtility.singleLineHeight;
			position.height = EditorGUI.GetPropertyHeight(m_installationHeights);
			EditorGUI.PropertyField(position, m_installationHeights, true);

			position.y += position.height;
			position.height = EditorGUIUtility.singleLineHeight;
			m_isFoldoutPoints = EditorGUI.Foldout(position, m_isFoldoutPoints, "Points", true);
			if (!m_isFoldoutPoints) return;

			while (m_isFoldoutPointData.Count < m_points.arraySize)
				m_isFoldoutPointData.Add(false);
			while (m_isFoldoutPointData.Count > m_points.arraySize)
				m_isFoldoutPointData.RemoveAt(m_isFoldoutPointData.Count - 1);

			position.x += 15.0f;
			position.width -= 15.0f;
			for (int i = 0, lastIndex = m_points.arraySize - 1; i < m_points.arraySize; ++i)
			{
				position.y += EditorGUIUtility.singleLineHeight;
				m_isFoldoutPointData[i] = EditorGUI.Foldout(position, m_isFoldoutPointData[i], "Element " + i, true);
				if (!m_isFoldoutPointData[i]) continue;

				var data = m_points.GetArrayElementAtIndex(i);
				position.y += EditorGUIUtility.singleLineHeight;
				EditorGUI.ObjectField(position, data.FindPropertyRelative("transform"), new GUIContent("Point transform"));

				var nextInfo = data.FindPropertyRelative("nextInfos");
				for (int k = 0; k < nextInfo.arraySize; ++k)
				{
					var nextInfoElement = nextInfo.GetArrayElementAtIndex(k);
					var nextIndex = nextInfoElement.FindPropertyRelative("nextElementIndex");
					var probability = nextInfoElement.FindPropertyRelative("probability");
					position.y += EditorGUIUtility.singleLineHeight;
					{
						var rect = position;
						rect.x += EditorStyles.label.CalcSize(new GUIContent("Next point 0  ")).x;
						rect.width = position.width + position.x - rect.x;

						EditorGUI.LabelField(position, "Next point " + k);
						nextIndex.intValue = EditorGUI.IntSlider(rect, nextIndex.intValue, 0, lastIndex);
					}
					position.y += EditorGUIUtility.singleLineHeight;
					{
						Rect rect = position, rect2;
						rect.x += 10.0f;
						rect.width -= 10.0f;
						rect2 = rect;
						rect2.x += EditorStyles.label.CalcSize(new GUIContent("Probability  ")).x;
						rect2.width = rect.width + rect.x - rect2.x;

						EditorGUI.LabelField(rect, "Probability  ");
						probability.floatValue = EditorGUI.Slider(rect2, probability.floatValue, 0.0f, 1.0f);
					}
				}

				position.y += EditorGUIUtility.singleLineHeight;
				{
					Rect rect = position;
					rect.width = rect.width / 2.0f - 5.0f;
					if (GUI.Button(rect, "Add next"))
						nextInfo.InsertArrayElementAtIndex(nextInfo.arraySize);
					rect.x += rect.width + 10.0f;
					if (GUI.Button(rect, "Sub next"))
						nextInfo.DeleteArrayElementAtIndex(nextInfo.arraySize - 1);
				}
			}
		}
	}
}