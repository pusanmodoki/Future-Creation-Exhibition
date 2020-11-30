using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    [System.Serializable]
    public class MoveCommand : PlayerCommandBase
    {
        public Rigidbody rigidbody { get; }

        [SerializeField]
        private float m_acceleration = 1.0f;
        [SerializeField]
        private float m_maxSpeed = 10.0f;
        [SerializeField]
        private float m_deceleration = 1.0f;
        [SerializeField]
        private bool m_isInputMove = false;

        private enum AxesName
        {
            Horizontal = 0,
            Vertical
        }

        public MoveCommand(PlayerController player) : base(player)
        {
            
        }


        public override void OnCommand()
        {
            bool isOnHorizontal = InputManagement.GameInput.GetButton(axesNames[(int)AxesName.Horizontal]);
            bool isOnVertical = InputManagement.GameInput.GetButton(axesNames[(int)AxesName.Horizontal]);

            if (!isOnHorizontal && !isOnVertical)
            {
                m_isInputMove = false;
                return;
            }

            m_isInputMove = true;

            // 進行方向計算
            float horizontal, vertical;

            horizontal = InputManagement.GameInput.GetAxis(axesNames[(int)AxesName.Horizontal]);
            vertical = InputManagement.GameInput.GetAxis(axesNames[(int)AxesName.Horizontal]);

            float rad = Mathf.Atan2(vertical, horizontal);
            //rad += (m_camera.polar.azimath + 90.0f) * Mathf.Deg2Rad;

            //m_force.x = Mathf.Cos(rad);
            //m_force.y = Mathf.Sin(rad);

            //Rolling();

        }
    }
}
