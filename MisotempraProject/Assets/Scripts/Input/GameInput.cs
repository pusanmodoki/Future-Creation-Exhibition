using System.Collections.Generic;
using UnityEngine;

/// <summary>Input Management</summary>
namespace InputManagement
{
	[DefaultExecutionOrder(-50)]
	public class GameInput : Singleton.DontDestroySingletonMonoBehaviour<GameInput>
	{
		struct Buffer<Key, Value>
		{
			public Dictionary<Key, Value> now { get; private set; }
			public Dictionary<Key, Value> old { get; private set; }

			public Buffer(int zero)
			{
				now = new Dictionary<Key, Value>();
				old = new Dictionary<Key, Value>();
			}
			public void InitializeAdd(Key key, Value value)
			{
				now.Add(key, value);
				old.Add(key, value);
			}
			public void Update(Key key, Value value)
			{
				old[key] = now[key];
				now[key] = value;
			}
		}

		public static string cFileName = "InputManagement";
		public static string cFileBeginMark = "<<InputManagement>>";

		List<KeyCode> m_inputKeyCodes = new List<KeyCode>();
		Buffer<KeyCode, bool> m_resultKeyCodes = new Buffer<KeyCode, bool>(0);

		List<string> m_inputAxes = new List<string>();
		Dictionary<string, AxisMode> m_axisModes = new Dictionary<string, AxisMode>();
		Buffer<string, float> m_resultAxes = new Buffer<string, float>(0);
		Buffer<string, float> m_resultRawAxes = new Buffer<string, float>(0);
		Buffer<string, bool> m_resultButtons = new Buffer<string, bool>(0);
		string m_forReadBuf = null;
		AxisMode m_forAxisBuf = default;
		bool m_keyBuf = false;


		/// <summary>
		/// Input.GetKeyと同等の動作
		/// </summary>
		/// <param name="keyCode"></param>
		/// <returns></returns>
		public static bool GetKey(KeyCode keyCode)
		{
			return instance.m_resultKeyCodes.now[keyCode];
		}
		/// <summary>
		/// Input.GetKeyDownと同等の動作
		/// </summary>
		/// <param name="keyCode"></param>
		/// <returns></returns>
		public static bool GetKeyDown(KeyCode keyCode)
		{
			return ((instance.m_keyBuf = instance.m_resultKeyCodes.now[keyCode]) ^
				instance.m_resultKeyCodes.old[keyCode]) & instance.m_keyBuf;
		}
		/// <summary>
		/// Input.GetKeyUpと同等の動作
		/// </summary>
		/// <param name="keyCode"></param>
		/// <returns></returns>
		public static bool GetKeyUp(KeyCode keyCode)
		{
			return (instance.m_resultKeyCodes.now[keyCode] ^
				(instance.m_keyBuf = instance.m_resultKeyCodes.old[keyCode])) & instance.m_keyBuf;
		}
		/// <summary>
		/// Input.GetButtonと同等の動作
		/// </summary>
		/// <param name="axisName"></param>
		/// <returns></returns>
		public static bool GetButton(string axisName)
		{
#if UNITY_EDITOR
			if (!DAxisDebugCheck(axisName, AxisMode.Button))
				return false;
#endif
			return instance.m_resultButtons.now[axisName];
		}
		/// <summary>
		/// Input.GetButtonDownと同等の動作
		/// </summary>
		/// <param name="axisName"></param>
		/// <returns></returns>
		public static bool GetButtonDown(string axisName)
		{
#if UNITY_EDITOR
			if (!DAxisDebugCheck(axisName, AxisMode.Button))
				return false;
#endif
			return ((instance.m_keyBuf = instance.m_resultButtons.now[axisName]) ^
				instance.m_resultButtons.old[axisName]) & instance.m_keyBuf;
		}
		/// <summary>
		/// Input.GetButtonUpと同等の動作
		/// </summary>
		/// <param name="axisName"></param>
		/// <returns></returns>
		public static bool GetButtonUp(string axisName)
		{
#if UNITY_EDITOR
			if (!DAxisDebugCheck(axisName, AxisMode.Button))
				return false;
#endif
			return (instance.m_resultButtons.now[axisName] ^
				(instance.m_keyBuf = instance.m_resultButtons.old[axisName])) & instance.m_keyBuf;
		}
		/// <summary>
		/// Input.GetAxisと同等の動作
		/// </summary>
		/// <param name="axisName"></param>
		/// <returns></returns>
		public static float GetAxis(string axisName)
		{
#if UNITY_EDITOR
			if (!DAxisDebugCheck(axisName, AxisMode.Axis))
				return 0.0f;
#endif
			return instance.m_resultAxes.now[axisName];
		}
		/// <summary>
		/// Input.GetAxis - Input.GetAxis(前フレーム)
		/// </summary>
		/// <param name="axisName"></param>
		/// <returns></returns>
		public static float GetMoveAxisOfPreviousFrame(string axisName)
		{
#if UNITY_EDITOR
			if (!DAxisDebugCheck(axisName, AxisMode.Axis))
				return 0.0f;
#endif
			return instance.m_resultAxes.now[axisName] - instance.m_resultAxes.old[axisName];
		}
		/// <summary>
		/// Input.GetAxisRawと同等の動作
		/// </summary>
		/// <param name="axisName"></param>
		/// <returns></returns>
		public static float GetAxisRaw(string axisName)
		{
#if UNITY_EDITOR
			if (!DAxisDebugCheck(axisName, AxisMode.AxisRaw))
				return 0.0f;
#endif
			return instance.m_resultRawAxes.now[axisName];
		}
		/// <summary>
		/// Input.GetAxisRaw - Input.GetAxisRaw(前フレーム)
		/// </summary>
		/// <param name="axisName"></param>
		/// <returns></returns>
		public static float GetMoveAxisRawOfPreviousFrame(string axisName)
		{
#if UNITY_EDITOR
			if (!DAxisDebugCheck(axisName, AxisMode.AxisRaw))
				return 0.0f;
#endif
			return instance.m_resultRawAxes.now[axisName] - instance.m_resultRawAxes.old[axisName];
		}

