using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CMenuContentTag : MonoBehaviour
{
    //--------------------------------------------------------------
    public CMenuExplanation m_ExplanationScript;
    public List<CMenuContentTagPart> m_PartScripts;

    //--------------------------------------------------------------
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
    //--------------------------------------------------------------
    // Active関数内で使用
    [SerializeField] private float m_timeActiveMax = 0.0f;
    private float m_timeStay = 0.0f;

    private int m_ActiveNo = 0;
    //--------------------------------------------------------------
    private int m_oldContentNo = CMenuAdministrator.m_errorCode;

    // Start is called before the first frame update
    void Start()
    {
        InActiveButton();
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

    //--------------------------------------------------------------
    // switch内で使用

    /// <summary>
    /// Init関数
    /// </summary>
    private void Init()
    {
        m_ActiveNo = 0;
        m_timeStay = 0.0f;

        m_oldContentNo = CMenuAdministrator.m_errorCode;
        CMenuAdministrator.m_proContentNo = 0;

        InActiveButton();

        for (int i = 0; i < m_PartScripts.Count; i++)
        {
            m_PartScripts[i].Init();
        }
    }

    /// <summary>
    /// Active関数
    /// </summary>
    private void Active()
    {
        ActiveButton();

        m_timeStay += Time.unscaledDeltaTime;
        if (m_timeStay >= m_timeActiveMax)
        {
            m_timeStay = 0.0f;

            // Active状態に
            m_PartScripts[m_ActiveNo].ActiveAnimation();
            m_PartScripts[m_ActiveNo].m_proButtonNo = m_ActiveNo;

            m_ActiveNo++;
            // すべて出し終えたら終了
            if (m_ActiveNo >= m_PartScripts.Count)
            {
                m_ActiveNo = 0;
                m_stateContent++;
            }
        }
    }

    /// <summary>
    /// Select関数
    /// </summary>
    private void Select()
    {
        if (m_oldContentNo != CMenuAdministrator.m_proContentNo)
        {
            if (m_oldContentNo != CMenuAdministrator.m_errorCode)
                m_PartScripts[m_oldContentNo].UnSelectAnimation();

            m_PartScripts[CMenuAdministrator.m_proContentNo].SelectAnimation();

            m_oldContentNo = CMenuAdministrator.m_proContentNo;
        }
    }

    /// <summary>
    /// InActive関数
    /// </summary>
    private void InActive()
    {
        for (int i = 0; i < m_PartScripts.Count; i++)
        {
            m_PartScripts[i].InActiveAnimation();
        }

        Init();

        m_stateContent = eState.Non;
    }

    //----------------------------------------------------
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
        m_stateContent = eState.InActive;
    }

    //----------------------------------------------------
    // buttonが重ならないように移動

    /// <summary>
    /// buttonを使えるようにする
    /// </summary>
    private void ActiveButton()
    {
        this.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0.0f, 0.0f, 0.0f);
    }

    /// <summary>
    /// buttonを使えないようにする
    /// </summary>
    private void InActiveButton()
    {
        this.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(-2000.0f, 0.0f, 0.0f);
    }
}
