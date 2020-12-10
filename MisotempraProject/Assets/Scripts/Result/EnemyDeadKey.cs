using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadKey : ResultKey
{
    private Enemy m_enemy = null;


    protected override void Init()
    {
        m_enemy = GetComponent<Enemy>();
    }

    protected override bool CheckAccept()
    {
        return m_enemy.isDead;
    }
}
