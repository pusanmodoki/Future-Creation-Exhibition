using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		public class BehaviorTree : MonoBehaviour
		{
			BehaviorBaseNode baseNode;
			List<BehaviorBaseNode> nodes;
			// Start is called before the first frame update
			void Start()
			{

			}

			// Update is called once per frame
			void Update()
			{
				if (!baseNode.Update())
				{
				}

			}
		}
	}
}