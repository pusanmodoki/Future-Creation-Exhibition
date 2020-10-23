using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>Behavior tree editor</summary>
	namespace BehaviorTree
	{
		public class BehaviorTreeAssetPostprocessor : AssetPostprocessor
		{
			static void OnPostprocessAllAssets(string[] importedAssets,
				string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
			{
				foreach (var path in importedAssets)
				{
					if (path.IndexOf(AI.BehaviorTree.BehaviorTree.cDUseEditorDataSavePath) == 0 &&
						path.IndexOf(FileAccess.FileAccessor.cExtension) == path.Length - FileAccess.FileAccessor.cExtension.Length)
					{
						BehaviorTreeWindow.ReloadFileInfomations();
						return;
					}
				}
				foreach (var path in movedAssets)
				{
					if (path.IndexOf(AI.BehaviorTree.BehaviorTree.cDUseEditorDataSavePath) == 0 &&
						path.IndexOf(FileAccess.FileAccessor.cExtension) == path.Length - FileAccess.FileAccessor.cExtension.Length)
					{
						BehaviorTreeWindow.ReloadFileInfomations();
						return;
					}
				}
				foreach (var path in deletedAssets)
				{
					if (path.IndexOf(AI.BehaviorTree.BehaviorTree.cDUseEditorDataSavePath) == 0 &&
						path.IndexOf(FileAccess.FileAccessor.cExtension) == path.Length - FileAccess.FileAccessor.cExtension.Length)
					{
						BehaviorTreeWindow.ReloadFileInfomations();

						foreach (var e in BehaviorTreeWindow.instances)
						{
							int find = e.fileName.IndexOf(path);
							if (find == e.fileName.Length - path.Length)
							{
								e.SetDeleteFileFile();
								EditorWindow.Destroy(e);
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