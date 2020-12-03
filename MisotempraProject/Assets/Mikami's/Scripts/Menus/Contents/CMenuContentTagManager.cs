using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMenuContentTagManager : MonoBehaviour
{
    //-----------------------------------------------------------------
    [SerializeField] private CMenuPattern m_PatternScript;
    [SerializeField] private List<CMenuContentTag> m_ContentScripts;
    private List<CMenuExplanation> m_ExplanationScripts = new List<CMenuExplanation>();

    //-----------------------------------------------------------------
    // 状態遷移
    private enum eState
    {
        Non,
        Active,
        Select,
        InActive,
        Max,
    }

    private eState m_stateContent = eState.Non;
    //-----------------------------------------------------------------
    private int m_oldTagNo = 0;

    // Start is called before the first frame update
    void Start()
    {
        // tagの説明文を確保
        for (int i = 0; i < m_ContentScripts.Count; i++)
            m_ExplanationScripts.Add(m_ContentScripts[i].m_ExplanationScript);
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_stateContent)
        {
            case eState.Non:
                break;
            case eState.Active:
                Active();
                break;
            case eState.Select:
                Select();
                break;
            case eState.InActive:
                InActive();
                break;
            default:
                break;
        }
    }

    //-----------------------------------------------------------------
    // switch内で使用

    /// <summary>
    /// Active関数
    /// </summary>
    private void Active()
    {
        m_ContentScripts[CMenuAdministrator.m_proTagNo].ActiveAnimation();

        m_ExplanationScripts[CMenuAdministrator.m_proTagNo].ActiveAnimation();
        m_stateContent++;

        m_PatternScript.ChangeScale(
            m_ContentScripts[CMenuAdministrator.m_proTagNo].m_PartScripts[0].gameObject.GetComponent<RectTransform>(),
            m_ContentScripts[CMenuAdministrator.m_proTagNo].m_PartScripts[m_ContentScripts[CMenuAdministrator.m_proTagNo].m_PartScripts.Count - 1].gameObject.GetComponent<RectTransform>());
    }

    /// <summary>
    /// Select関数
    /// </summary>
    private void Select()
    {
        // tagが変更された場合
        if (m_oldTagNo != CMenuAdministrator.m_proTagNo)
        {
            m_stateContent++;
        }
    }

    /// <summary>
    /// InActive関数
    /// </summary>
    private void InActive()
    {
        m_ContentScripts[m_oldTagNo].InActiveAnimation();
        //        m_ExplanationScripts[CMenuAdministrator.m_proTagNo].InActiveAnimation();
        m_ExplanationScripts[m_oldTagNo].InActiveAnimation();
        
        m_oldTagNo = CMenuAdministrator.m_proTagNo;

        m_stateContent = eState.Active;        
    }

    //-----------------------------------------------------------------
    // 状態を変更する

    /// <summary>
    /// 状態をActiveにする
    /// </summary>
    public void ActiveAnimation()
    {
        m_stateContent = eState.Active;
    }

    /// <summary>
    /// 状態をInActiveにする
    /// </summary>
    public void InActiveAnimation()
    {
        m_ContentScripts[CMenuAdministrator.m_proTagNo].InActiveAnimation();

        m_ExplanationScripts[CMenuAdministrator.m_proTagNo].InActiveAnimation();
        // 模様を初期化 (見えなくする)
        m_PatternScript.Clear();

        m_stateContent = eState.Non;
    }
}
