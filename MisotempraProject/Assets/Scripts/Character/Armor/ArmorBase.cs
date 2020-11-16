using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ArmorBase : MonoBehaviour
{

    [Header("Dead")]
    [SerializeField]
    protected bool m_isDead = false;

    public bool isDead { get { return m_isDead; } }

    [Header("Invincible")]
    [SerializeField]
    protected float m_invincibleTime = 1.0f;

    [SerializeField]
    protected bool m_isInvincible = false;

    [SerializeField]
    protected string m_hitEffectName = "";

    private void Update()
    {
        int id = gameObject.GetInstanceID();
        if(DamageMessageManager.messages.ContainsKey(id))
        {
            DamageMessage message = DamageMessageManager.messages[id];

            Damage(message);
            KnockBack(message);

            DamageMessageManager.messages.Remove(id);
            m_isDead = DeadCheck();

            EffectPopManager.messages.Enqueue(new EffectPopManager.Message(m_hitEffectName, EffectPopManager.Message.Command.Play, message.point));
        }
    }

    protected abstract void Damage(in DamageMessage message);

    protected virtual void KnockBack(in DamageMessage message) { }

    protected abstract bool DeadCheck();

    public void OnInvincible()
    {
        m_isInvincible = true;
        StartCoroutine("InvincibleCounter");
    }

    private IEnumerator InvincibleCounter()
    {
        m_isInvincible = true;

        yield return new WaitForSeconds(m_invincibleTime);

        m_isInvincible = false;
    }
}
