using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TypeName
{
	public string typeName { get { return m_typeName; } }

	[SerializeField]
	string m_typeName;

	public static implicit operator string(TypeName typeName) { return typeName.m_typeName; }
}
