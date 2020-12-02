﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController instance { get; private set; } = null;

        public ActionState state { get { return m_state; } set { m_state = value; } }

        public bool isAcceptAttack { get; set; } = true;

        private Ray m_jumpDetectRay = new Ray(new Vector3(0.0f, 0.0f, 0.0f), Vector3.down);

        public PlayerCamera playerCamera { get { return m_camera; } }

        public Animator animator { get; private set; }

        public Rigidbody playerRigidbody { get; private set; }

        public Damage.DamageController damageController { get; private set; }

        /// <summary>
        /// プレイヤーの状態
        /// </summary>
        [Header("State")]
        [SerializeField]
        private ActionState m_state = ActionState.None;

        /// <summary>
        /// 参照
        /// </summary>
        [SerializeField]
        private PlayerCamera m_camera = null;

        [Header("Attack Info")]
        [SerializeField]
        private List<AttackCommand> m_attackCommand = new List<AttackCommand>();

        [Header("Jump Info")]
        [SerializeField]
        private JumpCommand m_jumpCommand = null;

        [SerializeField]
        private bool m_isJump = true;

        [Header("Move Info")]
        [SerializeField]
        private MoveCommand m_moveCommand = null;

        private void Start()
        {
            if (instance)
            {
#if UNITY_EDITOR
                Debug.LogError("Playerが複数存在しています。");
#endif
            }
            instance = this;

            damageController = GetComponent<Damage.DamageController>();
            if (!damageController)
            {
#if UNITY_EDITOR
                Debug.LogError("DamageControllerが見つかりません。");
#endif
            }

            animator = GetComponent<Animator>();
            if (!animator)
            {
#if UNITY_EDITOR
                Debug.LogError("Animatorが見つかりません。");
#endif
            }

            playerRigidbody = GetComponent<Rigidbody>();
            if (!playerRigidbody)
            {
#if UNITY_EDITOR
                Debug.LogError("RigidBodyが見つかりません。");
#endif
            }
        }

        // Update is called once per frame
        void Update()
        {
            switch (m_state)
            {
                case ActionState.Stand:
                    {
                        m_moveCommand.Command(this);
                        // Jump();
                        //AttackInput();

                        m_attackCommand[0].Command(this);

                        m_jumpCommand.Command(this);
                        break;
                    }
                case ActionState.Run:
                    {
                        m_moveCommand.Command(this);
                        // Jump();
                        //AttackInput();
                        m_attackCommand[0].Command(this);
                        m_jumpCommand.Command(this);
                        break;
                    }
                case ActionState.Airial:
                    {
                        m_moveCommand.Command(this);

                        break;
                    }

                case ActionState.Attack:
                    {
                        m_attackCommand[0].Command(this);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void FixedUpdate()
        {
            switch (m_state)
            {
                case ActionState.Stand:
                    {
                        m_moveCommand.FixedUpdate(this);

                        break;
                    }
                case ActionState.Run:
                    {
                        m_moveCommand.FixedUpdate(this);

                        break;
                    }
                case ActionState.Attack:
                    {
                        break;
                    }
                case ActionState.Airial:
                    {
                        m_moveCommand.FixedUpdate(this);

                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            // 移動

            JumpDetect();
        }

        /// <summary>
        /// 移動処理（コントローラ）
        /// </summary>
        //private void MoveInput()
        //{
        //    float horizontal, vertical;

        //    horizontal = Input.GetAxis(m_horizontal);
        //    vertical = Input.GetAxis(m_vertical);

        //    if (horizontal == 0.0f && vertical == 0.0f)
        //    {
        //        m_force = Vector2.zero;
        //        return;
        //    }

        //    float rad = Mathf.Atan2(vertical, horizontal);
        //    rad += (m_camera.polar.azimath + 90.0f) * Mathf.Deg2Rad;


        //    m_force.x = Mathf.Cos(rad);
        //    m_force.y = Mathf.Sin(rad);
        //}

        private void JumpDetect()
        {
            Vector3 vec = transform.position;
            vec.y += 0.2f;
            m_jumpDetectRay.origin = vec;
            m_isJump = Physics.Raycast(m_jumpDetectRay, 0.3f);
            if (m_isJump) { state = ActionState.Stand; }
            else { state = ActionState.Airial; }
        }

        private void Advance(float value)
        {
            Vector3 vec = Vector3.zero;
            vec.x = Mathf.Cos(transform.rotation.eulerAngles.x * Mathf.Deg2Rad) * value;
            vec.z = Mathf.Sin(transform.rotation.eulerAngles.y * Mathf.Deg2Rad) * value;

            playerRigidbody.AddForce(vec);
        }

        public void SetAcceptAttack(int i)
        {
            if (animator != null)
            {
                animator.SetBool("IsAcceptAttack", i != 0);
            }
            isAcceptAttack = i != 0;
        }

        public void SetState(int state)
        {
            m_state = (ActionState)state;
        }

        private void OnDestroy()
        {
            instance = null;
        }
    }

    [Flags]
    public enum ActionState
    {
        None = 0,
        Stand = 1 << 0,
        Run = 1 << 1,
        Attack = 1 << 2,
        Airial = 1 << 3
    }
}

