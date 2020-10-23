using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GUIStyleExtension
{
	public static Vector2 GetSize(this GUIStyle self, string text)
	{
		var content = new GUIContent(text);
		var size = self.CalcSize(content);
		return size;
	}
}