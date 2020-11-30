using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    [System.Serializable]
    public abstract class PlayerCommandBase
    {
        public PlayerController m_player { get; }

        [SerializeField]
        [EnumFlags]
        private ActionState m_enableStates = ActionState.None;

        public ActionState enableStates { get { return m_enableStates; } }

        [SerializeField]
        private List<string> m_axesNames = new List<string>();

        public List<string> axesNames{ get { return m_axesNames; } }

        public PlayerController player { get; }

        public abstract void OnCommand();

        public PlayerCommandBase(PlayerController player)
        {
            m_player = player;
        }
    }
}
