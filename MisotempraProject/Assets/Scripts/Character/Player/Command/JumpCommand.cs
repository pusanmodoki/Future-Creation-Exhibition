using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class JumpCommand : PlayerCommandBase
    {
        [SerializeField]
        private float m_height = 2.5f;

        [SerializeField]
        private float m_maxHeightSeconds = 1.0f;

        [SerializeField]
        private float m_force = 1.0f;


        protected override bool OnCommand(PlayerController player)
        {
            if (InputManagement.GameInput.GetButtonDown(axesNames[0]))
            {
                player.playerRigidbody.AddForce(Vector3.up * m_force);
                return true;
            }
            return false;
        }
    }
}

