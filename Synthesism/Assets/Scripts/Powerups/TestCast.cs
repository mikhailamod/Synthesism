using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCast : MonoBehaviour {

    void Update()
    {
        RaycastHit hit;
		Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
		//Debug.Log("Did not Hit");
    }
}
