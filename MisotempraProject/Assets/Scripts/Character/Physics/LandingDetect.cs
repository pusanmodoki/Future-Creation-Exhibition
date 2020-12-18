using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OriginalPhysics
{
    [System.Serializable]
    public class LandingDetect
    {
        [SerializeField, NonEditable]
        private bool m_isLanding = false;

        [SerializeField]
        private LayerMask m_layerMask = new LayerMask();

        [SerializeField]
        private Vector3 m_offsetOrigin = new Vector3(0.0f, 0.0f, 0.0f);

        [SerializeField]
        private float m_maxDistance = 0.1f;

        [SerializeField]
        private Ray ray;

        public bool IsLanding(GameObject gameObject)
        {
            m_isLanding = Physics.Raycast(gameObject.transform.position + m_offsetOrigin, Vector3.down, m_maxDistance, m_layerMask);

            return m_isLanding;
        }

    }
}
