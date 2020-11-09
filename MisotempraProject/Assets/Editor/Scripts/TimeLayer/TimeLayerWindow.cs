//制作者: 植村将太
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEditor;
using TimeManagement;
using TimeManagement.Detail;
using FileAccess;

/// <summary>MisoTempra editor</summary>
namespace Editor
{
	/// <summary>TimeLayer用Windowを生成するTimeLayerWindow class</summary>
	public class TimeLayerWindow : EditorWindow
	{
		/// <summary>static instance</summary>
		public static TimeLayerWindow instance { get; private set; } = null;
		/// <summary>DrawLineで使用する色</summary>
		static readonly Color[] m_cLineColors = new Color[3] { Color.gray, Color.red, Color.yellow };
		/// <summary>デフォルトのウィンドウ間隔</summary>
		static readonly Vector2 m_cDefaultWindowInterval = new Vector2(50.0f, 15.0f);


		/// <summary>プロパティで使用するセーブレイヤーリスト</summary>
		public static ReadOnlyCollection<SaveTimeLayer> saveLayers { get; private set; } = null;
		/// <summary>プロパティで使用するレイヤーリスト(key save layer)</summary>
		public static ReadOnlyDictionary<SaveTimeLayer, TimeLayer> layers { get; private set; } = null;
		/// <summary>プロパティで使用するレイヤーリスト(key guid)</summary>
		public static ReadOnlyDictionary<string, TimeLayer> layersKeyGuid { get; private set; } = null;
		/// <summary>プロパティで使用するレイヤーインデックスリスト(key guid)</summary>
		public static ReadOnlyDictionary<string, int> layerIndexesKeyGuid { get; private set; } = null;
		/// <summary>プロパティで使用するネームリスト</summary>
		public static string[] usePropertyLayerNames { get; private set; } = null;

		/// <summary>万が一セーブレイヤーがなければロードする</summary>
		public static void IfEmptyLoading() { if (m_saveLayers.Count == 0) DataLoad(); }

		/// <summary>セーブ用レイヤー</summary>
		static List<SaveTimeLayer> m_saveLayers = new List<SaveTimeLayer>();
		/// <summary>レイヤー</summary>
		static Dictionary<SaveTimeLayer, TimeLayer> m_layers = new Dictionary<SaveTimeLayer, TimeLayer>();
		/// <summary>各レイヤーレクト, key = guid</summary>
		static Dictionary<string, Rect> m_windowRects = new Dictionary<string, Rect>();
		/// <summary>プロパティ用リスト</summary>
		static Dictionary<string, TimeLayer> m_layersKeyGuid = new Dictionary<string, TimeLayer>();
		/// <summary>プロパティ用リスト</summary>
		static Dictionary<string, int> m_layerIndexesKeyGuid = new Dictionary<string, int>();
		/// <summary>EditorApplication.playModeStateChangedにSaveCallbackを追加したか</summary>
		static bool m_isAddSaveCallback = false;

		/// <summary>MenuCallbackで使用, ノード作成</summary>
		const string m_cMenuModeCreateNode = "CreateNode";
		/// <summary>MenuCallbackで使用, ノード削除</summary>
		const string m_cMenuModeDeleteNode = "DeleteNode";
		/// <summary>MenuCallbackで使用, セーブ</summary>
		const string m_cMenuModeSave = "Save";

		/// <summary>選択したノードの親子レイヤー, 0 = 親, 1 = 子</summary>
		List<string>[] m_selectValueFamily = new List<string>[2] { new List<string>(), new List<string>() };
		/// <summary>Window size</summary>
		Vector2 m_windowSize = default;
		/// <summary>左項目のサイズ</summary>
		Vector2 m_windowBlockLeftSize = default;
		/// <summary>右項目のサイズ</summary>
		Vector2 m_windowBlockRightSize = default;
		/// <summary>右項目のサイズその2</summary>
		Vector2 m_windowBlockRightSize2 = default;

