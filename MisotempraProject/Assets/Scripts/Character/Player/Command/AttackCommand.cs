﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    [System.Serializable]
    public class AttackCommand : PlayerCommandBase
    {
        [SerializeField]
        private List<float> m_attackScales = new List<float>();


        [SerializeField]
        private string m_key = "";

        private int m_attackCounter = 0;

        protected override bool OnCommand(PlayerController player)
        {
            if (InputManagement.GameInput.GetButtonDown(axesNames[0]) && player.isAcceptAttack)
            {
                player.animator.SetTrigger("Attack");
                player.state = ActionState.Attack;

                //player.isAcceptAttack = false;
                //player.playerRigidbody.velocity = Vector3.zero;

                //player.SetAcceptAttack(0);
                //// m_attackCollision.SetActive(true);
                player.damageController.sender.EnableAction(m_key, m_attackScales[m_attackCounter]);
                ++m_attackCounter;
                if (m_attackCounter >= m_attackScales.Count)
                {
                    m_attackCounter = 0;
                }
                return true;
            }

            return false;
        }

        public void CountReset()
        {
            m_attackCounter = 0;
        }

    }
}
