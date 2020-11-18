using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject m_player = null;

    [SerializeField]
    private float m_speed = 1.0f;

    [SerializeField]
    private float m_deadLine = 0.3f;

    [SerializeField]
    private Vector3 m_offset = new Vector3(0.0f, 2.0f, 0.0f);

    [SerializeField]
    private float m_maxZenith = 120.0f;

    [SerializeField]
    private float m_minZenith = 20.0f;

    [SerializeField]
    private GameObject m_followObject = null;

    [SerializeField]
    private GameObject m_lookObject = null;

    private Vector2 force;

    [System.Serializable]
    public struct Polar
    {
        public Polar(float r,float a, float z)
        {
            radius = r;
            azimath = a;
            zenith = z;
        }

        public float radius;
        public float azimath;
        public float zenith;

        public Vector3 ToRectangular
        {
            get
            {
                Vector3 vec;

                vec.x = radius * Mathf.Sin(zenith * Mathf.Deg2Rad) * Mathf.Cos(azimath * Mathf.Deg2Rad);
                vec.z = radius * Mathf.Sin(zenith * Mathf.Deg2Rad) * Mathf.Sin(azimath * Mathf.Deg2Rad);
                vec.y = radius * Mathf.Cos(zenith * Mathf.Deg2Rad);

                return vec;
            }
        }
    }

    [SerializeField]
    private Polar m_polar = new Polar(15.0f, 90.0f, 0.0f);

    public Polar polar { get { return m_polar; } }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        InputMove();

        ConvertRectangular();
    }

    void InputMove()
    {
        force.x = Input.GetAxis("Mouse X") * m_speed;
        force.y = Input.GetAxis("Mouse Y") * m_speed;

        if(force.x > m_deadLine || force.x < -m_deadLine)
        {
            m_polar.azimath -= force.x;
            if (m_polar.azimath > 360.0f) { m_polar.azimath -= 360.0f; }
            else if (m_polar.azimath < 0.0f) { m_polar.azimath += 360.0f; }

        }

        if (force.y > m_deadLine || force.y < -m_deadLine)
        {
            m_polar.zenith += force.y;
            if (m_polar.zenith > m_maxZenith) { m_polar.zenith = m_maxZenith; }
            else if (m_polar.zenith < m_minZenith) { m_polar.zenith = m_minZenith; }
        }
    }

    void ConvertRectangular()
    {
        Vector3 pos = m_player.transform.position;

        Vector3 polarPos = m_polar.ToRectangular;
        transform.position = pos + polarPos + m_offset;

        transform.LookAt(m_player.transform);

    }
}
