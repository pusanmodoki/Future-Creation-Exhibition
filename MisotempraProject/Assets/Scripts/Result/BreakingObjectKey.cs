using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingObjectKey : ResultKey
{
    private void OnDisable()
    {
        isAccept = true;
    }



    protected override bool CheckAccept()
    {
        return false;
    }
}
