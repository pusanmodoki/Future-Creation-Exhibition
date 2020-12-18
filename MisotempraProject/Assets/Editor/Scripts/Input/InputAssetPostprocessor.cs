using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>MisoTempra editor</summary>
namespace LocalEditor
{
	/// <summary>Input editor</summary>
	namespace Input
	{
		public class InputAssetPostprocessor : UnityEditor.AssetPostprocessor
		{
			static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPath)
			{
				// InputManagerの変更チェック
				var inputManagerPath = System.Array.Find(
					importedAssets, path => System.IO.Path.GetFileName(path) == "InputManager.asset");
				if (inputManagerPath == null)
					return;

				if (InputEditorWindow.instance != null)
					InputEditorWindow.instance.LoadInputManager(inputManagerPath);
			}
		}
	}
}
