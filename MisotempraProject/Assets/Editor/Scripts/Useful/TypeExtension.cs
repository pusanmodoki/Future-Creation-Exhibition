using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;


/// <summary>MisoTempra editor</summary>
namespace LocalEditor
{
	public static class TypeExtension
	{
		/// <summary>
		/// プロジェクト内に存在する全スクリプトファイル
		/// </summary>
		static MonoScript[] monoScripts { get { return m_monoScripts ?? (m_monoScripts = Resources.FindObjectsOfTypeAll<MonoScript>().ToArray()); } }

		static MonoScript[] m_monoScripts;

		public static System.Type FindTypeInAllAssembly(this System.Type type, string fullName)
		{
			return FindTypeInAllAssembly(fullName);
		}

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

		public static IEnumerable<System.Type> GetAllTypes(this System.Type type)
		{
			return GetAllTypes();
		}

		public static IEnumerable<System.Type> GetAllTypes()
		{
			// Unity標準のクラスタイプ
			var buitinTypes = System.AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(asm => asm.GetTypes())
			.Where(type => type != null && !string.IsNullOrEmpty(type.Namespace))
			.Where(type => type.Namespace.Contains("UnityEngine"));

			// 自作のクラスタイプ
			var myTypes = monoScripts
			.Where(script => script != null)
			.Select(script => script.GetClass())
			.Where(classType => classType != null)
			.Where(classType => classType.Module.Name == "Assembly-CSharp.dll");

			return buitinTypes.Concat(myTypes)
			.Distinct();
		}
	}
}


