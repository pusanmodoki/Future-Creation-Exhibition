using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public readonly struct DamageMessage
{
    public enum DamageType
    {
        Physical,
        Energy
    }

    public enum AttackType
    {
        Weak,
        Middle,
        Strong
    }


    public DamageMessage(float d, GameObject o, AttackType at, DamageType dt, Vector3 p)
    {
        damage = d;
        owner = o;
        attackType = at;
        damageType = dt;
        point = p;
    }

    public AttackType attackType { get; }

    public DamageType damageType { get; }
    
    public float damage { get;}

    public GameObject owner { get;}    

    public Vector3 point { get; }
}
