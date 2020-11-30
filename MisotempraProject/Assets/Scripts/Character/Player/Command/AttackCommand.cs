using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    [System.Serializable]
    public class AttackCommand : PlayerCommandBase
    {
        [SerializeField]
        private Damage.DamageController m_damageController = null;

        [SerializeField]
        private float m_scale = 1.0f;

        [SerializeField]
        private string m_key = "";

        [Header("Attack Object")]
        [SerializeField]
        private GameObject m_attackCollision = null; 


        public override void OnCommand()
        {
            m_attackCollision.SetActive(true);
            m_damageController.sender.EnableAction("m_key", m_scale);
        }

        public AttackCommand(PlayerController player, Damage.DamageController damageController) : base(player)
        {
            m_damageController = damageController;
        }
    }
}
