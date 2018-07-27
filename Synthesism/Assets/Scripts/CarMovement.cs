using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarMovement: MonoBehaviour {

	//Speed of the vehicle as a slider
	[Range(1f, 15000f)]
	public float speed;

	private Rigidbody car;
	
	void Start () {
		//Get Access to the car's rigidbody
		car = GetComponent<Rigidbody>();
	}
	
	void Update () { }

	//adds force in either the left or right direction using the speed variable to determine how fast the car should move
	public void MoveHorizontal() {
		car.AddForce(transform.right * speed * Time.deltaTime * GetAxisMovement(Input.GetAxis("Horizontal")), ForceMode.Acceleration);
	}
	
	//adds force in either the up or down direction using the speed variable to determine how fast the car should move
	public void MoveVertical() {
		car.AddForce(transform.forward * speed * Time.deltaTime * GetAxisMovement(Input.GetAxis("Vertical")), ForceMode.Acceleration);
	}

	//Direction (vertical/horizontal) the force needs to be applied
	int GetAxisMovement(float axis) {
		return axis>0?1:-1;
	}
}
