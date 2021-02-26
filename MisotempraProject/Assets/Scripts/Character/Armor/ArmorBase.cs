using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ArmorBase : MonoBehaviour
{

    [Header("Base Property")]
    [SerializeField]
    protected bool m_isDead = false;
    public bool isDead { get { return m_isDead; } }

    [SerializeField]
    protected float m_invincibleTime = 1.0f;

    [SerializeField]
    protected bool m_isInvincible = false;

    public Damage.DamageController damageController { get; private set; }

    private bool m_enableKnockBack = false;

    public bool enableKnockBack
    {
        get
        {
            bool result = m_enableKnockBack;
            if (result)
            {
                m_enableKnockBack = false;
            }
            return result;
        }
    }


    private void Start()
    {
        damageController = GetComponent<Damage.DamageController>();
        if (!damageController)
        {
            Debug.LogError("not ref damage controller");
        }
        Init();
    }

    protected virtual void Init() { }

    private void Update()
    {
        if(damageController.receiver.requestQueue.Count < 1)
        {
            return;
        }


        Damage.RequestQueue request = damageController.receiver.Pop();

        TakeDamage(request);
        m_isDead = DeadCheck();

        if (!isDead)
        {
            KnockBack(request);
            m_enableKnockBack = true;
        }
        else
        {
            Death();
        }
    }

    protected abstract void TakeDamage(in Damage.RequestQueue request);

    protected virtual void KnockBack(in Damage.RequestQueue request) { }

    protected abstract bool DeadCheck();

    protected abstract void Death();

    public void OnInvincible()
    {
        m_isInvincible = true;
        
        StartCoroutine("InvincibleCounter");
    }

    /// <summary>
    /// 無敵カウント
    /// </summary>
    /// <returns></returns>
    private IEnumerator InvincibleCounter()
    {
        m_isInvincible = true;


        yield return new WaitForSeconds(m_invincibleTime);

        m_isInvincible = false;
    }
}
