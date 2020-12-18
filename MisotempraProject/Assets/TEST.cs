using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.BehaviorTree;

public class TEST : MonoBehaviour
{
	[SerializeField]
	TypeName m_typeName = default;
	[SerializeField]
	Damage.DamageController m_controller = null;
    // Start is called before the first frame update
    void Start()
    {
		//BehaviorTree.LoadBehaviorTree("a");

		//BehaviorTree tree = new BehaviorTree("a");

		//int a = 0;
		//a = 0;
		var i = TimeManagement.TimeManager.instance;
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
		{
			m_controller.sender.EnableAction("Test");
			int a = 0;
			a = 0;
		}
    }
}
