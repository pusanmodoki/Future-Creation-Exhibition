using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTitleGameStart : MonoBehaviour
{
    //-------------------------------------------------------------
    [SerializeField] private CMenuContentTagPart m_CotentScript = null;


    //-------------------------------------------------------------
    // 各buttonの意味
    private enum eButton
    {
        Non,
        GameStart,
        Max,
    }

    // 遷移
    private enum eState
    {
        Non,
        Button,
        Max,
    }
    private eState m_stateGameStart = eState.Non;
    //-------------------------------------------------------------
    protected bool m_flgGameStart = false;
    public bool m_proGameStart
    {
        set
        {
            m_flgGameStart = value;
        }
        get
        {
            bool flg = m_flgGameStart;
            // trueの場合、初期化を挟む
            if (flg)
                m_flgGameStart = false;
            return flg;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        // アニメーション開始
        m_CotentScript.ActiveAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_stateGameStart)
        {
            case eState.Non:
                SortButtonNo();
                m_stateGameStart++;
                break;
            case eState.Button:
                Button();
                break;
            default:
                break;
        }
    }

    //-------------------------------------------------------------
    /// <summary>
    /// button関数
    /// </summary>
    private void Button()
    {
        // buttonを押された場合
        switch (CMenuAdministrator.m_proContentNo)
        {
            // ゲームスタートボタンを押されたら
            case (int)eButton.GameStart:
                m_flgGameStart = true;

                // 選択されたアニメーション
                //                m_CotentScript.SelectAnimation();

                CMenuAdministrator.m_proContentNo = CMenuAdministrator.m_errorCode;

                m_stateGameStart = eState.Max;
                break;
            default:
                break;
        }
    }
    //-------------------------------------------------------------
    /// <summary>
    /// 判定用の数値を配布
    /// </summary>
    private void SortButtonNo()
    {
        m_CotentScript.m_proButtonNo = (int)eButton.GameStart;
    }
}
