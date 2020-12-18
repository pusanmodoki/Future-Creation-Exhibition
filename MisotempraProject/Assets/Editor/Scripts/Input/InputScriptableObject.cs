using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>MisoTempra editor</summary>
namespace LocalEditor
{
	/// <summary>Input editor</summary>
	namespace Input
	{
		public class InputScriptableObject : ScriptableObject
		{
			public InputManagement.InputCashContainer cashContainer { get { return m_cashContainer; } }

			[SerializeField]
			InputManagement.InputCashContainer m_cashContainer = null;

			public void Initialize(InputManagement.InputCashContainer cashContainer)
			{
				m_cashContainer = cashContainer;
			}
		}
	}
}