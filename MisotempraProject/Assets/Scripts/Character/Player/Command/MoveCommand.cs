using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    [System.Serializable]
    public class MoveCommand : PlayerCommandBase
    {
        [SerializeField, Tooltip("加速度")]
        private float m_acceleration = 1.0f;
        [SerializeField, Tooltip("最高速度")]
        private float m_maxSpeed = 10.0f;
        [SerializeField, Tooltip("減速度")]
        private float m_deceleration = 1.0f;
        [SerializeField, Range(0.0f, 2.0f)]
        private float m_limitMove = 1.0f;


        public Vector2 force { get; private set; }

        private float speed { get; set; }

        private enum AxesName
        {
            Horizontal = 0,
            Vertical
        }


        protected override bool OnCommand(PlayerController player)
        {
            bool isOnHorizontal = InputManagement.GameInput.GetButton(axesNames[(int)AxesName.Horizontal]);
            bool isOnVertical = InputManagement.GameInput.GetButton(axesNames[(int)AxesName.Vertical]);

            if (!isOnHorizontal && !isOnVertical)
            {
                if(InputManagement.GameInput.GetButtonUp(axesNames[(int)AxesName.Horizontal]) ||
                    InputManagement.GameInput.GetButtonUp(axesNames[(int)AxesName.Vertical]))
                {
                    player.animator.SetTrigger("RunStop");
                }
                else
                {
                    player.SetAnimationState(AnimationState.Stand);
                }
                return false;
            }


            // 進行方向計算
            float horizontal, vertical;
            player.state = ActionState.Run;

            horizontal = InputManagement.GameInput.GetAxis(axesNames[(int)AxesName.Horizontal]);
            vertical = InputManagement.GameInput.GetAxis(axesNames[(int)AxesName.Vertical]);

            float rad = Mathf.Atan2(vertical, horizontal);
            rad += (player.playerCamera.polar.azimath + 90.0f) * Mathf.Deg2Rad;

            Vector2 vec = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            Vector2 vecNormal = vec.normalized;
            Vector2 forceNormal = force.normalized;

            float dot = vecNormal.x * forceNormal.x + vecNormal.y * forceNormal.y;
            
            if(Mathf.Rad2Deg * Mathf.Acos(dot) > 60.0f)
            {
                speed *= -0.5f;
            }

            force = vec;

            // rolling 
            Vector3 rot = player.transform.eulerAngles;
            rot.y = -(Mathf.Atan2(force.y, force.x) * Mathf.Rad2Deg - 90);
            player.transform.eulerAngles = rot;

            player.SetAnimationState(AnimationState.Run);

            return true;
        }

        public override void FixedUpdate(PlayerController player)
        {
            if (isOnCommand)
            {
                speed += m_acceleration;
                if (speed > m_maxSpeed)
                {
                    speed = m_maxSpeed;
                }
            }
            else
            {
                speed -= m_deceleration;
                if (speed < 0.0f)
                {
                    speed = 0.0f;
                }
            }


            Vector3 velocity = player.playerRigidbody.velocity;
            Vector2 vec = force;

            // 移動ベクトル
            vec *= speed * m_limitMove;
            velocity.x = vec.x;
            velocity.z = vec.y;

            player.playerRigidbody.velocity = velocity;

        }


    }
}
