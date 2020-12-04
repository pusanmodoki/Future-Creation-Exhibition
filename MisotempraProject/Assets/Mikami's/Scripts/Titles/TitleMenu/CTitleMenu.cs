using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTitleMenu : MonoBehaviour
{
    //---------------------------------------------------------------------------
    [SerializeField] private List<CMenuContentTagPart> m_ContentScripts;

    //---------------------------------------------------------------------------
    // 各buttonの役割
    private enum eButton
    {
        Non,
        Begin,
        Continue,
        Option,
        Max,
    }

    [SerializeField] private string[] m_NextSceneName = new string[((int)eButton.Max) - 1];

    // 状態遷移
    private enum eState
    {
        Non,
        Active,
        Button,
        Max,
    }
    private eState m_stateMenu = eState.Non;

    //---------------------------------------------------------------------------
    // 表示の際
    [SerializeField] private float m_timeActive = 0.0f;
    private float m_timeStay = 0.0f;

    private int m_ActiveNo = 0;
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
                SortButtonNo();
                m_stateMenu = eState.Max;
                break;
            case eState.Active:
                Active();
                break;
            case eState.Button:
                Button();
                break;
            default:
                break;
        }
    }
    //---------------------------------------------------------------------------
    /// <summary>
    /// Active関数
    /// </summary>
    private void Active()
    {
        m_timeStay += Time.unscaledDeltaTime;
        // 一定時間経過後
        if (m_timeStay >= m_timeActive)
        {
            m_timeStay = 0.0f;

            // 表示アニメーションを開始
            m_ContentScripts[m_ActiveNo].ActiveAnimation();

            m_ActiveNo++;
            // 全部、開始したら
            if (m_ActiveNo >= m_ContentScripts.Count)
            {
                m_ActiveNo = 0;

                m_stateMenu++;
            }
        }
    }

    /// <summary>
    /// Button関数
    /// </summary>
    private void Button()
    {
        if (CMenuAdministrator.m_proContentNo != CMenuAdministrator.m_errorCode)
        {
            // 各ボタン
            switch (CMenuAdministrator.m_proContentNo)
            {
                case (int)eButton.Begin:
                    CSceneManager.SceneChange(m_NextSceneName[(int)eButton.Begin - 1]);
                    break;
                case (int)eButton.Continue:
                    break;
                case (int)eButton.Option:
                    break;
            }

            CMenuAdministrator.m_proContentNo = CMenuAdministrator.m_errorCode;

            m_stateMenu = eState.Max;
        }
    }

    //---------------------------------------------------------------------------
    /// <summary>
    /// buttonの役割を割り振る
    /// </summary>
    private void SortButtonNo()
    {
        for(int i = 0; i < m_ContentScripts.Count; i++)
        {
            m_ContentScripts[i].m_proButtonNo = (int)(eButton.Non) + i + 1;
        }
    }

    //---------------------------------------------------------------------------
    /// <summary>
    /// Active状態にする
    /// </summary>
    public void ActiveAnimation()
    {
        m_stateMenu = eState.Active;
    }
}
