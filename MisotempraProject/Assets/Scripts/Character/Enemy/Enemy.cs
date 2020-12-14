using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum State
    {
        Searching,
        Fighting,
        Dead,
        NotActive
    }

    [SerializeField]
    private State m_state = State.Searching;

    [SerializeField]
    private ArmorBase m_armor = null;

    [SerializeField]
    private EffectDictionary m_effectDictionary = null;

    [SerializeField]
    private ProcessingLoad.Physics m_physics = null;

    [SerializeField]
    private Transform m_player = null;

    [SerializeField]
    private float m_speed = 10.0f;

    [SerializeField]
    private float m_checkTime = 0.02f;


    [SerializeField]
    private TimeManagement.TimeLayer m_timeLayer = null;

    private bool m_isCheck = true;

    public bool isDead { get { return m_state == State.Dead;} }

    private void Awake()
    {
        TimeManagement.TimeLayer.InitLayer(ref m_timeLayer);
    }

    private void OnEnable()
    {
        StartCoroutine("CheckArmorState");
    }

    private IEnumerator CheckArmorState()
    {
        while (m_isCheck)
        {
            if (DeadCheck())
            {
                m_effectDictionary.PlayEffect("DeadEffect");
                m_state = State.Dead;
                break;
            }
            yield return new WaitForSeconds(m_checkTime);
        }
        while (true)
        {
            if (!m_effectDictionary.IsPlaying("DeadEffect"))
            {
                gameObject.SetActive(false);
                break;
            }
            yield return new WaitForSeconds(m_checkTime);
        }
    }

    private bool DeadCheck()
    {
        return m_armor.isDead;
    }
}
