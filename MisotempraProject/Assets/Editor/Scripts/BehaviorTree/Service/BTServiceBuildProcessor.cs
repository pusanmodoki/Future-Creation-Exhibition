using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Callbacks;
using System.IO;

/// <summary>MisoTempra editor</summary>
namespace LocalEditor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		public static class BTServiceBuildProcessor
		{
			static string m_savePath { get { return $"{Application.dataPath}/{m_cSavePathDetail}"; } }
			static readonly string m_cSavePathDetail = "Editor/Scripts/BehaviorTree/Service/";
			static readonly string m_cClassSaveFileName = "ServiceScriptableObjects.cs";

			[DidReloadScripts]
			static void OnPreprocessBuild()
			{
				Dictionary<string, string> writeClassNameKeyClassNames;
				SaveClass(out writeClassNameKeyClassNames);
			}

			static void SaveClass(out Dictionary<string, string> writeClassNameKeyClassNames)
			{
				string data = "";
				byte[] dataByte = null;
				string fullPath = $"{m_savePath}/{m_cClassSaveFileName}";

				List<string> writeClassNames = new List<string>();
				writeClassNameKeyClassNames = new Dictionary<string, string>();

				data +=
					"using System.Collections;\n" +
					"using UnityEngine;\n\n" +
					"/// <summary>MisoTempra editor</summary>\n" +
					"namespace LocalEditor\n" +
					"{\n" +
					"\t/// <summary>Behavior tree editor</summary>\n" +
					"\tnamespace BehaviorTree\n" +
					"\t{\n" +
					"\t\t///<summary>Write editor classes</summary>\n" +
					"\t\tnamespace ServiceScriptableObjects\n" +
					"\t\t{\n" +
					"\t\t\tpublic class BaseServiceScriptableObject : UnityEngine.ScriptableObject {}\n\n";

				foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
				{
					foreach (var type in assembly.GetTypes())
					{
						if (!type.IsClass || type.IsAbstract || !type.IsSubclassOf(typeof(AI.BehaviorTree.BaseService)))
							continue;

						int counter = -1;
						string name = "";
						do
						{
							name = "ServiceScriptableObjectClassName" + type.Name;
							if (counter >= 0) name += counter;
							++counter;
						} while (writeClassNames.Contains(name));
						writeClassNames.Add(name);
						writeClassNameKeyClassNames.Add(type.FullName, name);

						data +=
							"\t\t\tpublic class " + name + " : BaseServiceScriptableObject\n" +
							"\t\t\t{\n" +
							"\t\t\t\tpublic " + type.FullName + " service { get { return m_service; } }\n" +
							"\t\t\t\t[SerializeField]\n" +
							"\t\t\t\t" + type.FullName + " m_service = null;\n" +
							"\t\t\t\tpublic void Initialize(" + type.FullName + " initialize) { m_service = initialize; }\n" +
							"\t\t\t}\n";
					}
				}

				data += "\n";

				data +=
					"\t\t\tpublic static class ServiceScriptableObjectClassMediator\n" +
					"\t\t\t{\n" +
					"\t\t\t\tpublic static void CreateEditorAndScriptableObject(AI.BehaviorTree.BaseService service, \n" +
					"\t\t\t\t\tout UnityEditor.Editor editor, out UnityEngine.ScriptableObject scriptableObject, string serviceTypeFullName)\n" +
					"\t\t\t\t{\n";

				foreach (var names in writeClassNameKeyClassNames)
				{
					data +=
						"\t\t\t\t\tif (serviceTypeFullName == typeof(" + names.Key + ").FullName)\n" +
						"\t\t\t\t\t{\n" +
						"\t\t\t\t\t\tscriptableObject = UnityEngine.ScriptableObject.CreateInstance<" + names.Value + ">();\n" +
						"\t\t\t\t\t\t(scriptableObject as " + names.Value + ").Initialize(service as " + names.Key + ");\n" +
						"\t\t\t\t\t\teditor = UnityEditor.Editor.CreateEditor(scriptableObject as " + names.Value + ");\n" +
						"\t\t\t\t\t\treturn;\n" +
						"\t\t\t\t\t}\n";
				}

				data += "\t\t\t\t\tscriptableObject = null;\n" +
							  "\t\t\t\t\teditor = null;\n" +
							 "\t\t\t\t\treturn;\n" +
							 "\t\t\t\t}\n" +
							 "\t\t\t}\n" +
							 "\t\t}\n" +
							 "\t}\n" +
							 "}";

				dataByte = System.Text.Encoding.UTF8.GetBytes(data);

				if (!Directory.Exists(m_savePath))
					Directory.CreateDirectory(m_savePath);
				using (FileStream fileStream = File.Create(fullPath))
				{
					fileStream.Write(dataByte, 0, dataByte.Length);
				}
			}
		}
	}
}