using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMenuContent2 : MonoBehaviour
{
    //-------------------------------------------------------------------------
    //
    [SerializeField] private List<CMenuContent_Basic> m_BasicScripts = null;   // 通常
    [SerializeField] private List<CMenuContent_Model> m_ModelScripts = null;   // モデル

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
        // 通常
        for (int i = 0; i < m_BasicScripts.Count; i++)
            m_BasicScripts[i].ActiveAnimation();

        // モデル付き
        for (int i = 0; i < m_ModelScripts.Count; i++)
            m_ModelScripts[i].ActiveAnimation();

        m_state = eState.Max;
    }

    /// <summary>
    /// InActive関数
    /// </summary>
    private void InActive()
    {
        // 通常
        for (int i = 0; i < m_BasicScripts.Count; i++)
            m_BasicScripts[i].InActiveAnimation();

        // モデル付き
        for (int i = 0; i < m_ModelScripts.Count; i++)
            m_ModelScripts[i].InActiveAnimation();

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