		/// <summary>Open</summary>
		[MenuItem("Window/Time layer")]
		static void Open()
		{
			//インスタンス作成
			if (instance == null)
				instance = CreateInstance<TimeLayerWindow>();
			//コールバック追加
			if (!m_isAddSaveCallback)
			{
				EditorApplication.playModeStateChanged += SaveCallaback;
				m_isAddSaveCallback = true;
			}

			//ロード->表示
			instance.Load();
			instance.Show();
		}

		/// <summary>OnGUI</summary>
		void OnGUI()
		{
			//万が一セーブレイヤーがなければロードする
			if (m_saveLayers.Count == 0)
			{
				Load();
				if (m_saveLayers.Count == 0) return;
			}

			//ボタンアクション
			ButtonAction(Event.current);

			//線表示
			DrawLine(m_layers[m_saveLayers[0]]);

			//ノードウィンドウ表示
			BeginWindows();
			for (int i = 0; i < m_windowRects.Count; ++i)
			{
				m_windowRects[m_saveLayers[i].guid] = GUI.Window(i, m_windowRects[m_saveLayers[i].guid],
					DrawNodeWindow, m_saveLayers[i].name);
			}
			EndWindows();
		}
		/// <summary>OnDestroy</summary>
		void OnDestroy()
		{
			//インスタンスがあれば削除
			if (instance != null)
			{
				instance.Save();
				instance = null;
			}
		}


		/// <summary>セーブを行う</summary>
		void Save()
		{
			//パス取得
			string path = TimeManager.savePath, name = TimeManager.cSaveFileName;
			//セーブ
			FileAccessor.SaveObject(path, name, ref m_saveLayers, TimeManager.cFileBeginMark);
		}
		/// <summary>EditorApplication用コールバック</summary>
		static void SaveCallaback(PlayModeStateChange change)
		{
			//プレイモードになった場合セーブを行う
			if (change == PlayModeStateChange.EnteredPlayMode)
				instance.Save();
		}


		/// <summary>ロードを行う</summary>
		void Load()
		{
			//ウィンドウサイズを求める
			m_windowBlockLeftSize = EditorStyles.label.GetSize("Time scale: ");
			m_windowBlockRightSize = EditorStyles.label.GetSize("FFFFFFFFFFFFFFFFFFFF");;
			m_windowBlockRightSize2 = EditorStyles.label.GetSize("value->0.00000"); 
			m_windowBlockLeftSize.y = m_windowBlockRightSize.y += 5;
			m_windowSize = new Vector2(m_windowBlockLeftSize.x + m_windowBlockRightSize.x + 20,
				m_windowBlockLeftSize.y * 6);

			//Load
			DataLoad();

			//Add window
			LoadingAddWindowRect(m_layers[m_saveLayers[0]], 10, 10, new List<int>(), 0);
		}
		/// <summary>データロード</summary>
		static void DataLoad()
		{
			//リストクリア
			m_saveLayers.Clear();
			m_layers.Clear();
			m_windowRects.Clear();
			
			//パス取得
			string path = TimeManager.savePath, name = TimeManager.cSaveFileName;
			//ロードを行う, ファイルがなかった場合初期状態のものをセーブしそれを使う
			if (FileAccessor.IsExistsFile(path, name))
			{
				try { FileAccessor.LoadObject(path, name, out m_saveLayers, TimeManager.cFileBeginMark); }
				catch(System.Exception e) { Debug.LogError(e.Message); throw; }
			}
			else
			{
				m_saveLayers = new List<SaveTimeLayer>();
				m_saveLayers.Add(new SaveTimeLayer("root", "", new List<string>(), 1.0f));
				FileAccessor.SaveObject(path, name, ref m_saveLayers, TimeManager.cFileBeginMark);
			}

			//レイヤー追加ループ
			foreach (var e in m_saveLayers)
				m_layers.Add(e, new TimeLayer(e.name, e.timeScale, e.guid));
			//親子設定ループ
			foreach (var e in m_saveLayers)
			{
				//親がいれば
				if (e.parentGuid != null || e.parentGuid.Length > 0)
				{
					//親探査ループ
					foreach (var find in m_layers)
					{
						//親が見つかれば設定
						if (e.parentGuid == find.Value.guid)
						{
							m_layers[e].callManagerFunctions.SetParent(find.Value);
							break;
						}
					}
					//失敗？
					if (e.parentGuid.Length > 0 && m_layers[e].parent == null)
						Debug.LogError("Parentが見つかりません！ layer->" + e.name + ", parent->" + e.parentGuid);
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
						if (e.childrensGuid[i] == find.Value.guid)
						{
							m_layers[e].callManagerFunctions.AddChildren(find.Value);
							break;
						}
					}
				}
			}

