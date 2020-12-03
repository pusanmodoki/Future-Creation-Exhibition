using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMenuBackGround : CAnimationController
{
    //-----------------------------------------------------------
    [SerializeField] private List<GameObject> m_objUpBars;
    [SerializeField] private List<GameObject> m_objDownBars;

    //-----------------------------------------------------------
    private enum eState
    {
        Non,
        Active,
        BarActive,
        InActive,
        Max,
    }
    private eState m_stateBG = eState.Non;
    //-----------------------------------------------------------
    private int m_BarNo = 0;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        InActive();
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_stateBG)
        {
            case eState.Non:
                break;
            case eState.Active:
                Active();
                break;
            case eState.BarActive:
                BarActive();
                break;
            case eState.InActive:
                InActive();
                break;
            default:
                break;
        }
    }

    //-----------------------------------------------------
    /// <summary>
    /// Activeに状態を変更
    /// </summary>
    public void ActiveAnimation()
    {
        m_stateBG = eState.Active;
    }

    /// <summary>
    /// InActiveに状態を変更
    /// </summary>
    public void InActiveAnimation()
    {
        m_stateBG = eState.InActive;
    }

    //-----------------------------------------------------
    /// <summary>
    /// Active関数
    /// </summary>
    private void Active()
    {
        base.ChangeBoolAnimation(true, "Active");
        m_stateBG++;
    }

    /// <summary>
    /// バーをだす関数
    /// </summary>
    private void BarActive()
    {
        // 右から
        m_objUpBars[m_BarNo].SetActive(true);
        m_objDownBars[m_BarNo].SetActive(true);

//        m_objUpBars[m_objUpBars.Count-m_BarNo].SetActive(true);
//        m_objDownBars[m_objDownBars.Count - m_BarNo].SetActive(true);

        m_BarNo--;
        if (m_BarNo < 0)
        {
            m_stateBG = eState.Max;
        }
    }

    /// <summary>
    /// InActive関数
    /// </summary>
    private void InActive()
    {
        base.ChangeBoolAnimation(false, "Active");

        m_BarNo = m_objUpBars.Count - 1;


        for (int i = 0; i < m_objUpBars.Count; i++)
        {
            m_objUpBars[i].SetActive(false);
            m_objDownBars[i].SetActive(false);
        }
        m_stateBG = eState.Non;
    }
}
