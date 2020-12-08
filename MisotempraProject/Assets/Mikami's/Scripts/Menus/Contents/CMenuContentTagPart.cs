using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CMenuContentTagPart : CAnimationController
{
    //----------------------------------------------------
    // contentsの中身
    [SerializeField] private CMoveFont m_MoveFontScript;
    [SerializeField] private CMenuContent2 m_ContentScript;

    //----------------------------------------------------
    // 状態遷移
    private enum eState
    {
        Non,
        Active,
        Select,
        UnSelect,
        InActive,
        Max,
    }
    private eState m_statePart = eState.Non;
    //----------------------------------------------------
    // 自分の配置されている番号
    public int m_proButtonNo { set; get; }
    private bool m_stateFontAnimation = false;

    //--------------------------------------------------------------
    [SerializeField] private Image m_imgBackGround;
    [SerializeField] private Color32 m_colBackGround = new Color32(255, 255, 255, 255);
    [SerializeField] private Color32 m_colHitBackGround = new Color32(255, 255, 255, 255);

    // Start is called before the first frame update
    void Start()
    {
        m_proButtonNo = 0;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_statePart)
        {
            case eState.Non:
                break;
            case eState.Active:
                Active();
                break;
            case eState.Select:
                Select();
                break;
            case eState.UnSelect:
                UnSelect();
                break;
            case eState.InActive:
                InActive();
                break;
            default:
                break;
        }

        //-----------------------------------------------------------------------
        // contentTagのActiveアニメーションが終わったら,FontAnimationを開始する
        if (m_stateFontAnimation)
        {
            if (base.JudgeAnimation("Base Layer.MenuContent_Active"))
            {
                FontAnimation();                
                m_stateFontAnimation = false;
            }
        }
    }
    //----------------------------------------------------
    /// <summary>
    /// Init関数
    /// </summary>
    public void Init()
    {
        base.ChangeBoolAnimation(false, "Active");
        base.ChangeBoolAnimation(false, "Select");
    }

    //----------------------------------------------------
    // switch内で使用

    /// <summary>
    /// Active関数
    /// </summary>
    private void Active()
    {
        // アニメーション関連
        base.ChangeBoolAnimation(true, "Active");
        m_statePart = eState.Max;

        // フォントアニメーションを開始する
        m_stateFontAnimation = true;
    }

    /// <summary>
    /// Select関数
    /// </summary>
    private void Select()
    {
        base.ChangeBoolAnimation(true, "Select");
        m_statePart = eState.Max;
    }

    /// <summary>
    /// UnSelect関数
    /// </summary>
    private void UnSelect()
    {
        base.ChangeBoolAnimation(false, "Select");
        m_statePart = eState.Max;
    }


    /// <summary>
    /// InActive関数
    /// </summary>
    private void InActive()
    {
        base.ChangeBoolAnimation(false, "Active");

        //// Content非表示
        //for (int i = 0; i < m_Contents.Count; i++)
        //    m_Contents[i].InActiveAnimation();
        m_ContentScript.InActiveAnimation();

        m_statePart = eState.Max;
    }

    //----------------------------------------------------
    // 状態を変更する

    /// <summary>
    /// 状態をActiveにする
    /// </summary>
    public void ActiveAnimation()
    {
        m_statePart = eState.Active;
    }

    /// <summary>
    /// 状態をSelectにする
    /// </summary>
    public void SelectAnimation()
    {
        m_statePart = eState.Select;
    }

    /// <summary>
    /// 状態をUnSelectにする
    /// </summary>
    public void UnSelectAnimation()
    {
        m_statePart = eState.UnSelect;
    }

    /// <summary>
    /// 状態をInActiveにする
    /// </summary>
    public void InActiveAnimation()
    {
        m_statePart = eState.InActive;

        // FontAnimationの中身を初期化
        m_MoveFontScript.Clear();
    }

    /// <summary>
    /// Fontアニメーションを開始
    /// </summary>
    public void FontAnimation()
    {
        m_MoveFontScript.m_proState = true;
    }

    /// <summary>
    /// コンテンツ開始
    /// </summary>
    public void ActiveContent()
    {
        // Content表示
        //        for (int i = 0; i < m_Contents.Count; i++)
        //            m_Contents[i].ActiveAnimation();
        m_ContentScript.ActiveAnimation();
    }

    /// <summary>
    /// コンテンツ開始
    /// </summary>
    public void InActiveContent()
    {
        // Content表示
        //        for (int i = 0; i < m_Contents.Count; i++)
        //            m_Contents[i].InActiveAnimation();
        m_ContentScript.InActiveAnimation();
    }
    //----------------------------------------------------
    /// <summary>
    /// Buttonをクリックした時
    /// </summary>

    public void OnClickButton()
    {
        CMenuAdministrator.m_proContentNo = m_proButtonNo;
    }

    /// <summary>
    /// マウスが接触した時
    /// </summary>
    public void OnMouseEnter()
    {
        m_imgBackGround.color = m_colHitBackGround;
    }

    /// <summary>
    /// マウスが離れた時
    /// </summary>
    public void OnMouseExit()
    {
        m_imgBackGround.color = m_colBackGround;
    }
}
