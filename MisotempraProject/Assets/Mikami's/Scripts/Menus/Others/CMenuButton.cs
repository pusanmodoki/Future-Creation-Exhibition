using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMenuButton : MonoBehaviour
{
//    protected int m_ButtonNo;
    public int m_proButtonNo { set; get; }

    // Start is called before the first frame update
    void Start()
    {
        m_proButtonNo = CMenuAdministrator.m_errorCode;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TagSelect()
    {

    }

    public void TagUnSelect()
    {

    }
}
