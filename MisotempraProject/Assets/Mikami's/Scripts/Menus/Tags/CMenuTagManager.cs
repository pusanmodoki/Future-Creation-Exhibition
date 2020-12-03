using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMenuTagManager : MonoBehaviour
{
    //------------------------------------------------------------
    [SerializeField] private GameObject m_objTag;

    //------------------------------------------------------------
    [SerializeField] private CMenuHeader m_HeaderScript;
    [SerializeField] private CMenuPattern m_PatternScript;
    [SerializeField] private List<CMenuTag2> m_TagScripts;

    //------------------------------------------------------------
    // 状態遷移
    private enum eState
    {
        Non,
        Active,
        SelectStay,
        Select,
        InActive,
        Max,
    }
    private eState m_stateTag = eState.Non;
    //------------------------------------------------------------
    // 表示アニメーション用
    [SerializeField] private float m_timeActiveMax = 0.0f;

    // 経過時間
    private float m_timeStay = 0.0f;
    // 開始したtag数
    private int m_activeTagNo = 0;
    //------------------------------------------------------------
    // Tagの管理用
    protected int m_oldTagNo = CMenuAdministrator.m_errorCode;

    // 模様用
    private bool m_flgPattern = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (m_stateTag)
        {
            case eState.Non:
                break;
            case eState.Active:
                Active();
                break;

            case eState.SelectStay:
//                if (m_TagScripts[1].JudgeFontAnimation())
                    m_stateTag++;
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

    //------------------------------------------------------------
    /// <summary>
    /// Tagを表示していく
    /// </summary>
    private void Active()
    {
        // 最初に起動
        if (!m_flgPattern)
            m_PatternScript.ChangeScale(m_objTag.GetComponent<RectTransform>()); m_flgPattern = true;

         m_timeStay += Time.unscaledDeltaTime;
        // 一定時間経過で表示アニメーション開始
        if (m_timeStay >= m_timeActiveMax)
        {
            m_timeStay = 0.0f;

            m_TagScripts[m_activeTagNo].ActiveAnimation();
            m_activeTagNo++;

            // tag数分表示させたら終了
            if (m_activeTagNo >= m_TagScripts.Count)
            {
                // 初期化
                m_activeTagNo = 0;
                m_flgPattern = false;

                // tagに番号を振る
                // 振るまで選択できない
                SortTagNo();

                // 次の遷移へ
                m_stateTag++;
            }
        }
    }

    /// <summary>
    /// tagを変更する
    /// </summary>
    private void Select()
    {
        // 選択した場所が変わったら
        if (CMenuAdministrator.m_proTagNo != m_oldTagNo)
        {
            // エラーコードの場合は除外
            if (m_oldTagNo != CMenuAdministrator.m_errorCode)
                // 選択してない状態へ戻す
                m_TagScripts[m_oldTagNo].UnSelectAnimation();

            // 選択状態へ変更
            m_TagScripts[CMenuAdministrator.m_proTagNo].SelectAnimation();

            // Headerを変更する
            m_HeaderScript.SetInputFont(m_TagScripts[CMenuAdministrator.m_proTagNo].GetTagName());

            // 保存
            m_oldTagNo = CMenuAdministrator.m_proTagNo;
        }
    }

    /// <summary>
    /// Tagを非表示にしていく
    /// </summary>
    private void InActive()
    {
        CMenuAdministrator.m_proTagNo = 0;
        m_oldTagNo = CMenuAdministrator.m_errorCode;

        for (int i=0;i<m_TagScripts.Count;i++)
        {
            m_TagScripts[i].InActiveAnimation();
        }

        
        m_HeaderScript.Clear();

        // 模様を初期化 (見えなくする)
        m_PatternScript.Clear();

        m_stateTag = eState.Max;
    }

    //------------------------------------------------------------
    /// <summary>
    /// Active状態に
    /// </summary>
    public void ActiveAnimation()
    {
        m_stateTag = eState.Active;
    }

    /// <summary>
    /// InActive状態に
    /// </summary>
    public void InActiveAnimation()
    {
        m_stateTag = eState.InActive;
    }

    //------------------------------------------------------------
    /// <summary>
    /// Tagの配置順を渡す
    /// </summary>
    private void SortTagNo()
    {
        for(int i = 0; i < m_TagScripts.Count; i++)
        {
            m_TagScripts[i].m_proButtonNo = i;
        }
    }
}
