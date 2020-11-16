using System.Collections.Generic;
using UnityEngine;

/// <summary>Input Management</summary>
namespace InputManagement
{
	[DefaultExecutionOrder(-50)]
	public class GameInput : SingletonMonoBehaviour<GameInput>
	{
		public static string cFileName = "InputManagement";
		public static string cFileBeginMark = "<<InputManagement>>";

		List<KeyCode> m_inputKeyCodes = new List<KeyCode>();
		Dictionary<KeyCode, bool> m_resultKeyCodes = new Dictionary<KeyCode, bool>();
		Dictionary<KeyCode, bool> m_oldResultKeyCodes = new Dictionary<KeyCode, bool>();

		List<string> m_inputAxes = new List<string>();
		Dictionary<string, bool> m_resultAxes = new Dictionary<string, bool>();
		Dictionary<string, bool> m_oldResultAxes = new Dictionary<string, bool>();

		protected override void Init()
		{
			InputCashContainer container;
			FileAccess.FileAccessor.LoadObject(Application.streamingAssetsPath + "/Input", cFileName,
				out container, cFileBeginMark);

			var values = System.Enum.GetValues(typeof(KeyCode));
			KeyCode tempKeyCode;
			for (int i = 0; i < container.isEnableEnums.Length; ++i)
			{
				if (container.isEnableEnums[i])
				{
					tempKeyCode = (KeyCode)values.GetValue(i);
					m_inputKeyCodes.Add((KeyCode)values.GetValue(i));
					m_resultKeyCodes.Add(m_inputKeyCodes.Back(), false);
					//m_oldResultKeyCodes.Add()
				}
			}

			for (int i = 0; i < container.isEnableAxes.Count; ++i)
			{
				if (container.usePlayIsEnableAxes[i])
				{
					m_inputAxes.Add(container.axisNames[i]);
					m_resultAxes.Add(m_inputAxes.Back(), false);
				}
			}
		}

		void Update()
		{
			foreach(var e in m_inputKeyCodes)
			{

			}
		}
	}
}