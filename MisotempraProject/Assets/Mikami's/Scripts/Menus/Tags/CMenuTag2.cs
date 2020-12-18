using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CMenuTag2 : CAnimationController
{
    //-----------------------------------------------------
    [SerializeField] private CMoveFont m_MoveFontScript = null;

    //-----------------------------------------------------
    [SerializeField] private Image m_imgBackGround = null;

    [SerializeField] private Color32 m_colBackGround = default;
    [SerializeField] private Color32 m_colHitBackGround = default;
    //-----------------------------------------------------
    // 状態遷移
    private enum eState
    {
        Non,
        Active,
        Active_Font,
        Select,
        UnSelect,
        InActive,
        Max,
    }
    private eState m_stateTag = eState.Non;
    //-----------------------------------------------------
    // 自分の配置されている番号
    public int m_proButtonNo { set; get; }

    private bool m_flgFontActive = false;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        // errorコード
        m_proButtonNo = CMenuAdministrator.m_errorCode;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_stateTag)
        {
            case eState.Non:
                break;
            // tagの表示アニメーションを開始する
            case eState.Active:
                Active();
                break;
            // 文字を出す
            case eState.Active_Font:
                Active_Font();
                break;

            // 選択されている
            case eState.Select:
                Select();
                break;

            // 選択を外した           
            case eState.UnSelect:
                UnSelect();
                break;

            // tagの非表示アニメーションを開始
            case eState.InActive:
                InActive();
                break;
            // その他
            default:
                break;
        }
    }
        

    //-----------------------------------------------------
    /// <summary>
    /// Active関数
    /// </summary>
    private void Active()
    {
        base.ChangeBoolAnimation(true, "Active");
        m_stateTag++;
    }

    /// <summary>
    /// Active_Font関数
    /// </summary>
    private void Active_Font()
    {
        // 表示アニメーションが終わったら文字を出す
        if (base.JudgeAnimation("Base Layer.MenuTag_Active"))
        {
            m_MoveFontScript.m_proState = true;
            m_stateTag = eState.Max;

            // 文字が出現する
            m_flgFontActive = true;
        }
    }

    /// <summary>
    /// Select関数
    /// </summary>
    private void Select()
    {
        base.ChangeBoolAnimation(true, "Select");
        m_stateTag = eState.Max;
    }

    /// <summary>
    /// UnSelect関数
    /// </summary>
    private void UnSelect()
    {
        base.ChangeBoolAnimation(false, "Select");
        m_stateTag = eState.Max;
    }

    /// <summary>
    /// InActive関数
    /// </summary>
    private void InActive()
    {
        base.ChangeBoolAnimation(false, "Active");
        base.ChangeBoolAnimation(false, "Select");
        m_stateTag++;

        // 初期化
        m_flgFontActive = false;
        m_MoveFontScript.Clear();
    }



    //-----------------------------------------------------
    /// <summary>
    /// Activeに状態を変更
    /// </summary>
    public void ActiveAnimation()
    {
        m_stateTag = eState.Active;
    }

    /// <summary>
    /// InActiveに状態を変更
    /// </summary>
    public void InActiveAnimation()
    {
        m_stateTag = eState.InActive;
    }

    /// <summary>
    /// Selectに状態を変更
    /// </summary>
    public void SelectAnimation()
    {
        m_stateTag = eState.Select;
    }

    /// <summary>
    /// UnSelectに状態を変更
    /// </summary>
    public void UnSelectAnimation()
    {
        m_stateTag = eState.UnSelect;
    }

    //-----------------------------------------------------
    /// <summary>
    /// フォントアニメーションを終了したか
    /// </summary>
    /// <return> 終了:true / 処理中:false </return>
    public bool JudgeFontAnimation()
    {
        return m_MoveFontScript.JudgeAnimation();
    }

    //-----------------------------------------------------
    /// <summary>
    /// Buttonをクリックした時
    /// </summary>
    public void OnClickButton()
    {
        // 文字が表示してから押せるようになる
        if(m_flgFontActive)
            CMenuAdministrator.m_proTagNo = m_proButtonNo;        
    }

    /// <summary>
    /// Mouseが接触したら
    /// </summary>
    public void OnMouseEnter()
    {
        m_imgBackGround.color = m_colHitBackGround;
    }

    /// <summary>
    /// マウスが外れたら
    /// </summary>
    public void OnMouseExit()
    {
        m_imgBackGround.color = m_colBackGround;
    }

    //-----------------------------------------------------
    /// <summary>
    /// tagの名前を返す
    /// </summary>
    /// <return> 現在のタグ名 </return>
    public string GetTagName()
    {
        return m_MoveFontScript.GetInputFont();
    }
}