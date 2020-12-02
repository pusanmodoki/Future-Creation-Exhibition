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

    protected override void TakeDamage(in Damage.RequestQueue request)
    {
        m_durable -= request.attack * (1.0f - Resist(request.details.damageType));
    }

    protected override bool DeadCheck()
    {
        if (m_durable <= 0.0f)
        {
            m_durable = 0.0f;
            return true;
        }
        return false;
    }

    protected override void Death()
    {
        gameObject.SetActive(false);
    }

    public float Resist(Damage.DamageType type)
    {
        if(type == Damage.DamageType.Physical)
        {
            return m_resistPhysical;
        }
        return m_resistEnergy;
    }

}
