using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Input Management</summary>
namespace InputManagement
{
	[System.Flags]
	public enum AxisMode
	{
		Button = 1,
		Axis = 2,
		AxisRaw = 4,
	}
	[System.Serializable]
	public class InputCashContainer
	{
		public string[] enumNames { get { return m_enumNames; } }
		public string[] axisNames { get { return m_axisNames; } }
		public bool[] isEnableEnums { get { return m_isEnableEnums; } }
		public Dictionary<string, AxisMode> axisModes { get; private set; } = new Dictionary<string, AxisMode>();
		public Dictionary<string, bool> isEnableAxes { get; private set; } = new Dictionary<string, bool>();

		public bool[] usePlayIsEnableAxes { get { return m_isEnableAxes; } }
		public AxisMode[] usePlayAxisModes { get { return m_axisModes; } }

		public int[] joystickIndexes
		{
			get
			{
				if (m_joystickIndexes == null || m_joystickIndexes.Length == 0)
				{
					m_joystickIndexes = new int[9];
					m_joystickIndexes[0] = System.Array.IndexOf(m_enumNames, "JoystickButton0");
					m_joystickIndexes[1] = System.Array.IndexOf(m_enumNames, "Joystick1Button0");
					m_joystickIndexes[2] = System.Array.IndexOf(m_enumNames, "Joystick2Button0");
					m_joystickIndexes[3] = System.Array.IndexOf(m_enumNames, "Joystick3Button0");
					m_joystickIndexes[4] = System.Array.IndexOf(m_enumNames, "Joystick4Button0");
					m_joystickIndexes[5] = System.Array.IndexOf(m_enumNames, "Joystick5Button0");
					m_joystickIndexes[6] = System.Array.IndexOf(m_enumNames, "Joystick6Button0");
					m_joystickIndexes[7] = System.Array.IndexOf(m_enumNames, "Joystick7Button0");
					m_joystickIndexes[8] = System.Array.IndexOf(m_enumNames, "Joystick8Button0");
				}
				return m_joystickIndexes;
			}
		}

		[SerializeField]
		string[] m_enumNames = null;
		[SerializeField]
		string[] m_axisNames = null;
		[SerializeField]
		bool[] m_isEnableEnums = null;
		[SerializeField]
		bool[] m_isEnableAxes = null;
		[SerializeField]
		AxisMode[] m_axisModes = null; 
		
		int[] m_joystickIndexes = null;

#if UNITY_EDITOR
		public void EditFirstInitializeEnums()
		{
			m_enumNames = System.Enum.GetNames(typeof(KeyCode)); ;
			m_isEnableEnums = new bool[m_enumNames.Length];
			for (int i = 0; i < m_isEnableEnums.Length; ++i) m_isEnableEnums[i] = false;
		}
		public void EditReloadAxes(UnityEditor.SerializedProperty axes)
		{
			Dictionary<string, bool> isEnableAxesTemp = isEnableAxes;
			Dictionary<string, AxisMode> axisModesTemp = axisModes;

			isEnableAxes = new Dictionary<string, bool>();
			axisModes = new Dictionary<string, AxisMode>();

			List<string> axisNamesTemp = new List<string>();
			List<bool> isEnableAxesArrayTemp = new List<bool>();
			List<AxisMode> axisModesArrayTemp = new List<AxisMode>();

			for (int i = 0; i < axes.arraySize; ++i)
			{
				var name = axes.GetArrayElementAtIndex(i).FindPropertyRelative("m_Name").stringValue;
				if (axisNamesTemp.Contains(name)) continue;

				axisNamesTemp.Add(name);
				
				if (isEnableAxesTemp.ContainsKey(name))
				{
					isEnableAxesArrayTemp.Add(isEnableAxesTemp[name]);
					isEnableAxes.Add(name, isEnableAxesTemp[name]);
				}
				else
				{
					isEnableAxesArrayTemp.Add(false);
					isEnableAxes.Add(name, false);
				}
				if (axisModesTemp.ContainsKey(name))
				{
					axisModesArrayTemp.Add(axisModesTemp[name]);
					axisModes.Add(name, axisModesTemp[name]);
				}
				else
				{
					axisModesArrayTemp.Add(0);
					axisModes.Add(name, 0);
				}
			}
			m_axisNames = axisNamesTemp.ToArray();
			m_isEnableAxes = isEnableAxesArrayTemp.ToArray();
			m_axisModes = axisModesArrayTemp.ToArray();
		}
		public void EditBuildAxes()
		{
			if (m_axisNames == null) return;
			for (int i = 0; i < m_axisNames.Length; ++i)
			{
				isEnableAxes.Add(m_axisNames[i], m_isEnableAxes[i]);
				axisModes.Add(m_axisNames[i], m_axisModes[i]);
			}
		}
		public void SaveConvert()
		{
			for (int i = 0; i < axisNames.Length; ++i)
			{
				m_isEnableAxes[i] = isEnableAxes[axisNames[i]];
				m_axisModes[i] = axisModes[axisNames[i]];
			}
		}
#endif
	}
}