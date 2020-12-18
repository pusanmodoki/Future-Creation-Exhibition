using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class StateSetter : MonoBehaviour
    {
        private void SetState(int state)
        {
            PlayerController.instance.SetState(state);
        }
    }
}
