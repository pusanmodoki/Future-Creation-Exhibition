using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>MisoTempra editor</summary>
namespace LocalEditor
{
	public static class SerializedPropertyExtensions
	{
		public static void ForElements(this UnityEditor.SerializedProperty self, System.Action<UnityEditor.SerializedProperty> loopAction)
		{
			for (int i = 0; i < self.arraySize; ++i)
				loopAction(self.GetArrayElementAtIndex(i));
		}

		public static void ArrayAddEmpty(this UnityEditor.SerializedProperty self)
		{
			self.InsertArrayElementAtIndex(self.arraySize);
		}
		public static SerializedProperty ArrayBack(this UnityEditor.SerializedProperty self)
		{
			return self.GetArrayElementAtIndex(self.arraySize - 1);
		}
		public static SerializedProperty GetArrayElement(this UnityEditor.SerializedProperty self, int index)
		{
			return self.GetArrayElementAtIndex(index);
		}
	}
}