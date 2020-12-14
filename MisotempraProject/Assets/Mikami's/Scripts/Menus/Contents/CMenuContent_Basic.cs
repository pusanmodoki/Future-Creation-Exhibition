using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMenuContent_Basic : CAnimationController
{

    //-------------------------------------------------------------------------
    //
    private enum eState
    {
        Non,
        Active,
        InActive,
        Max,
    }
    private eState m_state = eState.Non;
    //-------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        {
            case eState.Non:
                break;
            case eState.Active:
                Active();
                break;
            case eState.InActive:
                InActive();
                break;
            default:
                break;
        }
    }

    //---------------------------------------------------------------
    /// <summary>
    /// Active関数
    /// </summary>
    private void Active()
    {
        base.ChangeBoolAnimation(true, "Active");
        m_state = eState.Max;
    }

    /// <summary>
    /// InActive関数
    /// </summary>
    private void InActive()
    {
        base.ChangeBoolAnimation(false, "Active");
        m_state = eState.Max;
    }

    //---------------------------------------------------------------
    /// <summary>
    /// Active状態にする
    /// </summary>
    public void ActiveAnimation()
    {
        m_state = eState.Active;
    }

    /// <summary>
    /// InActive状態にする
    /// </summary>
    public void InActiveAnimation()
    {
        m_state = eState.InActive;
    }
}
