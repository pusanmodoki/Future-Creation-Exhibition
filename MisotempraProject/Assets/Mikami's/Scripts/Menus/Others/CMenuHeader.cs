using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CMenuHeader : MonoBehaviour
{
    [SerializeField] private CMoveFont m_whiteMoveFontScript = null;
    [SerializeField] private CMoveFont m_blackMoveFontScript = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        IfNeededFontAnimation();
    }

    //------------------------------------------------------
    /// <summary>
    /// Headerに現在のタグ名を渡す
    /// </summary>
    public void SetInputFont(string input)
    {
        // タグ名を入力
        m_whiteMoveFontScript.SetInputFont(input);
        m_blackMoveFontScript.SetInputFont(input);

        // アニメーション開始
        m_whiteMoveFontScript.m_proState = true;
        m_blackMoveFontScript.m_proState = true;
    }

    /// <summary>
    /// Textの中身を初期化する
    /// </summary>
    public void Clear()
    {
        m_blackMoveFontScript.Clear();
        m_whiteMoveFontScript.Clear();
    }

    //------------------------------------------------------
    /// <summary>
    /// Select関数
    /// </summary>
    private void IfNeededFontAnimation()
    {

    }
}
