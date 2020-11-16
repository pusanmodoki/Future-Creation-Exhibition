using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollision : MonoBehaviour
{

    [Header("Collision Type")]
    [SerializeField]
    private DamageMessage.AttackType m_attackType = DamageMessage.AttackType.Weak;

    [SerializeField]
    private DamageMessage.DamageType m_damageType = DamageMessage.DamageType.Physical;

    [Header("Damage")]
    [SerializeField]
    private float m_power = 10.0f;


    private void Awake()
    {

    }


    private void OnDestroy()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        foreach(var queue in DamageMessageManager.messages)
        {
            Debug.Log(queue.Key);
            Debug.Log(queue.Value);
        }

        

        int id = other.gameObject.GetInstanceID();
        DamageMessageManager.messages[id] = new DamageMessage(m_power, gameObject, m_attackType, m_damageType, other.ClosestPointOnBounds(transform.position));
    }
}
