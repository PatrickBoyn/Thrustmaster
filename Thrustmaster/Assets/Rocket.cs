using System;
using UnityEngine;

public class Rocket : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        ProcessUpdate();
	}

    void ProcessUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            print("Spacebar was pressed!");
        }
    }
}