			//Update
			UpdatePropertyInfo();

			//New
			saveLayers = new ReadOnlyCollection<SaveTimeLayer>(m_saveLayers);
			layers = new ReadOnlyDictionary<SaveTimeLayer, TimeLayer>(m_layers);
			layersKeyGuid = new ReadOnlyDictionary<string, TimeLayer>(m_layersKeyGuid);
			layerIndexesKeyGuid = new ReadOnlyDictionary<string, int>(m_layerIndexesKeyGuid);
		}
		/// <summary>ロード時のウィンドウ追加再帰関数</summary>
		void LoadingAddWindowRect(TimeLayer layer, float x, float y, List<int> windowPositionIndex, int depth)
		{
			//ウィンドウ追加
			m_windowRects.Add(layer.guid, new Rect(x, y, m_windowSize.x, m_windowSize.y));
			//depth indexを使用できない場合Addして確保
			while (windowPositionIndex.Count <= depth) windowPositionIndex.Add(0);

			//depthが0ではない->rootではない
			if (depth != 0)
			{
				//子供ループ
				for (int i = 0; i < layer.childrens.Count; ++i)
				{
					//同じ関数を実行
					LoadingAddWindowRect(layer.childrens[i], x + m_windowSize.x + m_cDefaultWindowInterval.x,
						y + (m_windowSize.y + m_cDefaultWindowInterval.y) * (windowPositionIndex[depth]++),
						windowPositionIndex, depth + 1);
				}
			}
			//root
			else
			{
				//childrenのまとまりでYを区切る
				float childrenY = y;
				//子供ループ
				for (int i = 0; i < layer.childrens.Count; ++i)
				{
					//同じ関数を実行
					LoadingAddWindowRect(layer.childrens[i], x + m_windowSize.x + m_cDefaultWindowInterval.x,
						childrenY + (m_windowSize.y + m_cDefaultWindowInterval.y) * (windowPositionIndex[depth]++),
						windowPositionIndex, depth + 1);

					//今回の子供グループのYMaxを取得
					int max = 0;
					for (int k = 0; k < windowPositionIndex.Count; ++k)
					{
						max = Mathf.Max(max, windowPositionIndex[k]);
						windowPositionIndex[k] = 0;
					}
					//Y座標に加算
					childrenY += (m_windowSize.y + m_cDefaultWindowInterval.y) * max;
				}
			}
		}

		/// <summary>プロパティ用情報更新</summary>
		static void UpdatePropertyInfo()
		{
			//プロパティ用ネーム設定
			List<string> result = new List<string>();
			Dictionary<string, int> guidToIndexes = new Dictionary<string, int>();
			for (int i = 0; i < m_layers.Count; ++i)
			{
				string layerName = m_layers[m_saveLayers[i]].name;
				int sameCounter = 0;
				for (int k = 0; m_saveLayers[k].guid != m_layers[m_saveLayers[i]].guid; ++k)
				{
					if (m_saveLayers[k].name == m_layers[m_saveLayers[i]].name)
						++sameCounter;
				}
				if (sameCounter > 0) layerName += " (" + sameCounter + ")";

				result.Add(layerName);
				guidToIndexes.Add(m_layers[m_saveLayers[i]].guid, i);
			}
			foreach (var e in m_layers)
			{
				string layerName = result[guidToIndexes[e.Value.guid]];
				if (e.Value.parent != null)
				{
					layerName = result[guidToIndexes[e.Value.parent.guid]] + "/" + layerName;
				}
				result[guidToIndexes[e.Value.guid]] = layerName;
			}
			usePropertyLayerNames = result.ToArray();

			//リスト構築
			m_layersKeyGuid.Clear();
			m_layerIndexesKeyGuid.Clear();
			for (int i = 0; i < m_saveLayers.Count; ++i)
			{
				m_layersKeyGuid.Add(m_saveLayers[i].guid, m_layers[m_saveLayers[i]]);
				m_layerIndexesKeyGuid.Add(m_saveLayers[i].guid, i);
			}
		}

