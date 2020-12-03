using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CFadeController : MonoBehaviour
{
    //----------------------------------------------------------
    // 
    private static GameObject m_fadePanel = null;
    private static Image m_fadeImage = null;

    //----------------------------------------------------------
    // 
    private enum eFade
    {
        Non,
        FadeIn,
        FadeOut,
        Max,
    }
    private static eFade m_stateFade = eFade.Non;

    //----------------------------------------------------------
    // fadeIn用
    private static bool m_flgFadeIn = false;
    public static bool m_proFadeIn
    {
        private set
        {            
            m_flgFadeIn = value;
        }
        // 終了 : true / 処理中等 : false
        get
        {
            bool getter = m_flgFadeIn;
            if (getter)
                m_flgFadeIn = false;
            return getter;
        }
    }

    // fadeOut用
    private static bool m_flgFadeOut = false;
    public static bool m_proFadeOut
    {
        private set
        {
            m_flgFadeOut = value;
        }
        // 終了 : true / 処理中等 : false
        get
        {
            bool getter = m_flgFadeOut;
            if (getter)
                m_flgFadeOut = false;
            return getter;
        }
    }

    //----------------------------------------------------------
    private const int m_addAlpha = 5;
    private static Color32 m_colorPanel = new Color32(0, 0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_stateFade)
        {
            case eFade.Non:
                break;

            case eFade.FadeIn:
                FadeIn();
                break;

            case eFade.FadeOut:
                FadeOut();
                break;
            default:
                break;
        }
    }

    //----------------------------------------------------------
    /// <summary>
    /// FadeIn開始
    /// </summary>
    public static void StartFadeIn()
    {
        Init(eFade.FadeIn);
        m_stateFade = eFade.FadeIn;
    }

    /// <summary>
    /// FadeOut開始
    /// </summary>
    public static void StartFadeOut()
    {
        Init(eFade.FadeOut);
        m_stateFade = eFade.FadeOut;
    }

    //----------------------------------------------------------
    /// <summary>
    /// FadeIn関数
    /// </summary>
    private void FadeIn()
    {
        m_colorPanel.a += m_addAlpha;
        //真っ黒画面になる
        if (m_colorPanel.a >= 255)
        {
            m_colorPanel.a = 255;

            // fadeイン終了を知らせる
            m_proFadeIn = true;

            m_stateFade = eFade.Non;
        }
        m_fadeImage.color = m_colorPanel;
    }

    /// <summary>
    /// FadeOut関数
    /// </summary>
    private void FadeOut()
    {
        m_colorPanel.a -= m_addAlpha;
        //真っ黒画面になる
        if (m_colorPanel.a <= 0)
        {
            m_colorPanel.a = 0;

            // fadeアウト終了を知らせる
            m_proFadeOut = true;

            m_stateFade = eFade.Non;
        }
        m_fadeImage.color = m_colorPanel;
    }

    //-----------------------------------------------------------------------------------------
    /// <summary>
    /// 初期化関数
    /// </summary>
    private static void Init(eFade state)
    {
        //-----------------------------------------------------------------------------------------
        // panelが生成されていない時
        if (m_fadePanel == null)
        {
            // オブジェクト生成
            GameObject n_objCanvas = GameObject.Find("Canvas");
            // Canvasが存在してなかったら
            if (n_objCanvas == null)
                n_objCanvas = new GameObject("Canvas",typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));

            GameObject n_FadeManager = new GameObject("FadeManager");
            m_fadePanel = new GameObject("FadePanel", typeof(CFadeController));

            // 画面サイズ
            m_fadePanel.transform.localScale = new Vector3(Screen.width, Screen.height, 0f);

            // 画像
            m_fadeImage = m_fadePanel.AddComponent<Image>();
            m_fadeImage.raycastTarget = false;

            // 親子関係の構築
            n_FadeManager.transform.parent = n_objCanvas.transform;
            m_fadePanel.transform.parent = n_FadeManager.transform;

            // panelの位置調整
            m_fadePanel.transform.position = n_objCanvas.transform.position;
        }

        //-----------------------------------------------------------------------------------------
        // panelのアルファー値を初期化
        if (m_stateFade == eFade.Non)
        {
            switch (state)
            {
                case eFade.FadeIn:
                    m_colorPanel = new Color32(0, 0, 0, 0);

                    m_proFadeIn = false;
                    break;
                case eFade.FadeOut:
                    m_colorPanel = new Color32(0, 0, 0, 255);

                    m_proFadeOut = false;
                    break;
                default:
                    break;
            }

            // 色を挿入
            m_fadeImage.color = m_colorPanel;
        }
    }
}
