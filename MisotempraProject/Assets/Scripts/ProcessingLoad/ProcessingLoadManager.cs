using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProcessingLoad
{
    public class ProcessingLoadManager : Singleton.SingletonMonoBehaviour<ProcessingLoadManager>
    {
        // ----- 処理負荷ゲージ関連
        [Header("GaugeStatus")]
        [SerializeField]
        float m_processGauge = 100.0f;    // ゲージ本体

        public float processGauge { get { return m_processGauge; } }

        [SerializeField]
        float processReduceRate = 1.0f; // 減少量乗算

        [SerializeField]
        float reduceDelayTimer = 0.0f;  // 減少までの時間（未使用）

        [SerializeField]
        TimeManagement.TimeLayer m_timeLayer = null;

        public TimeManagement.TimeLayer timeLayer { get { return m_timeLayer; } }

        float gaugeBorderCaution = 50.0f;   // StableとCautionの境界％
        float gaugeBorderWarning = 80.0f;   // CautionとWarningの境界％

        //GameObject plGauge = null;
        //Slider plSlider = null;
        //GameObject gaugeBack = null;
        //Image gaugeBackImage = null;
        //GameObject gaugeFill = null;
        //Image gaugeFillImage = null;


        //// ----- お色プリセット
        //Color white;
        //Color red;
        //Color orange;
        //Color green;

        //GameObject plPercent = null;
        //Text plPercentText = null;

        protected override void Init()
        {
            TimeManagement.TimeLayer.InitLayer(ref m_timeLayer);
        }


        public enum GaugeState
        {
            Stable,
            Caution,
            Warning,
            Freeze,
            MaxGaugeState
        }
        [SerializeField] private GaugeState m_nowState = GaugeState.Stable;

        // ========== 処理負荷ゲージ増加関数 ==========
        public void AddProcessingGauge(float num)
        {
            // reduceDelayTimer = 3.0f;
            m_processGauge += num;
            if (m_processGauge > 100.0f) m_processGauge = 100.0f;
        }

        public GaugeState nowState { get{ return m_nowState; } }

        // Start is called before the first frame update
        //void Start()
        //{
        //    plGauge = GameObject.Find("PLGauge");
        //    plSlider = plGauge.GetComponent<Slider>();
        //    gaugeBack = GameObject.Find("Gauge Background");
        //    Debug.Log("Gauge BG : " + gaugeBack);
        //    gaugeBackImage = gaugeBack.GetComponent<Image>();
        //    Debug.Log("Gauge BGImg : " + gaugeBackImage);
        //    gaugeFill = GameObject.Find("Gauge Fill");
        //    Debug.Log("Gauge Fill : " + gaugeFill);
        //    gaugeFillImage = gaugeFill.GetComponent<Image>();
        //    Debug.Log("Gauge FillImg : " + gaugeFillImage);

        //    white = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        //    red = new Color(1.0f, 0.1f, 0.1f, 1.0f);
        //    orange = new Color(1.0f, 0.5f, 0.1f, 1.0f);
        //    green = new Color(0.1f, 1.0f, 0.2f, 1.0f);

        //    plPercent = GameObject.Find("PLGaugePercent");
        //    plPercentText = plPercent.GetComponent<Text>();
        //}

        // Update is called once per frame
        void Update()
        {
            // ----- 負荷ゲージ減少 -----
            if (reduceDelayTimer <= 0.0f)
            {
                if (m_processGauge > 0.0f)
                {
                    m_processGauge -= (0.05f * processReduceRate);
                }
                if (m_processGauge < 0.0f)
                {
                    m_processGauge = 0.0f;
                }
            }
            else
            {
                reduceDelayTimer -= 0.01f;
            }


            // ----- 状態更新 -----
            if (m_processGauge == 100.0f) m_nowState = GaugeState.Freeze;
            if (m_processGauge < 100.0f && m_processGauge >= gaugeBorderWarning) m_nowState = GaugeState.Warning;
            if (m_processGauge < gaugeBorderWarning && m_processGauge >= gaugeBorderCaution) m_nowState = GaugeState.Caution;
            if (m_processGauge < gaugeBorderCaution) m_nowState = GaugeState.Stable;

            // ----- 表示更新 -----
            //plSlider.value = m_processGauge / 100;
            //plPercentText.text = (int)m_processGauge + "%";

            //switch (m_nowState)
            //{
            //    case GaugeState.Freeze:
            //        plPercentText.color = white;
            //        gaugeFillImage.color = white;
            //        break;
            //    case GaugeState.Warning:
            //        plPercentText.color = red;
            //        gaugeFillImage.color = red;
            //        break;
            //    case GaugeState.Caution:
            //        plPercentText.color = orange;
            //        gaugeFillImage.color = orange;
            //        m_timeLayer.SetTimeScale(0.2f);
            //        break;
            //    case GaugeState.Stable:
            //        plPercentText.color = green;
            //        gaugeFillImage.color = green;
            //        m_timeLayer.SetTimeScale(1.0f);
            //        break;
            //}
        }
    }

}