		/// <summary>ボタンアクション用関数</summary>
		void ButtonAction(Event guiEvent)
		{
			//マウスダウンでなければ終わる
			if (guiEvent.type != EventType.MouseDown)
				return;

			//押したウィンドウのID
			int onWindow = MouseOnWindow(guiEvent.mousePosition);

			//ボタンでスイッチ
			switch (guiEvent.button)
			{
				//右
				case 1:
					{
						//IDが該当しないと思われる場合
						if (onWindow < 0)
						{
							//メニュー
							GenericMenu menu = new GenericMenu();

							//メニュー追加
							menu.AddItem(new GUIContent("Save"), false, MenuCallback, m_cMenuModeSave);

							//メニュー表示
							menu.ShowAsContext();
							guiEvent.Use();
						}
						//該当しとる！
						{
							//メニュー
							GenericMenu menu = new GenericMenu();

							//メニュー追加
							menu.AddItem(new GUIContent("Create children node"),
								false, MenuCallback, m_cMenuModeCreateNode + "," + onWindow.ToString());
							menu.AddItem(new GUIContent("Delete node"), false,
								MenuCallback, m_cMenuModeDeleteNode + "," + onWindow.ToString());
							menu.AddSeparator("");
							//メニュー追加
							menu.AddItem(new GUIContent("Save"), false, MenuCallback, m_cMenuModeSave);

							//メニュー表示
							menu.ShowAsContext();
							guiEvent.Use();
						}
					}
					break;
				//左
				case 0:
					{
						//IDが該当しないと思われる場合終了
						if (onWindow < 0)
						{
							//選択したノードの親子クリア
							m_selectValueFamily[0].Clear();
							m_selectValueFamily[1].Clear();
							return;
						}

						//親ノード追加
						for (var layer = m_layers[m_saveLayers[onWindow]]; layer != null; layer = layer.parent)
							m_selectValueFamily[0].Add(layer.guid);

						//子ノード追加
						AddChildrenGuid(m_layers[m_saveLayers[onWindow]], m_selectValueFamily[1]);
					}
					break;
				default: break;
			}
		}
		/// <summary>マウスがどのウィンドウに乗ってるか判断する</summary>
		int MouseOnWindow(Vector2 position)
		{
			//レイヤーでループし当たり判定を取る, あたったらindexを返却
			for (int i = 0; i < m_saveLayers.Count; ++i)
			{
				if (m_windowRects[m_saveLayers[i].guid].Contains(position))
					return i;
			}
			//あたってなければ-1
			return -1;
		}
		/// <summary>Childrenのguidをm_selectValueFamilyに追加する再帰関数</summary>
		void AddChildrenGuid(TimeLayer layer, List<string> list)
		{
			//追加
			list.Add(layer.guid);
			//子供でも実行
			foreach (var e in layer.childrens)
				AddChildrenGuid(e, list);
		}


