using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>Input editor</summary>
	namespace Input
	{

		[System.Serializable]
		class InputCashContainer
		{
			public string[] enumNames { get { return m_enumNames; } }
			public string[] axisNames { get { return m_axisNames; } }
			public bool[] isEnableEnums { get { return m_isEnableEnums; } }

			public Dictionary<string, bool> isEnableAxes { get; private set; } = new Dictionary<string, bool>();

			public void FirstInitializeEnums()
			{
				m_enumNames = System.Enum.GetNames(typeof(KeyCode)); ;
				m_isEnableAxes = new bool[m_enumNames.Length];
				for (int i = 0; i < m_isEnableAxes.Length; ++i) m_isEnableAxes[i] = false;
			}
			public void ReloadAxes(SerializedProperty axes)
			{
				Dictionary<string, bool> isEnableAxesTemp = isEnableAxes;

				isEnableAxes = new Dictionary<string, bool>();
				m_axisNames = new string[axes.arraySize];
				m_isEnableAxes = new bool[axes.arraySize];

				for (int i = 0; i < axes.arraySize; ++i)
				{
					var property = axes.GetArrayElement(i);
					m_axisNames[i] = property.FindPropertyRelative("m_Name").stringValue;

					if (isEnableAxesTemp.ContainsKey(m_axisNames[i]))
					{
						m_isEnableAxes[i] = isEnableAxesTemp[m_axisNames[i]];
						isEnableAxes.Add(m_axisNames[i], m_isEnableAxes[i]);
					}
					else
					{
						m_isEnableAxes[i] = false;
						isEnableAxes.Add(m_axisNames[i], false);
					}
				}
			}

			public void BuildAxes()
			{
				if (m_axisNames == null) return;
				for (int i = 0; i < m_axisNames.Length; ++i)
					isEnableAxes.Add(m_axisNames[i], m_isEnableAxes[i]);
			}

			[SerializeField]
			string[] m_enumNames;
			[SerializeField]
			string[] m_axisNames;
			[SerializeField]
			bool[] m_isEnableEnums;
			[SerializeField]
			bool[] m_isEnableAxes;
		}
	}
}