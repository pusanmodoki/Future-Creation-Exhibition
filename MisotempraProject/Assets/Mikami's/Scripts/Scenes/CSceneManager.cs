using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CSceneManager : MonoBehaviour
{
    //-----------------------------------------------------------
    private static GameObject m_objSceneManager = null;

    //-----------------------------------------------------------
    private static string m_sceneName = null;

    //-----------------------------------------------------------
    private enum eState
    {
        Non,
        FadeIn,
        SceneChange,
        FadeOut,
        Max,
    }
    private static eState m_stateScene = eState.Max;

    //-----------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (m_stateScene)
        {
            case eState.Non:
                // 次のシーン先を渡されている場合
                if (m_sceneName != null)
                {
                    m_stateScene++;
                }
                break;
            case eState.FadeIn:
                CFadeController.StartFadeIn();
                m_stateScene++;
                break;
            case eState.SceneChange:
                // fadeIn終了
                if (CFadeController.m_proFadeIn)
                {
                    SceneManager.LoadScene(m_sceneName);
                    m_stateScene++;
                }
                break;
            case eState.FadeOut:
                CFadeController.StartFadeOut();

                // 遷移名を消去
                m_sceneName = null;

                m_stateScene = eState.Max;
                break;
            default:
                break;
        }
    }

    //-----------------------------------------------------------
    /// <summary>
    /// 遷移先のシーン名
    /// </summary>
    /// <param name="nextSceneName"></param>
    public static void SceneChange(string nextSceneName)
    {
        Init();
        // まだ次のシーン名が入力されていない場合
        if (m_sceneName == null)
            m_sceneName = nextSceneName; m_stateScene = eState.Non;
    }

    //-----------------------------------------------------------
    /// <summary>
    /// 初期化関数
    /// </summary>
    private static void Init()
    {
        // まだスクリプト付きオブジェクトを作ってない場合
        if (m_objSceneManager == null)
        {
            //生成
            m_objSceneManager = new GameObject("SceneManager");

            // スクリプトを付ける
            m_objSceneManager.AddComponent<CSceneManager>();

            // 残す
            DontDestroyOnLoad(m_objSceneManager);
            //// Canvasを探す
            //GameObject n_objCanvas = GameObject.Find("Canvas");
            //// Canvasが存在してなかったら
            //if (n_objCanvas == null)
            //    n_objCanvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));

            //// 子供にする
            //m_objSceneManager.transform.parent = n_objCanvas.transform;
        }
    }
}
