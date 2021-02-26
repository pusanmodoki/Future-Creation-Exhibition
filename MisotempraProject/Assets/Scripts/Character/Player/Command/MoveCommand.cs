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

        public Vector2 velocity { get; private set; }

        private enum AxesName
        {
            Horizontal = 0,
            Vertical
        }


        protected override bool OnCommand(PlayerController player)
        {
            if (!InputCheck())
            {
                return false;
            }

            // 進行方向計算
            float horizontal, vertical;

            if(player.state != ActionState.Airial)
            {
                player.state = ActionState.Run;
            }

            horizontal = InputManagement.GameInput.GetAxis(axesNames[(int)AxesName.Horizontal]);
            vertical = InputManagement.GameInput.GetAxis(axesNames[(int)AxesName.Vertical]);

            float rad = Mathf.Atan2(vertical, horizontal);
            rad += (player.playerCamera.polar.azimath + 90.0f) * Mathf.Deg2Rad;

            Vector2 vec = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));


            // 進行方向が大きく変わった時

            Vector2 vecNormal = vec.normalized;
            Vector2 forceNormal = force.normalized;

            float dot = vecNormal.x * forceNormal.x + vecNormal.y * forceNormal.y;
            
            if(Mathf.Abs(Mathf.Rad2Deg * Mathf.Acos(dot)) > 60.0f)
            {
                player.animator.SetTrigger("Turn");
                speed = 0.0f;
            }

            force = vec;


            // rolling 
            Vector3 rot = player.transform.eulerAngles;
            rot.y = -(Mathf.Atan2(force.y, force.x) * Mathf.Rad2Deg - 90);
            player.transform.eulerAngles = rot;

            return true;
        }

        public override void FixedUpdate(PlayerController player)
        {
            Vector3 velocity = player.playerRigidbody.velocity;

            Vector2 vec = force;

            if (player.state == ActionState.Attack)
            {
                speed = 0.0f;
                vec *= speed * m_limitMove;
                velocity.x = vec.x;
                velocity.z = vec.y;

                player.playerRigidbody.velocity = velocity;

                return;
            }


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


            // Vector2 vec = force;
            
            // 移動ベクトル
            vec *= speed * m_limitMove;
            velocity.x = vec.x;
            velocity.z = vec.y;

            player.playerRigidbody.velocity = velocity;


        }


        private bool InputCheck()
        {
            bool isOnHorizontal = InputManagement.GameInput.GetButton(axesNames[(int)AxesName.Horizontal]);
            bool isOnVertical = InputManagement.GameInput.GetButton(axesNames[(int)AxesName.Vertical]);

            // 入力検知と状態更新
            if (!isOnHorizontal && !isOnVertical)
            {
                if (InputManagement.GameInput.GetButtonUp(axesNames[(int)AxesName.Horizontal]) ||
                    InputManagement.GameInput.GetButtonUp(axesNames[(int)AxesName.Vertical]))
                {
                    PlayerController.instance.state = ActionState.Stand;
                }
                //else
                //{
                //    PlayerController.instance.SetAnimationState(AnimationState.Stand);
                //}
                return false;
            }
            return true;
        }

    }
}
