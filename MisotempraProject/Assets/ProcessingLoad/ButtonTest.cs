using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTest : MonoBehaviour
{
    float plusProcessGauge = 10.0f;

    GameObject plGaugeCon = null;
    ProcessingLoad.ProcessingLoadManager plManagerScr = null;

    // Start is called before the first frame update
    void Start()
    {
        plGaugeCon = GameObject.Find("PLManager");
        plManagerScr = plGaugeCon.GetComponent<ProcessingLoad.ProcessingLoadManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PressAddButton()
    {
        plManagerScr.AddProcessingGauge(plusProcessGauge);
    }
}
