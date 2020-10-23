using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTest : MonoBehaviour
{
	AI.BehaviorTree.BehaviorCompositeSequenceNode m_node = new AI.BehaviorTree.BehaviorCompositeSequenceNode();
    // Start is called before the first frame update
    void Start()
    {
		{
			var node = new AI.BehaviorTree.BehaviorTaskNode();
			node.task = new TestTask();
			m_node.nodes.Add(node);
		}
		{
			var node = new AI.BehaviorTree.BehaviorCompositeSequenceNode();
			m_node.nodes.Add(node);


			for (int i = 0; i < 3; ++i)
			{
				AI.BehaviorTree.BehaviorTaskNode taskNode = new AI.BehaviorTree.BehaviorTaskNode();
				taskNode.task = new TestTask();
				node.nodes.Add(taskNode);
			}
		}
		m_node.OnEnable();
    }

    // Update is called once per frame
    void Update()
    {
		var result = m_node.Update();
		if (result == AI.BehaviorTree.UpdateResult.Failed)
			m_node.OnDisable(result);

		Debug.Log(result);
    }
}
