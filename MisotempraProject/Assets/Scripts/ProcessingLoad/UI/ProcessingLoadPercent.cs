using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProcessingLoad
{
    public class ProcessingLoadPercent : MonoBehaviour
    {
        Text m_text = null;


        // Start is called before the first frame update
        void Start()
        {
            m_text = GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            m_text.text = (int)ProcessingLoadManager.instance.processGauge + "%";

            switch (ProcessingLoadManager.instance.nowState)
            {
                case ProcessingLoadManager.GaugeState.Freeze:
                    m_text.color = Color.white;
                    break;
                case ProcessingLoadManager.GaugeState.Warning:
                    m_text.color = Color.red;
                    break;
                case ProcessingLoadManager.GaugeState.Caution:
                    m_text.color = Color.yellow;
                    break;
                case ProcessingLoadManager.GaugeState.Stable:
                    m_text.color = Color.green;
                    break;
            }

        }
    }

}
