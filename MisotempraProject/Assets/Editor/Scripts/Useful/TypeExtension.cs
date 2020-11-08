using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>MisoTempra editor</summary>
namespace Editor
{
	public static class TypeExtension
	{
		public static System.Type FindTypeInAllAssembly(string fullName)
		{
			foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (var getType in assembly.GetTypes())
				{
					if (getType.FullName == fullName)
					{
						return getType;
					}
				}
			}

			return null;
		}
	}
}


