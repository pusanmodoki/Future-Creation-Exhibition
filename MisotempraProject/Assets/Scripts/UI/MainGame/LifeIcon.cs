using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeIcon : MonoBehaviour
{
    private LifeUI canvas { get; set; } = null;

    private void Start()
    {
        canvas = transform.root.GetComponent<LifeUI>();

        
    }

    public void DisableDisplay()
    {

    }
}
