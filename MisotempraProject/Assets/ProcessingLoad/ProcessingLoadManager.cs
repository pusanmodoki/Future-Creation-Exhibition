using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProcessingLoadManager : MonoBehaviour
{
    // ----- 処理負荷ゲージ関連
    [Header("GaugeStatus")]
    [SerializeField]
    float processGauge = 100.0f;    // ゲージ本体

    [SerializeField]
    float processReduceRate = 1.0f; // 減少量乗算

    [SerializeField]
    float reduceDelayTimer = 0.0f;  // 減少までの時間（未使用）

    [SerializeField]
    TimeManagement.TimeLayer m_timeLayer = null;

    public TimeManagement.TimeLayer timeLayer { get { return m_timeLayer; } }

    public static ProcessingLoadManager instance { get; private set; } = null;

    float gaugeBorderCaution = 50.0f;   // StableとCautionの境界％
    float gaugeBorderWarning = 80.0f;   // CautionとWarningの境界％

    GameObject plGauge = null;
    Slider plSlider = null;
    GameObject gaugeBack = null;
    Image gaugeBackImage = null;
    GameObject gaugeFill = null;
    Image gaugeFillImage = null;

    // ----- お色プリセット
    Color white;
    Color red;
    Color orange;
    Color green;

    GameObject plPercent = null;
    Text plPercentText = null;

    public enum gaugeState
    {
        Stable,
        Caution,
        Warning,
        Freeze,
        MaxGaugeState
    }
    [SerializeField] private gaugeState nowState = gaugeState.Stable;

    // ========== 処理負荷ゲージ増加関数 ==========
    public void AddProcessingGauge(float num)
    {
        // reduceDelayTimer = 3.0f;
        processGauge += num;
        if (processGauge > 100.0f) processGauge = 100.0f;
    }

    public gaugeState getGaugeState() { return nowState; }

    private void Awake()
    {
        TimeManagement.TimeLayer.InitLayer(ref m_timeLayer);
        instance = this;

    }
    // Start is called before the first frame update
    void Start()
    {
        plGauge = GameObject.Find("PLGauge");
        plSlider = plGauge.GetComponent<Slider>();
        gaugeBack = GameObject.Find("Gauge Background");
        Debug.Log("Gauge BG : " + gaugeBack);
        gaugeBackImage = gaugeBack.GetComponent<Image>();
        Debug.Log("Gauge BGImg : " + gaugeBackImage);
        gaugeFill = GameObject.Find("Gauge Fill");
        Debug.Log("Gauge Fill : " + gaugeFill);
        gaugeFillImage = gaugeFill.GetComponent<Image>();
        Debug.Log("Gauge FillImg : " + gaugeFillImage);

        white = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        red = new Color(1.0f, 0.1f, 0.1f, 1.0f);
        orange = new Color(1.0f, 0.5f, 0.1f, 1.0f);
        green = new Color(0.1f, 1.0f, 0.2f, 1.0f);

        plPercent = GameObject.Find("PLGaugePercent");
        plPercentText = plPercent.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // ----- 負荷ゲージ減少 -----
        if (reduceDelayTimer <= 0.0f)
        {
            if (processGauge > 0.0f)
            {
                processGauge -= (0.05f * processReduceRate);
            }
            if(processGauge < 0.0f)
            {
                processGauge = 0.0f;
            }
        }
        else
        {
            reduceDelayTimer -= 0.01f;
        }


        // ----- 状態更新 -----
        if (processGauge == 100.0f) nowState = gaugeState.Freeze;
        if (processGauge < 100.0f && processGauge >= gaugeBorderWarning) nowState = gaugeState.Warning;
        if (processGauge < gaugeBorderWarning && processGauge >= gaugeBorderCaution) nowState = gaugeState.Caution;
        if (processGauge < gaugeBorderCaution) nowState = gaugeState.Stable;

        // ----- 表示更新 -----
        plSlider.value = processGauge / 100;
        plPercentText.text = (int)processGauge + "%";

        switch (nowState)
        {
            case gaugeState.Freeze:
                plPercentText.color = white;
                gaugeFillImage.color = white;
                break;
            case gaugeState.Warning:
                plPercentText.color = red;
                gaugeFillImage.color = red;
                break;
            case gaugeState.Caution:
                plPercentText.color = orange;
                gaugeFillImage.color = orange;
                m_timeLayer.SetTimeScale(0.5f);
                break;
            case gaugeState.Stable:
                plPercentText.color = green;
                gaugeFillImage.color = green;
                m_timeLayer.SetTimeScale(1.0f);
                break;
        }
    }
}
