using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPopManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> m_effects = new List<GameObject>();

    private Queue<GameObject> m_instancedEffects = new Queue<GameObject>();

    public Dictionary<string, GameObject> m_dictionary { get; private set; } = new Dictionary<string, GameObject>();

    public readonly struct Message
    {
        public enum Command
        {
            Play,
            Stop,
            Destroy
        }
        public Message(string n, Command c, Vector3 p)
        {
            name = n;
            command = c;
            position = p;
        }
        public string name { get; }

        public Command command { get; }

        public Vector3 position { get; }
    }


    public static Queue<Message> messages { get; private set; } = new Queue<Message>();

    private void Start()
    {
        foreach(var obj in m_effects)
        {
            m_dictionary.Add(obj.name, obj);
        }

    }

    private void Update()
    {
        while(messages.Count > 0)
        {
            Message message = messages.Dequeue();
            switch (message.command)
            {
                case Message.Command.Play:
                    PopEffect(message.name, message.position);
                    break;

                case Message.Command.Stop:
                    break;

                case Message.Command.Destroy:
                    break;
            }
        }
    }

    private void PopEffect(in string name, in Vector3 pos)
    {
        m_instancedEffects.Enqueue(GameObject.Instantiate(m_dictionary[name], pos, Quaternion.identity));        
    }
}
