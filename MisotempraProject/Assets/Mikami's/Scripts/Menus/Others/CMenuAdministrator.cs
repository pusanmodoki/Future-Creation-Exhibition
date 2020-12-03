using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMenuAdministrator : MonoBehaviour
{
    public const int m_errorCode = 9999;
    //---------------------------------------------------------
    // Tagの管理用
    protected static int m_TagNo = m_errorCode;
    public static int m_proTagNo { set; get; }

    //---------------------------------------------------------
    // Contentの管理用
    protected static int m_ContentNo = m_errorCode;
    public static int m_proContentNo { set; get; }

    //---------------------------------------------------------
    // Start is called before the first frame update
    public void Start()
    {
        m_TagNo = m_errorCode;
        m_ContentNo = m_errorCode;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