		/// <summary>メニューのコールバック</summary>
		void MenuCallback(object obj)
		{
			//split string
			string[] split = obj.ToString().Split(',');
			//モード
			string type = split[0];

			//モードでスイッチ
			switch (type)
			{
				//Create node
				case m_cMenuModeCreateNode:
					{
						//レイヤーインデックス
						int index = int.Parse(split[1]);

						//New layerが何個あるか確認, 被らないような名前にする
						int equalNameCount = 0;
						foreach (var e in m_saveLayers)
							if (e.name.StartsWith("New layer")) ++equalNameCount;

						//セーブ用レイヤーに追加
						m_saveLayers.Add(new SaveTimeLayer(
							equalNameCount == 0 ? "New layer" : "New layer" + equalNameCount,
							m_saveLayers[index].guid, null, 1.0f));
						//レイヤーに追加
						m_layers.Add(m_saveLayers[m_saveLayers.Count - 1],
							new TimeLayer(m_saveLayers[m_saveLayers.Count - 1].name,
							1.0f, m_saveLayers[m_saveLayers.Count - 1].guid));
						//親登録
						m_layers[m_saveLayers[m_saveLayers.Count - 1]].callManagerFunctions.SetParent(m_layers[m_saveLayers[index]]);
						//作成したレイヤーの親に子供として登録
						m_saveLayers[index].AddChildren(m_saveLayers[m_saveLayers.Count - 1].guid);
						m_layers[m_saveLayers[index]].callManagerFunctions.AddChildren(m_layers[m_saveLayers[m_saveLayers.Count - 1]]);

						//ウィンドウ用Rect用を追加
						AddChildrenWindowRect(m_layers[m_saveLayers[m_saveLayers.Count - 1]]);

						//更新
						UpdatePropertyInfo();
					}
					break;
				//Delete node
				case m_cMenuModeDeleteNode:
					{
						//レイヤーインデックス
						int index = int.Parse(split[1]);

						//rootの場合削除しない
						if (m_layers[m_saveLayers[index]].parent == null)
						{
							Debug.LogError("TimeLayer->rootは削除できません");
							return;
						}

						//子供削除, 削除したguidリストを取得
						var list = m_layers[m_saveLayers[index]].parent.callManagerFunctions.DDeleteChildren(m_layers[m_saveLayers[index]]);
						//ウィンドウ削除
						m_windowRects.Remove(m_saveLayers[index].guid);
						//削除
						m_layers.Remove(m_saveLayers[index]);
						//セーブ用レイヤーから削除
						m_saveLayers.RemoveAt(index);

						//削除guidループ
						foreach (var guid in list)
						{
							//セーブ用レイヤーループ, guidが合致していた場合双方から削除
							for (int i = 0; i < m_saveLayers.Count; ++i)
							{
								if (m_saveLayers[i].guid == guid.ToString())
								{
									m_layers.Remove(m_saveLayers[i]);
									m_windowRects.Remove(m_saveLayers[i].guid);
									m_saveLayers.RemoveAt(i);
									break;
								}
							}
						}
						//更新
						UpdatePropertyInfo();
					}
					break;
				//Save
				case m_cMenuModeSave:
					Save();
					break;
				default: break;
			}
		}
		/// <summary>親ウィンドウを参照してウィンドウを作成</summary>
		void AddChildrenWindowRect(TimeLayer layer)
		{
			//自分の親から見た子供インデックスを検索, 取得
			int find = 0;
			for (int i = 0; i < layer.parent.childrens.Count; ++i)
				if (layer.parent.childrens[i].guid == layer.guid)
				{
					find = i;
					break;
				}

			//index 0の場合真横に配置
			if (find == 0)
				m_windowRects.Add(layer.guid, new Rect(m_windowRects[layer.parent.guid].xMax + m_cDefaultWindowInterval.x,
					m_windowRects[layer.parent.guid].y, m_windowSize.x, m_windowSize.y));
			//index > 0の場合上の子供の下に追加
			else
				m_windowRects.Add(layer.guid, new Rect(m_windowRects[layer.parent.guid].xMax + m_cDefaultWindowInterval.x,
					m_windowRects[layer.parent.childrens[find - 1].guid].yMax + m_cDefaultWindowInterval.y,
					m_windowSize.x, m_windowSize.y));
		}


