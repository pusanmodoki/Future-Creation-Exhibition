using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damage
{
    public class DamageReceiver : MonoBehaviour
    {
        [SerializeField]
        DamageController m_damageController = null;

        public void Request(GameObject attackObject, AttackInfo info)
        {
            m_damageController.receiver.Request(attackObject, info);
        }
    }
}
