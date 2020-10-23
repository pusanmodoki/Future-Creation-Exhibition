using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		public class BehaviorTree : MonoBehaviour
		{
			/// <summary>Data save path</summary>
			public static string dataSavePath { get { return Application.streamingAssetsPath + "/AI"; } }
			public static readonly string cFileBeginMark = "<<B>>BehaviorTreeNodeDataBeginMark";
#if UNITY_EDITOR
			/// <summary>Data save path (Assetから, Editor用)</summary>
			public static readonly string cDUseEditorDataSavePath = "Assets/StreamingAssets/AI";
#endif


			BehaviorBaseNode baseNode;
			List<BehaviorBaseNode> nodes;
			// Start is called before the first frame update
			void Start()
			{

			}

			// Update is called once per frame
			void Update()
			{
				//if (!baseNode.Update())
				//{
				//}

			}
		}
	}
}