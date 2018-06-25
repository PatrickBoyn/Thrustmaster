using System;
using UnityEngine;

public class Rocket : MonoBehaviour {
    Rigidbody rigidbody;


	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        ProcessUpdate();
	}

    void ProcessUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddRelativeForce(Vector3.up);
        }
        if (Input.GetKey(KeyCode.A))
        {
            print("Rotating Left!");
        }else if (Input.GetKey(KeyCode.D))
        {
            print("Rotating Right!");
        }
    }
}
