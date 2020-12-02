using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProcessingLoad
{
    public class ProcessingLoadGauge : MonoBehaviour
    {
        [SerializeField]
        private Slider m_slider = null;
        [SerializeField]
        private Image m_fillImage = null;

        // Update is called once per frame
        void Update()
        {
            // gauge update
            m_slider.value = ProcessingLoadManager.instance.processGauge / 100;

            // fill update
            switch (ProcessingLoadManager.instance.nowState)
            {
                case ProcessingLoadManager.GaugeState.Freeze:
                    m_fillImage.color = Color.white;
                    break;
                case ProcessingLoadManager.GaugeState.Warning:
                    m_fillImage.color = Color.red;
                    break;
                case ProcessingLoadManager.GaugeState.Caution:
                    m_fillImage.color = Color.yellow;
                    break;
                case ProcessingLoadManager.GaugeState.Stable:
                    m_fillImage.color = Color.green;
                    break;
            }

        }
    }

}
