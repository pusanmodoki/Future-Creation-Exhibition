using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.BehaviorTree.Node;
using Composite = AI.BehaviorTree.Node.Composite;

public class BehaviorTest : MonoBehaviour
{
	Composite.SequenceNode m_node = new Composite.SequenceNode();
    // Start is called before the first frame update
    void Start()
    {
		{
			var node = new TaskNode();
			node.task = new TestTask();
			m_node.nodes.Add(node);
		}
		{
			var node = new Composite.SequenceNode();
			m_node.nodes.Add(node);


			for (int i = 0; i < 3; ++i)
			{
				TaskNode taskNode = new TaskNode();
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
