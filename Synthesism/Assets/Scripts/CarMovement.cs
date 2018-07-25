using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement: MonoBehaviour {

	public float speed;
	private Rigidbody car;
	
	// Use this for initialization
	void Start () {
		car = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void MoveHorizontal() {
		car.AddForce(transform.right * speed * Time.deltaTime * GetAxisMovement(Input.GetAxis("Horizontal")), ForceMode.Acceleration);
	}

	public void MoveVertical() {
		car.AddForce(transform.forward * speed * Time.deltaTime * GetAxisMovement(Input.GetAxis("Vertical")), ForceMode.Acceleration);
	}

	int GetAxisMovement(float axis) {
		return axis>0?1:-1;
	}
}
