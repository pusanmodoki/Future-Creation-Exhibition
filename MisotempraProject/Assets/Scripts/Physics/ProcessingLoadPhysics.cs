using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessingLoadPhysics : MonoBehaviour
{
    [Header("Time Layer")]
    [SerializeField]
    private TimeManagement.TimeLayer m_timeLayer;

    [Header("Is Active")]
    [SerializeField]
    private bool m_isActive = true;

    [Header("Velocity")]
    [SerializeField]
    private Vector3 m_velocity = new Vector3();

    [SerializeField]
    private float m_decelerate = 0.0f;

    [Header("Gravity")]
    [SerializeField]
    private float m_gravity = 9.8f;

    [SerializeField]
    private float m_maxFallSpeed = 100.0f;

    private Rigidbody m_rigidbody = null;

    public Vector3 velocity { get { return m_velocity; } }
    public float gravity { get { return m_gravity; } }

    private void Awake()
    {
        TimeManagement.TimeLayer.InitLayer(ref m_timeLayer);
    }

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        if(m_rigidbody == null)
        {
            Debug.LogError("Not attach Rigidbody");
        }
    }


    private void FixedUpdate()
    {
        if (!m_isActive) { return; }



        AddGravity();           // 重力
        Decelerate();           // 減速

        UpdateRigidbody();      // Rigidbodyに渡す
    }

    
    


    /// <summary>
    /// 計算後の速度をRigidbodyに渡す
    /// </summary>
    private void UpdateRigidbody()
    {
        m_rigidbody.velocity = m_velocity * m_timeLayer.timeScale;
    }


    /// <summary>
    /// 重力の加算
    /// </summary>
    private void AddGravity()
    {
        m_velocity += Vector3.down * m_gravity * m_timeLayer.fixedDeltaTime;
        if(m_velocity.y < m_maxFallSpeed)
        {
            m_velocity.y = -m_maxFallSpeed;
        }
    }

    /// <summary>
    /// 原則処理
    /// </summary>
    private void Decelerate()
    {
        Vector3 velo_xz = m_velocity;
        Vector3 vec_decelerate = m_velocity;

        velo_xz.y = vec_decelerate.y = 0.0f;

        vec_decelerate = vec_decelerate.normalized * m_decelerate;

        if(velo_xz.magnitude < vec_decelerate.magnitude)
        {
            m_velocity.x = 0.0f;
            m_velocity.z = 0.0f;
        }
        else
        {
            m_velocity -= vec_decelerate;
        }

    }

}
