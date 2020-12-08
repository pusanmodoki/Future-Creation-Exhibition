using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : Singleton.DontDestroySingletonMonoBehaviour<CharacterManager>
{
    Dictionary<string, ArmorBase> m_characters = new Dictionary<string, ArmorBase>();

    protected override void Init()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