		//Debug only
#if UNITY_EDITOR
		static bool DAxisDebugCheck(string axisName, AxisMode axisMode)
		{
			if (!instance.m_axisModes.ContainsKey(axisName))
			{
				Debug.LogWarning("GameInput-axisNameが入力軸として登録されていません");
				return false;
			}
			if (!instance.m_axisModes[axisName].HasFlag(axisMode))
			{
				Debug.LogWarning("GameInput-" + axisName + "では" +
					axisMode.ToString() + "が有効化されていません");
				return false;
			}
			return true;
		}
#endif

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
					m_inputKeyCodes.Add(tempKeyCode);
					m_resultKeyCodes.InitializeAdd(tempKeyCode, false);
				}
			}

			for (int i = 0; i < container.usePlayIsEnableAxes.Length; ++i)
			{
				if (container.usePlayIsEnableAxes[i])
				{
					var tempName = container.axisNames[i];
					m_inputAxes.Add(tempName);
					m_axisModes.Add(tempName, container.usePlayAxisModes[i]);
					m_resultAxes.InitializeAdd(tempName, 0.0f);
					m_resultRawAxes.InitializeAdd(tempName, 0.0f);
					m_resultButtons.InitializeAdd(tempName, false);
				}
			}
		}

		void Update()
		{
			foreach (var keyCode in m_inputKeyCodes)
				m_resultKeyCodes.Update(keyCode, Input.GetKey(keyCode));

			for (int i = 0; i < m_inputAxes.Count; ++i)
			{
				m_forReadBuf = m_inputAxes[i];
				m_forAxisBuf = m_axisModes[m_forReadBuf];

				if (m_forAxisBuf.HasFlag(AxisMode.Axis))
					m_resultAxes.Update(m_forReadBuf, Input.GetAxis(m_forReadBuf));
				if (m_forAxisBuf.HasFlag(AxisMode.AxisRaw))
					m_resultRawAxes.Update(m_forReadBuf, Input.GetAxisRaw(m_forReadBuf));
				if (m_forAxisBuf.HasFlag(AxisMode.Button))
					m_resultButtons.Update(m_forReadBuf, Input.GetButton(m_forReadBuf));
			}
		}
	}
}