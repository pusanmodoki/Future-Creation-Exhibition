using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    [System.Serializable]
    public abstract class PlayerCommandBase
    {
        [Header("Base Property")]

        [SerializeField, Tooltip("入力を受け付ける状態一覧")]
        [EnumFlags]
        private ActionState m_enableStates = ActionState.None;

        [SerializeField, Tooltip("どの状態に遷移するか")]
        private ActionState m_onCommandState = ActionState.None;

        [SerializeField]
        private ActionState m_offCommandState = ActionState.None;

        public ActionState enableStates { get { return m_enableStates; } }

        
        [SerializeField]
        private List<string> m_axesNames = new List<string>();


        public List<string> axesNames{ get { return m_axesNames; } }

        public bool isOnCommand { get; private set; }


        public void Command(PlayerController player)
        {
            if (OnCommand(player))
            {
                isOnCommand = true;
            }
            else
            {
                isOnCommand = false;
            }
        }


        protected abstract bool OnCommand(PlayerController player);

        public virtual void FixedUpdate(PlayerController player) { }

    }
}
