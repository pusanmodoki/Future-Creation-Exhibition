using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeManagement.Detail;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	namespace TimeLayer
	{
		public class TimeLayerScriptableObject : ScriptableObject
		{
			public List<SaveTimeLayer> saveLayers { get { return m_saveLayers; } }

			[SerializeField]
			List<SaveTimeLayer> m_saveLayers = new List<SaveTimeLayer>();

			public void Initialize(List<SaveTimeLayer> saveLayers)
			{
				m_saveLayers = saveLayers;
			}
		}
	}
}