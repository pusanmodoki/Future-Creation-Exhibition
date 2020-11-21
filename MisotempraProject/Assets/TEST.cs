using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.BehaviorTree;

public class TEST : MonoBehaviour
{
	[SerializeField]
	TimeManagement.TimeLayer timeLayer = null;
	[SerializeField]
	TagEx tag = null;
	[SerializeField]
	TagEx[] tags = null;

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
        
    }
}
