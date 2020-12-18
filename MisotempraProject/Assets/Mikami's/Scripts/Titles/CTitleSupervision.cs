using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTitleSupervision : MonoBehaviour
{
    //---------------------------------------------------------------
    [SerializeField] private CTitleRogo m_RogoScript = null;
    [SerializeField] private CTitleGameStart m_GameStartScript = null;
    [SerializeField] private CTitleMenu m_MenuScript = null;

    //---------------------------------------------------------------
    private enum eState
    {
        Non,
        GameStart,
        MenuStay,
        Menu,
        Max,
    }
    private eState m_stateTitle = eState.Non;

    //---------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_stateTitle)
        {
            case eState.Non:
                m_stateTitle++;
                break;
            case eState.GameStart:
                GameStart();
                break;
            case eState.MenuStay:
                MenuStay();
                break;
            case eState.Menu:
                Menu();
                break;
            default:
                break;
        }
    }

    //---------------------------------------------------------------
    /// <summary>
    /// ゲームスタート関数
    /// </summary>
    private void GameStart()
    {
        // buttonが押されたら
        if (m_GameStartScript.m_proGameStart)
        {
            // ロゴを流す
            m_RogoScript.LScrollAnimation();
            m_stateTitle++;
        }
    }

    /// <summary>
    /// MenuStay関数
    /// </summar-y>
    private void MenuStay()
    {
        // scrollし終えたら
        if(m_RogoScript.JudgeGameStart())
        {
            m_MenuScript.ActiveAnimation();
            m_stateTitle++;
        }
    }

    /// <summary>
    /// Menu関数
    /// </summary>
    private void Menu()
    {

    }

    //---------------------------------------------------------------
    // 
}
