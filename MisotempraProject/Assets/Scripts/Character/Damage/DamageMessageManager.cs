using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMessageManager:MonoBehaviour
{
    static public Dictionary<int, DamageMessage> messages { get; private set; } = new Dictionary<int, DamageMessage>();

}
