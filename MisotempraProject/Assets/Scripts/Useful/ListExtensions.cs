using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions 
{
	public static T Front<T>(this List<T> list)
	{
		return list[0];
	}
	public static T Back<T>(this List<T> list)
	{
		return list[list.Count - 1];
	}
}
