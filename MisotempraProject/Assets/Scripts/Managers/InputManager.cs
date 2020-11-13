using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	[System.Serializable]
	public class InputInfomation
	{
		public KeyCode key { get { return m_key; } }
		public string axis { get { return m_axis; } }
		public bool isEnable { get { return m_isEnable; } }

		[SerializeField]
		KeyCode m_key = KeyCode.None;
		[SerializeField]
		string m_axis = "";
		[SerializeField]
		bool m_isEnable = false;
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
