using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMenuExplanation : CAnimationController
{
    //-----------------------------------------------------------------
    //
    [SerializeField] private CMoveFont m_MoveFontScript;

    //-----------------------------------------------------------------
    //
    private enum eState
    {
        Non,
        Active,
        Font,
        InActive,
        Max,
    }
    private eState m_stateExplanation = eState.Non;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_stateExplanation)
        {
            case eState.Non:
                break;
            case eState.Active:
                Active();
                break;
            case eState.Font:
                Font();
                break;
            case eState.InActive:
                InActive();
                break;
            default:
                break;
        }
    }

    //-----------------------------------------------------------------
    //
    /// <summary>
    /// Active状態にする
    /// </summary>
    public void ActiveAnimation()
    {
        m_stateExplanation = eState.Active;
    }

    /// <summary>
    /// InActive状態にする
    /// </summary>
    public void InActiveAnimation()
    {
        m_stateExplanation = eState.InActive;
    }

    //-----------------------------------------------------------------
    //
    /// <summary>
    /// ActiveAnimationを実行
    /// </summary>
    private void Active()
    {
        base.ChangeBoolAnimation(true, "Active");

        m_stateExplanation = eState.Font;
    }

    /// <summary>
    /// FontAnimationを実行
    /// </summary>
    private void Font()
    {
        if (base.JudgeAnimation("Base Layer.Menu_Explanation"))
        {
            m_MoveFontScript.m_proState = true;
            m_stateExplanation = eState.Max;
        }
    }

    /// <summary>
    /// InActiveAnimationを実行
    /// </summary>
    private void InActive()
    {
        base.ChangeBoolAnimation(false, "Active");
        m_MoveFontScript.Clear();
        m_stateExplanation = eState.Max;
    }
}
