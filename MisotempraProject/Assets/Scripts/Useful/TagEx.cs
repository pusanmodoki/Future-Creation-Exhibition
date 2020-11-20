using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public struct TagEx
{
	public string tag { get { return m_tag; } }

	[SerializeField, Tooltip("Tag")]
	string m_tag;

	public TagEx(string tag)
	{
		m_tag = tag;
	}

	/// <summary>
	/// operator to string
	/// </summary>
	public static implicit operator string(TagEx tag)
	{
		return tag.m_tag;
	}
	/// <summary>
	/// operator to TagEx
	/// </summary>
	public static implicit operator TagEx(string tag)
	{
		return new TagEx(tag);
	}
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(TagEx))]
public class TagExDrawer : PropertyDrawer
{
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("m_tag"), label);
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		property.FindPropertyRelative("m_tag").stringValue = 
			EditorGUI.TagField(position, label, property.FindPropertyRelative("m_tag").stringValue);
	}
}
#endif
