using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurableArmor : ArmorBase
{

    [Header("Durable")]
    [SerializeField]
    private float m_durable = 100.0f;

    public float durable { get { return m_durable; } }

    [Header("Resist")]
    [SerializeField, Range(0.0f, 1.0f)]
    private float m_resistPhysical = 0.0f;

    [SerializeField, Range(0.0f, 1.0f)]
    private float m_resistEnergy = 0.0f;

    public enum WeightLevel
    {
        Light,
        Middle,
        Heavy
    }

    [Header("Weight")]
    [SerializeField]
    private float m_weight = 1.0f;

    [SerializeField]
    private ProcessingLoad.Physics m_physics = null;




    protected override void Damage(in DamageMessage message)
    {
        switch (message.damageType)
        {
            case DamageMessage.DamageType.Physical:
                PhysicalDamage(message.damage);
                break;

            case DamageMessage.DamageType.Energy:
                EnergyDamage(message.damage);
                break;

            default:
                break;
        }
    }

    protected override bool DeadCheck()
    {
        if (m_durable < 0.0f)
        {
            m_durable = 0.0f;
            gameObject.SetActive(false);
            return true;
        }
        return false;
    }

    protected override void KnockBack(in DamageMessage message)
    {
        switch (message.attackType)
        {
            case DamageMessage.AttackType.Weak:
                WeakKnockBack(message);
                break;
            case DamageMessage.AttackType.Middle:
                break;
            case DamageMessage.AttackType.Strong:
                break;

            default:
                break;
        }
    }


    public void EnergyDamage(in float damage)
    {
        m_durable -= damage * (1.0f - m_resistEnergy);
    }

    public void PhysicalDamage(in float damage)
    {
        m_durable -= damage * (1.0f - m_resistPhysical);
    }

    virtual protected void WeakKnockBack(in DamageMessage message)
    {
        Vector3 vector = (transform.position - message.point);


        vector.y = 0.0f;
        vector.Normalize();

        vector.y = 5.0f;

        m_physics.AddForce(vector * m_weight);
    }


    virtual protected void MiddleKnockBack()
    {

    }

    virtual protected void StrongKnockBack()
    {

    }
}
