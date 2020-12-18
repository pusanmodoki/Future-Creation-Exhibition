/// <summary>
/// メニューの統括
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CMenuSupervision : MonoBehaviour
{
    //--------------------------------------------------------------------
    [SerializeField] private CMenuTagManager m_TagManagerScript = null;
    [SerializeField] private CMenuContentTagManager m_ContentManagerScript = null;
    [SerializeField] private CMenuBackGround m_BackGroundScript = null;

    //--------------------------------------------------------------------
    [SerializeField] private KeyCode m_menuOpenKeyCode = KeyCode.Q;

    //--------------------------------------------------------------------
    /// <" 状態遷移 ">
    private enum eState
    {
        Non,            // 動いてない
        MenuOpen,
        BackGround,
        Tag_Active, // tagのアニメーション開始
        Content_Active,        // contentsを開始
        Select,
        MenuClose,
        InActive,

        // defaultで処理
        Max,
    }
    private eState m_stateMenu = eState.Non;

    //--------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_stateMenu)
        {
            case eState.Non:
                // 指定の入力でフェードイン
                if (Input.GetKeyDown(m_menuOpenKeyCode))
                {
                    Time.timeScale = 0;
                    CFadeController.StartFadeIn();
                    m_stateMenu = eState.MenuOpen;
                }
                break;
            case eState.MenuOpen:
                MenuOpen();
                break;
            // 背景のアニメーション
            case eState.BackGround:
                BackGround();
                break;
            // tagの表示アニメーションを開始
            case eState.Tag_Active:
                TagActiveAnimation();
                break;
            // tagに対応したコンテンツTagの表示アニメーション開始
            case eState.Content_Active:
                ContentAnimation();
                break;
            // 選択　(現在特になし)
            case eState.Select:
                if (Input.GetKeyDown(m_menuOpenKeyCode))
                {
                    CFadeController.StartFadeIn();

                    m_stateMenu = eState.MenuClose;
                }
                break;
            case eState.MenuClose:
                MenuClose();
                break;
            // 全てを消し去る
            case eState.InActive:
                InActive();
                break;
            default:

                break;
        }
    }

    ///====================================================================
    /// <" switch内の関数 ">
    //---------------------------------------------------------------------

    /// <summary>
    /// Tagのアニメーションを開始させる
    /// </summary>
    private void TagActiveAnimation()
    {
        m_TagManagerScript.ActiveAnimation();
        m_stateMenu++;
    }

    /// <summary>
    /// contentsのアニメーションを開始させる
    /// </summary>
    private void ContentAnimation()
    {
        m_ContentManagerScript.ActiveAnimation();
        m_stateMenu++;
    }

    /// <summary>
    /// 全てを終了
    /// </summary>
    private void InActive()
    {
        m_TagManagerScript.InActiveAnimation();
        m_ContentManagerScript.InActiveAnimation();
        m_BackGroundScript.InActiveAnimation();

        m_stateMenu = eState.Non;
    }

    /// <summary>
    /// 背景をいれる
    /// </summary>
    private void BackGround()
    {
        m_BackGroundScript.ActiveAnimation();
        m_stateMenu++;
    }

    /// <summary>
    /// fadeインさせる
    /// </summary>
    private void MenuClose()
    {
        // 終了したら指定の遷移へ
        if (CFadeController.m_proFadeIn)
        {
            // fadeOut開始
            CFadeController.StartFadeOut();
            // 消していく開始
            m_stateMenu = eState.InActive;
        }
    }

    /// <summary>
    /// fadeアウトさせる
    /// </summary>
    private void MenuOpen()
    {
        // fadeインがしゅうりょうしたら
        if (CFadeController.m_proFadeIn)
        {
            // fadeOut開始
            CFadeController.StartFadeOut();
            // 背景アニメーション開始
            m_stateMenu = eState.BackGround;
        }
    }
    //====================================================================
}
