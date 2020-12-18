using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    public class AttackCounter : MonoBehaviour
    {
        public void CounterReset()
        {
            PlayerController.instance.attackCommand.CountReset();
        }
    }
}
