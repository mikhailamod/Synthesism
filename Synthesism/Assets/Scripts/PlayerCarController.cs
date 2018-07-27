using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CarMovement))]
public class PlayerCarController : MonoBehaviour {

	//Get Access to CarMovement Script
	private CarMovement car; 

	void Start () {
		//actually gets access to the instance of the class so you can use its methods
		car = GetComponent<CarMovement>();
	}
	
	void Update () {
		MoveVehicle();
	}

	void MoveVehicle() {
		//Retrieve left or right input
		if(Input.GetButton("Horizontal")){
			car.MoveHorizontal();
		}
		//Retrieves up or down input
		else if(Input.GetButton("Vertical")){
			car.MoveVertical();
		}
	}

}
