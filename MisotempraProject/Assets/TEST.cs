using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestA
{
	[SerializeField]
	string str = "AAAAAAAAAAA";
}
[System.Serializable]
public class TestB : TestA
{
	[SerializeField]
	int aa = 100;
}


public class TEST : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		TestA b = new TestB();
		FileAccess.FileAccessor.SaveObject<TestA>("Test", "test", ref b);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
