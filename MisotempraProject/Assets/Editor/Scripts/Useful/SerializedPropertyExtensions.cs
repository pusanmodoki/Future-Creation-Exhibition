using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SerializedPropertyExtensions 
{
	public static void ForElements(this UnityEditor.SerializedProperty self, System.Action<UnityEditor.SerializedProperty> loopAction)
	{
		for (int i = 0; i < self.arraySize; ++i)
			loopAction(self.GetArrayElementAtIndex(i));
	}
}
