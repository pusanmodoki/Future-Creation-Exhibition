using System.Collections;
using System.Collections.Generic;
using Damage;
using UnityEngine;

public class PlayerArmor : ArmorBase
{
    [SerializeField]
    private int m_maxStock = 3;

    [SerializeField, NonEditable]
    private int m_stock;

    public int stock { get { return m_stock; } }

    public int maxStock { get { return m_maxStock; } }

    private void Awake()
    {
        m_stock = m_maxStock;
    }

    protected override void TakeDamage(in RequestQueue request)
    {
        --m_stock;
    }

    protected override bool DeadCheck()
    {
        if (m_stock <= 0)
        {
            m_stock = 0;
            return true;
        }
        return false;
    }

    protected override void Death()
    {

    }
}
