using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollisionSwitch : MonoBehaviour
{
    [SerializeField]
    private AttackAnimationMessageReceiver receiver = null;

    private CapsuleCollider m_collider = null;

    private void Start()
    {
        m_collider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        AttackAnimationMessage message = receiver.Pop();
        if(message.messageType == AttackAnimationMessage.MessageType.EnableCollision)
        {
            m_collider.enabled = true;
        }
        else if (message.messageType == AttackAnimationMessage.MessageType.DisableCollision)
        {
            m_collider.enabled = false;
        }
    }
}
