using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    public class AnimationMessageReceiver : MonoBehaviour
    {
        public List<AnimationStringMessage> messages { get; private set; } = new List<AnimationStringMessage>();

        [SerializeField]
        private List<string> m_strings = new List<string>();

        private void AddMessage(string message)
        {
            messages.Add(new AnimationStringMessage(message));
        }

        private void AddMessage(int num)
        {
            if(m_strings.Count <= num || num < 0) { return; }
            messages.Add(new AnimationStringMessage(m_strings[num]));
        }

        public bool FindMessage(string s)
        {
            foreach(var message in messages)
            {
                if(s.GetHashCode() == message.value.GetHashCode()) { return true; }
            }
            return false;
        }

        private void Start()
        {
            
        }

        IEnumerator MessageRemove()
        {
            bool isLoop = true;
            while (isLoop)
            {
                foreach(var message in messages)
                {

                }
                yield return new WaitForSeconds(1.0f);
            }
        }

    }
}
