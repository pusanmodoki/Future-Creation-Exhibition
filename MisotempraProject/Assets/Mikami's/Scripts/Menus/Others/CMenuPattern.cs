using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CMenuPattern : MonoBehaviour
{
    //-----------------------------------------------
    [SerializeField] private Text m_textPattern;

    private bool m_flgAnimation = false;
    private float m_scalePattern = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        IfNeededAnimation();
    }

    //-----------------------------------------------------------------------------
    // 
    /// <summary>
    /// サイズを変更する　(1つ用)
    /// </summary>
    /// <param name="Trans">一番上のオブジェクト</param>
    public void ChangeScale(RectTransform Trans)
    {
        Clear();

        // 位置をずらす
        Vector3 recPos = this.gameObject.GetComponent<RectTransform>().localPosition;
        recPos.y = Trans.localPosition.y;
        this.gameObject.GetComponent<RectTransform>().localPosition = recPos;

        // ボタンの厚みを加算
        m_scalePattern = Trans.sizeDelta.y;

        // アニメーション開始
        m_flgAnimation = true;
    }

    /// <summary>
    /// サイズを変更する (複数用)
    /// </summary>
    /// <param name="topTrans">一番上のオブジェクト</param>
    /// <param name="bottomTrans">一番下のオブジェクト</param>
    public void ChangeScale(RectTransform topTrans, RectTransform bottomTrans)
    {
        Clear();

        // ① : button間の数値を算出
        m_scalePattern = Mathf.Abs(topTrans.localPosition.y) + Mathf.Abs(bottomTrans.localPosition.y) ;

        // ①と一番上のボタンを使用して、中間地点を算出
        Vector3 recPos = this.gameObject.GetComponent<RectTransform>().localPosition;
        recPos.y = topTrans.localPosition.y - (m_scalePattern * 0.5f);
        this.gameObject.GetComponent<RectTransform>().localPosition = recPos;

        // ①にボタンの厚みを加算
        m_scalePattern += (topTrans.sizeDelta.y * 0.5f + bottomTrans.sizeDelta.y * 0.5f);

        // アニメーション開始
        m_flgAnimation = true;
    }

    //-----------------------------------------------------------------------------
    /// <summary>
    /// 伸ばすアニメーション
    /// </summary>
    private void IfNeededAnimation()
    {
        // 終了したらおわりんご
        if (m_flgAnimation)
        {
            Vector2 recScale = this.gameObject.GetComponent<RectTransform>().sizeDelta;
            // 現在,20frame後に最大
            recScale.y += m_scalePattern * 0.05f;

            this.gameObject.GetComponent<RectTransform>().sizeDelta = recScale;
            // 指定したサイズを超えたら終了
            if (this.gameObject.GetComponent<RectTransform>().sizeDelta.y >= m_scalePattern)
            {
                m_flgAnimation = false;
            }
        }
    }

    // 初期化
    public void Clear()
    {
        // 最大サイズを初期化
        m_scalePattern = 0.0f;

        // y軸初期化
        Vector2 recScale = this.gameObject.GetComponent<RectTransform>().sizeDelta;
        recScale.y = 0.0f;
        this.gameObject.GetComponent<RectTransform>().sizeDelta = recScale;
    }
}
