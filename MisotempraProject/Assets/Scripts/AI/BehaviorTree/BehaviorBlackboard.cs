using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		public class Blackboard
		{
			public static readonly string[] cKeyClassNames = new string[15]
			{
				typeof(GameObject).FullName,
				typeof(Transform).FullName,
				typeof(Component).FullName,
				typeof(AI.AIAgent).FullName,
				typeof(Quaternion).FullName,
				typeof(Vector2).FullName,
				typeof(Vector3).FullName,
				typeof(Vector4).FullName,
				typeof(string).FullName,
				typeof(System.Enum).FullName,
				typeof(int).FullName,
				typeof(float).FullName,
				typeof(double).FullName,
				typeof(Object).FullName,
				typeof(object).FullName,
			};
		}
	}
}