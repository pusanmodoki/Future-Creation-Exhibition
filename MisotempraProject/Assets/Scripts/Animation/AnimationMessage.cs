using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    public enum MessageType
    {
        Integer,
        Float,
        Boolean,
        String,
        Object
    }

    public interface IAnimationMessageBase
    {
        MessageType messageType { get; }
    }

    public struct AnimationIntegerMessage : IAnimationMessageBase
    {
        public MessageType messageType { get { return MessageType.Integer; } }

        public int value { get; }

        public AnimationIntegerMessage(in int v)
        {
            value = v;
        }
    }

    public struct AnimationFloatMessage : IAnimationMessageBase
    {
        public MessageType messageType { get { return MessageType.Float; } }

        public float value { get; }

        public AnimationFloatMessage(in float v)
        {
            value = v;
        }
    }
    public struct AnimationBooleanMessage : IAnimationMessageBase
    {
        public MessageType messageType { get { return MessageType.Boolean; } }

        public bool value { get; }

        public AnimationBooleanMessage(in bool v)
        {
            value = v;
        }
    }

    public struct AnimationStringMessage : IAnimationMessageBase
    {
        public MessageType messageType { get { return MessageType.Boolean; } }

        public string value { get; }

        public AnimationStringMessage(string v)
        {
            value = v;
            isReceived = false;
        }

        public bool isReceived { get; private set; }
    }
}
