using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    [System.Serializable]
    public class AttackCommand : PlayerCommandBase
    {
        [SerializeField]
        private float m_scale = 1.0f;

        [SerializeField]
        private string m_key = "";


        protected override bool OnCommand(PlayerController player)
        {
            if (InputManagement.GameInput.GetButtonDown(axesNames[0]) && player.isAcceptAttack)
            {
                player.animator.SetTrigger("Attack");

                player.isAcceptAttack = false;
                player.playerRigidbody.velocity = Vector3.zero;


                // m_attackCollision.SetActive(true);
                player.damageController.sender.EnableAction(m_key, m_scale);
                return true;
            }

            return false;
        }

    }
}
