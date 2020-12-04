using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTitleRogo : CAnimationController
{
    //---------------------------------------------------------------
    private enum eState
    {
        Non,
        Active,
        LScroll,
        RScroll,
        Max,
    }
    private eState m_stateRogo = eState.Non;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();    }

    // Update is called once per frame
    void Update()
    {
        switch (m_stateRogo)
        {
            case eState.Non:
                base.ChangeBoolAnimation(true, "Active");
                m_stateRogo = eState.Max;
                break;
            case eState.Active:
                Active();
                break;
            case eState.LScroll:
                LScroll();
                break;
            case eState.RScroll:
                RScroll();
                break;
            default:
                break;
        }
    }
    //---------------------------------------------------------------
    /// <summary>
    /// アクティブ関数
    /// </summary>
    private void Active()
    {
        // 現在は使ってない
        // 後々使う
//        base.ChangeBoolAnimation(true, "Active");
    }

    /// <summary>
    /// 左スクロール関数
    /// </summary>
    private void LScroll()
    {
        base.ChangeBoolAnimation(true, "Scroll");
        m_stateRogo = eState.Max;
    }

    /// <summary>
    /// 右スクロール関数
    /// </summary>
    private void RScroll()
    {
        base.ChangeBoolAnimation(false, "Scroll");
        m_stateRogo = eState.Max;
    }

    //---------------------------------------------------------------
    /// <summary>
    /// アクティブ状態
    /// </summary>
    public void ActiveAnimation()
    {
        m_stateRogo = eState.Active;
    }

    /// <summary>
    /// 左スクロール状態
    /// </summary>
    public void LScrollAnimation()
    {
        m_stateRogo = eState.LScroll;
    }

    /// <summary>
    /// 右スクロール状態
    /// </summary>
    public void RScrollAnimation()
    {
        m_stateRogo = eState.RScroll;
    }

    //---------------------------------------------------------------
    /// <summary>
    /// アニメーションが終了したかを確認
    /// </summary>
    public bool JudgeGameStart()
    {
        if (base.JudgeAnimation("Base Layer.Title_Rogo_Active"))
            return true;
        return true;
    }
}
