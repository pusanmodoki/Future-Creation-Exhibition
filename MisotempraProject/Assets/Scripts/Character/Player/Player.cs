using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public delegate void StateFunction();
    public enum StateType
    {
        None = 0,
        Stand,
        Run,
        Jumping,
        Falling,
        Attack,
        Max
    }


    /// <summary>
    /// 参照
    /// </summary>
    [Header("class ref")]
    [SerializeField]
    private PlayerCamera m_camera = null;
    [SerializeField]
    private Animator m_animator = null;
    [SerializeField]
    private Rigidbody m_rigidbody = null;

    /// <summary>
    /// プレイヤーの状態
    /// </summary>
    [Header("player status")]
    [SerializeField]
    private StateType m_state = StateType.None;


    /// <summary>
    /// 移動パラメータ
    /// </summary>
    [Header("player speed")]
    [SerializeField]
    private float m_acceleration = 1.0f;
    [SerializeField]
    private float m_maxSpeed = 10.0f;
    [SerializeField]
    private float m_deceleration = 1.0f; 
    [SerializeField]
    private bool m_isInputMove = false;

    [SerializeField, Range(0.0f, 2.0f)]
    private float m_limitMove = 1.0f;

    [Header("jump parameter")]
    [SerializeField]
    private float m_jumpForce = 1.0f;
    [SerializeField]
    private float m_jumpHeight = 5.0f;

    [SerializeField]
    private bool m_isJump = true;


    /// <summary>
    /// 入力関係
    /// </summary>
    [Header("input name")]
    [SerializeField]
    private string m_horizontal = "Horizontal";
    [SerializeField]
    private string m_vertical = "Vertical";
    public enum InputDevice
    {
        KeyboardAndMouse,
        Controller
    }

    [SerializeField]
    private float m_speed;

    [SerializeField]
    private Vector2 m_force;
    private List<StateFunction> m_stateFunctions = new List<StateFunction>((int)StateType.Max);

    private bool m_isAcceptAttack = true;

    private Ray m_jumpDetectRay = new Ray(new Vector3(0.0f, 0.0f, 0.0f), Vector3.down);


    public void SetAcceptAttack(int i)
    {
        m_animator.SetBool("IsAcceptAttack", i != 0);
        m_isAcceptAttack = i != 0;
    }

    public void SetState(int state)
    {
        m_state = (StateType)state;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        {
            case StateType.Stand:
                {
                    MoveInputKeyboard();
                    Jump();
                    AttackInput();
                    break;
                }
            case StateType.Run:
                {
                    MoveInputKeyboard();
                    Jump();
                    AttackInput();
                    break;
                }
            case StateType.Attack:
                {
                    AttackInput();
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
            case StateType.Stand:
                {
                    break;
                }
            case StateType.Run:
                {
                    Run();
                    Jump();
                    break;
                }
            case StateType.Attack:
                {
                    m_speed = 0.0f;

                    break;
                }
            default:
                {
                    break;
                }
        }

        // 移動
        Moving();
        JumpDetect();
    }

    /// <summary>
    /// 移動処理（コントローラ）
    /// </summary>
    private void MoveInput()
    {
        float horizontal, vertical;

        horizontal = Input.GetAxis(m_horizontal);
        vertical = Input.GetAxis(m_vertical);

        if(horizontal == 0.0f && vertical == 0.0f)
        {
            m_force = Vector2.zero;
            return;
        }

        float rad = Mathf.Atan2(vertical, horizontal);
        rad += (m_camera.polar.azimath + 90.0f) * Mathf.Deg2Rad;


        m_force.x = Mathf.Cos(rad);
        m_force.y = Mathf.Sin(rad);
    }

    /// <summary>
    /// 移動入力（キーボード）
    /// </summary>
    private void MoveInputKeyboard()
    {
        // ボタンの判定
        bool isOnHorizontal = Input.GetButton(m_horizontal);
        bool isOnVertical = Input.GetButton(m_vertical);

        if(!isOnHorizontal && !isOnVertical)
        {
            m_isInputMove = false;
            return;
        }

        m_isInputMove = true;
        m_state = StateType.Run;

        // 進行方向計算
        float horizontal, vertical;

        horizontal = Input.GetAxis(m_horizontal);
        vertical = Input.GetAxis(m_vertical);

        float rad = Mathf.Atan2(vertical, horizontal);
        rad += (m_camera.polar.azimath + 90.0f) * Mathf.Deg2Rad;

        m_force.x = Mathf.Cos(rad);
        m_force.y = Mathf.Sin(rad);

        Rolling();

    }

    /// <summary>
    /// 加速処理
    /// </summary>
    private void Acceleration()
    {
        // 加速処理
        m_speed += m_acceleration;
        if (m_speed > m_maxSpeed)
        {
            m_speed = m_maxSpeed;
            m_state = StateType.Run;
        }
    }


    /// <summary>
    /// 移動処理
    /// </summary>
    private void Moving()
    {
        Vector3 velocity = m_rigidbody.velocity;
        Vector2 force = m_force;

        // 移動ベクトル
        force *= m_speed * m_limitMove;
        velocity.x = force.x;
        velocity.z = force.y;

        m_rigidbody.velocity = velocity;
    }

    private void Run()
    {
        if (m_isInputMove) Acceleration();
        else Deceleration();
    }

    /// <summary>
    /// 減速処理
    /// </summary>
    private void Deceleration()
    {
        m_speed -= m_deceleration;

        if(m_speed < 0.0f)
        {
            m_speed = 0.0f;
            m_state = StateType.Stand;
        }
    }

    /// <summary>
    /// 攻撃入力
    /// </summary>
    private void AttackInput()
    {
        bool isAttack = Input.GetButtonDown("Fire1");

        if (isAttack && m_isAcceptAttack)
        {

            m_state = StateType.Attack;

            m_animator.SetTrigger("Attack");
            m_isAcceptAttack = false;
        }
    }

    /// <summary>
    /// プレイヤーの向き
    /// </summary>
    private void Rolling()
    {
        Vector3 rot = transform.eulerAngles;
        rot.y = -(Mathf.Atan2(m_force.y, m_force.x) * Mathf.Rad2Deg - 90);
        transform.eulerAngles = rot;
    }

    private void JumpDetect()
    {
        Vector3 vec = transform.position;
        vec.y += 0.2f;
        m_jumpDetectRay.origin = vec;
        m_isJump = Physics.Raycast(m_jumpDetectRay, 1.0f);
        if (m_isJump)
        {
            if (m_state == StateType.Falling) m_state = StateType.Stand;
        }
        else
        {
            if(m_state != StateType.Jumping) m_state = StateType.Falling;
        }
    }

    private void Jump()
    {
        if (m_isJump)
        {
            if (Input.GetButtonDown("Jump"))
            {
                m_state = StateType.Jumping;
                // m_rigidbody.AddForce(Vector3.up * m_jumpForce, ForceMode.Acceleration);
                Vector3 vec = m_rigidbody.velocity;

                vec.y = m_jumpForce;
                m_rigidbody.velocity = vec;               
            }
        }
    }
    
}
