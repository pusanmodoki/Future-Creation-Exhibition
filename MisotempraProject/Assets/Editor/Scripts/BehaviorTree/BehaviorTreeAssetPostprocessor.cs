using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>MisoTempra editor</summary>
namespace LocalEditor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		public class BehaviorTreeAssetPostprocessor : AssetPostprocessor
		{
			static void OnPostprocessAllAssets(string[] importedAssets,
				string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
			{
				foreach (var path in deletedAssets)
				{
					if (path.IndexOf(AI.BehaviorTree.BehaviorTree.cDUseEditorDataSavePath) == 0 &&
						path.IndexOf(FileAccess.FileAccessor.cExtension) == path.Length - FileAccess.FileAccessor.cExtension.Length)
					{
						foreach (var e in BehaviorTreeWindow.instances)
						{
							string findPath = "/" + e.fileName + "." + FileAccess.FileAccessor.cExtension;
							int find = path.IndexOf(findPath);
							if (find == path.Length - findPath.Length)
							{
								e.SetTrueIsDeleteFile();
								e.Close();
								break;
							}
						}

						return;
					}
				}
			}
		}
	}
}