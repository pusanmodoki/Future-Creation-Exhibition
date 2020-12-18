using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    public class PlayerPhysics : MonoBehaviour
    {
        public new Rigidbody rigidbody { get; private set; } = null;
        public float gravity { get { return m_gravity; } private set { m_gravity = value; } }
        private Vector3 force { get; set; }

        [SerializeField]
        private float m_gravity = 9.8f;

        [SerializeField, NonEditable]
        private Vector3 m_velocity = new Vector3(0.0f, 0.0f, 0.0f);

        [SerializeField, NonEditable]
        private Vector3 m_acceleration = new Vector3(0.0f, 0.0f, 0.0f);

        // Start is called before the first frame update
        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            if (!rigidbody)
            {
                rigidbody = gameObject.AddComponent<Rigidbody>();
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {

        }

        private void GravityAdd()
        {

        }
    }
}
