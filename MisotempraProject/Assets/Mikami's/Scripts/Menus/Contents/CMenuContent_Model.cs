using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMenuContent_Model : CAnimationController
{

    //-------------------------------------------------------------------------
    // 状態遷移
    private enum eState
    {
        Non,
        Active,
        InActive,
        Max,
    }
    private eState m_state = eState.Non;

    //-------------------------------------------------------------------------
    //
    [SerializeField] private string m_ObjName = "";     // 検索名
    private GameObject m_Obj = null;                    // 対応オブジェクト

    //-------------------------------------------------------------------------
    // 


    //-------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        SearchObject();
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        {
            case eState.Non:
                break;
            case eState.Active:
                Active();
                break;
            case eState.InActive:
                InActive();
                break;
            default:
                break;
        }
    }

    //---------------------------------------------------------------
    /// <summary>
    /// Active関数
    /// </summary>
    private void Active()
    {
        base.ChangeBoolAnimation(true, "Active");
        m_state = eState.Max;
    }

    /// <summary>
    /// InActive関数
    /// </summary>
    private void InActive()
    {
        base.ChangeBoolAnimation(false, "Active");
        m_state = eState.Max;
    }

    /// <summary>
    /// 指定オブジェクトを探す関数
    /// </summary>
    private void SearchObject()
    {
        //-----------------------------------------------------------
        // 検索
        GameObject n_SearchObj = GameObject.Find(m_ObjName);
        // 検索に引っかからない場合
        if (n_SearchObj == null)
            return;
        // 生成
        m_Obj = Instantiate(n_SearchObj, new Vector3(0f, 0f, 0f), new Quaternion(0f, 0f, 0f, 1f));

        m_Obj.transform.name = m_Obj.transform.name + "+Clone";
        //-----------------------------------------------------------
        // 子供にする
        m_Obj.transform.parent = this.gameObject.transform;
    }

    //---------------------------------------------------------------
    /// <summary>
    /// Active状態にする
    /// </summary>
    public void ActiveAnimation()
    {
        m_state = eState.Active;
    }

    /// <summary>
    /// InActive状態にする
    /// </summary>
    public void InActiveAnimation()
    {
        m_state = eState.InActive;
    }
}
