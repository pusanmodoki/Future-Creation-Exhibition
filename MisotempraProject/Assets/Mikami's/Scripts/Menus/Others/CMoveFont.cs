using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CMoveFont : MonoBehaviour
{
    // フォントに書き込み文字列
    [SerializeField] private string m_inputFont;
    [SerializeField] private Text m_text;

    //-------------------------------------------------------------
    protected bool m_flgState = false;
    public bool m_proState
    {
        // 実行してなかったら、実行
        set
        {
//            if (!m_flgState&&value)
            if (value)
                Clear();
            m_flgState = value;
        }
        // アニメーション中、true
        get
        {
            if ((m_pushFontNo > 0) && m_flgState)
                return true;
            return false;
        }
    }

    //-------------------------------------------------------------
    // 出力する文字列
    private string m_outputFont = "";
    // 現在の文字番号
    private int m_pushFontNo = 0;

    // Start is called before the first frame update
    void Start()
    {
        Clear();
    }

    // Update is called once per frame
    void Update()
    {
        IfNeededFontAnimation();
    }

    //-------------------------------------------------------------
    /// <summary>
    /// 入力する文字列を変数に渡す
    /// </summary>
    public void SetInputFont(string input)
    {
        m_inputFont = input;
    }

    /// <summary>
    /// 入力する文字列を返す
    /// </summary>
    public string GetInputFont()
    {
        return m_inputFont;
    }

    /// <summary>
    /// 出力する文字列を返す
    /// </summary>
    public string GetOutputFont()
    {
        return m_outputFont;
    }

    //-------------------------------------------------------------
    /// <summary>
    /// フォントアニメーションが終了したか、判定
    /// </summary>
    /// <return> 終了:true / 処理中:false </return>
    public bool JudgeAnimation()
    {
        if (!m_flgState && m_outputFont.Length > 0)
            return true;
        return false;
    }

    //-------------------------------------------------------------
    /// <summary>
    /// 呼び出されたら、文字のアニメーション開始
    /// </summary>
    private void IfNeededFontAnimation()
    {
        if (m_flgState)
        {
            string n_outputFont = m_outputFont;
            // Output内の最後尾を一文字ずつ更新
            n_outputFont += m_inputFont.Substring(m_pushFontNo, 1);
            m_text.text = n_outputFont;
            m_pushFontNo++;
            //----------------------------------------------------------
            // 文字カウントがinputの文字数分以上になる
            if (m_pushFontNo >= m_inputFont.Length)
            {
                m_pushFontNo = 0;
                // 一文字ずつ確定させていく
                m_outputFont += m_inputFont[m_outputFont.Length];

                // すべて確定したら終了
                if (m_outputFont.Length >= m_inputFont.Length)
                    m_proState = false;
            }
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Clear()
    {
        // フラグを初期化
        m_proState = false;
        
        // データを初期化
        m_pushFontNo = 0;
        m_outputFont = "";
        m_text.text = m_outputFont;
    }
}