		/// <summary>ノード描画</summary>
		public void DrawNodeWindow(int id)
		{
			//セーブ用レイヤーを取得
			var drawLayer = m_saveLayers[id];
			//描画ポジション
			Vector2 position = new Vector2(10, m_windowBlockLeftSize.y * 2);

			//以下ほとんどはポジション設定->描画

			GUI.Label(new Rect(position, m_windowBlockLeftSize), "Name: ");
			position.x += m_windowBlockLeftSize.x + 5;

			//rootの場合は変更できないようにlabelで表示
			if (drawLayer != m_saveLayers[0])
			{
				var field = GUI.TextField(new Rect(position, m_windowBlockRightSize), drawLayer.name);
				//スライダー変更された場合はセーブ用, 通常ともに更新
				if (GUI.changed)
				{
					m_layers[drawLayer].callManagerFunctions.SetName(field);
					drawLayer.SetName(field);
				}
			}
			else
				GUI.Label(new Rect(position, m_windowBlockRightSize), "<root>");

			position.x = 10;
			position.y += m_windowBlockLeftSize.y;
			GUI.Label(new Rect(position, m_windowBlockLeftSize), "Time scale: ");

			position.x = m_windowSize.x - m_windowBlockRightSize2.x;
			GUI.Label(new Rect(position, m_windowBlockRightSize2), "value->" + drawLayer.timeScale.ToString("F4"));

			position.x = m_windowSize.x - m_windowBlockRightSize2.x;
			position.y += m_windowBlockLeftSize.y;
			GUI.Label(new Rect(position, m_windowBlockRightSize2), "multi->" + m_layers[drawLayer].callManagerFunctions.DCalculateScale().ToString("F4"));

			position.x = 10 + m_windowBlockLeftSize.x + 5;
			position.y += m_windowBlockLeftSize.y;
			var scale = GUI.HorizontalSlider(new Rect(position, m_windowBlockRightSize), drawLayer.timeScale, 0.0f, 1.0f);
			//スライダー変更された場合はセーブ用, 通常ともに更新
			if (GUI.changed)
			{
				m_layers[drawLayer].callManagerFunctions.SetScale(scale);
				drawLayer.SetScale(scale);
			}

			GUI.DragWindow();
		}
		/// <summary>ノードをつなぐラインを描画, 再帰関数</summary>
		void DrawLine(TimeLayer layer)
		{
			//子供がいない場合終了
			if (layer.childrens == null) return;

			//子供ループ
			foreach (var e in layer.childrens)
			{
				//双方が赤描画に指定されている場合赤で描画
				if (m_selectValueFamily[0].Contains(layer.guid) && m_selectValueFamily[0].Contains(e.guid))
					DrawLine(m_windowRects[layer.guid], m_windowRects[e.guid], 1);
				//子供レイヤーが黄色描画に指定されている場合黄色で描画
				else if (m_selectValueFamily[1].Contains(layer.guid))
					DrawLine(m_windowRects[layer.guid], m_windowRects[e.guid], 2);
				//どちらも色指定されていない場合グレーで描画
				else
					DrawLine(m_windowRects[layer.guid], m_windowRects[e.guid], 0);
				//子供でも同じ関数を実行
				DrawLine(e);
			}
		}
		/// <summary>実際にラインを描画する関数</summary>
		void DrawLine(Rect preview, Rect next, int colorIndex)
		{
			//右->左か左->右で描画位置を変える
			if (preview.x <= next.x)
			{
				Vector3 start = new Vector3(preview.x + preview.width - 1, preview.y + 1, 0.0f);
				Vector3 end = new Vector3(next.x + 1, next.y + next.height - 1, 0.0f);
				Handles.DrawBezier(start, end, new Vector3(100f + start.x, start.y, start.z),
					new Vector3(-100 + end.x, end.y, end.z), m_cLineColors[colorIndex], null, 3f);
			}
			else
			{
				Vector3 start = new Vector3(preview.x + 1, preview.y + 1, 0.0f);
				Vector3 end = new Vector3(next.x + next.width - 1, next.y + next.height - 1, 0.0f);
				Handles.DrawBezier(start, end, new Vector3(-100f + start.x, start.y, start.z),
					new Vector3(100 + end.x, end.y, end.z), m_cLineColors[colorIndex], null, 3f);
			}
		}
	}
}