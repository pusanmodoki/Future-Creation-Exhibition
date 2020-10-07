//制作者: 植村将太
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using FileAccess;
using TimeManagement.Detail;

/// <summary>Time management</summary>
namespace TimeManagement
{
	/// <summary>DeltaTimeを管理するTimeManager class</summary>
	[DefaultExecutionOrder(-1000)]
	public class TimeManager : SingletonMonoBehaviour<TimeManager>
	{
		/// <summary>Layer data save path</summary>
		public static string LayerSavePath() { return Application.streamingAssetsPath + "/Time"; }
		/// <summary>Layer data file name</summary>
		public static string LayerSaveFileName() { return "TimeLayerData.bin"; }

		/// <summary>root time layer</summary>
		public TimeLayer rootLayer { get; private set; } = null;
		/// <summary>root time layer</summary>
		public ReadOnlyCollection<TimeLayer> layers { get; private set; } = null;
		/// <summary>layers indexes(key = layer guid)</summary>
		public ReadOnlyDictionary<string, int> layerIndexes { get; private set; } = null;

		/// <summary>layers</summary>
		List<TimeLayer> m_layers = new List<TimeLayer>();
		/// <summary>layer indexes</summary>
		Dictionary<string, int> m_layerIndexes = new Dictionary<string, int>();

		/// <summary>Awakeの代わり</summary>
		protected override void Init()
		{
			base.Awake();
			if (!m_isThisInstance) return;

			//new
			layers = new ReadOnlyCollection<TimeLayer>(m_layers);
			layerIndexes = new ReadOnlyDictionary<string, int>(m_layerIndexes);

			//パス取得
			string path = LayerSavePath(), name = LayerSaveFileName();
			//セーブ用レイヤー
			List<SaveTimeLayer> loadLayer;
			//ロードを行う, ファイルがなかった場合初期状態のものをセーブしそれを使う
			if (FileAccessor.IsExistsFile(path, name))
				FileAccessor.LoadObject(path, name, out loadLayer);
			else
			{
				loadLayer = new List<SaveTimeLayer>();
				loadLayer.Add(new SaveTimeLayer("root", "", new List<string>(), 1.0f));
				FileAccessor.SaveObject(path, name, ref loadLayer);
			}

			//各レイヤーリストに追加
			for (int i = 0; i < loadLayer.Count; ++i)
			{
				m_layers.Add(new TimeLayer(loadLayer[i].name,
					loadLayer[i].timeScale, loadLayer[i].guid));

				m_layerIndexes.Add(loadLayer[i].guid, i);
			}

			//親子設定ループ
			foreach (var e in loadLayer)
			{
				//親がいれば
				if (e.parentGuid != null || e.parentGuid.Length > 0)
				{
					//親探査ループ
					foreach (var find in m_layers)
					{
						//親が見つかれば設定
						if (e.parentGuid == find.guid)
						{
							m_layers[m_layerIndexes[e.guid]].callManagerFunctions.SetParent(find);
							break;
						}
					}
#if UNITY_EDITOR
					//失敗？
					if (e.parentGuid.Length > 0 && m_layers[m_layerIndexes[e.guid]].parent == null)
						Debug.LogError("Parentが見つかりません！ layer->" + e.name + ", parent->" + e.parentGuid);
#endif
				}

				//子がいなければcontinue
				if (e.childrensGuid == null || e.childrensGuid.Count == 0) continue;
				//子供検索ループ
				for (int i = 0; i < e.childrensGuid.Count; ++i)
				{
					//子探査ループ
					foreach (var find in m_layers)
					{
						//子が見つかれば設定
						if (e.childrensGuid[i] == find.guid)
						{
							m_layers[m_layerIndexes[e.guid]].callManagerFunctions.AddChildren(find);
							break;
						}
					}
				}
			}

			//Root設定
			rootLayer = m_layers[0];
			TimeLayer.CallManagerFunctions.Awake(rootLayer, layers);
			rootLayer.callManagerFunctions.Update(1.0f / 60.0f);
			rootLayer.callManagerFunctions.FixedUpdate(Time.fixedDeltaTime);
		}

		/// <summary>Update</summary>
		void Update()
		{
			rootLayer.callManagerFunctions.Update(Time.deltaTime);
		}
		/// <summary>FixedUpdate</summary>
		void FixedUpdate()
		{
			rootLayer.callManagerFunctions.FixedUpdate(Time.fixedDeltaTime);
		}
	}
}