using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AttackAnimationMessage
{
    public enum MessageType
    {
        None,
        CancelAccept,
        StartAnimation,
        EnableCollision,
        DisableCollision
    }

    public MessageType messageType { get; }

    public AttackAnimationMessage(MessageType t)
    {
        messageType = t;
    }
}

public class AttackAnimationMessageReceiver : MonoBehaviour
{
    public List<AttackAnimationMessage> messages { get; private set; } = new List<AttackAnimationMessage>();


    public AttackAnimationMessage Find(AttackAnimationMessage.MessageType t)
    {
        for(int i = 0; i < messages.Count; ++i)
        {
            if(messages[i].messageType == t)
            {
                return messages[i];
            }
        }

        return new AttackAnimationMessage(AttackAnimationMessage.MessageType.None);
    }

    public AttackAnimationMessage FindAndPop(AttackAnimationMessage.MessageType t)
    {
        for (int i = 0; i < messages.Count; ++i)
        {
            if (messages[i].messageType == t)
            {
                var result = messages[i];
                messages.RemoveAt(i);
                return result;
            }
        }

        return new AttackAnimationMessage(AttackAnimationMessage.MessageType.None);
    }

    public AttackAnimationMessage Pop()
    {
        if(messages.Count > 0)
        {
            var result = messages[0];
            messages.RemoveAt(0);
            return result;
        }
        return new AttackAnimationMessage(AttackAnimationMessage.MessageType.None);

    }

    // Update is called once per frame
    private void StartAttackAnimation()
    {
        messages.Add(new AttackAnimationMessage(AttackAnimationMessage.MessageType.StartAnimation));
    }

    private void AnimationCancelAccept()
    {
        messages.Add(new AttackAnimationMessage(AttackAnimationMessage.MessageType.CancelAccept));
    }

    private void EnableAttackCollision()
    {
        messages.Add(new AttackAnimationMessage(AttackAnimationMessage.MessageType.EnableCollision));
    }

    private void DisableAttackCollision()
    {
        messages.Add(new AttackAnimationMessage(AttackAnimationMessage.MessageType.DisableCollision));
    }

}
