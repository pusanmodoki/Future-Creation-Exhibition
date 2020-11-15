using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessingLoadGravity : MonoBehaviour
{

    [SerializeField]
    private TimeManagement.TimeLayer m_timeLayer;

    public static float gravity { get; private set; } = 9.8f;
    private Rigidbody m_rigidbody;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        TimeManagement.TimeLayer.InitLayer(ref m_timeLayer);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_rigidbody.AddForce(Vector3.down * gravity * m_timeLayer.fixedDeltaTime * 50.0f);
    }
}
