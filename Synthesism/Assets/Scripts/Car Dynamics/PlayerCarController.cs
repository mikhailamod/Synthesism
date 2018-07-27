using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO: Inherit from CarController base class
public class PlayerCarController : MonoBehaviour {

	//Get Access to CarMovement Script
    [Header("Movement Properties")]
	public CarMovement carMovementProperties;
    
	void Update () {
		MoveVehicle();
	}

	void MoveVehicle() {
		//Retrieve left or right input
		if(Input.GetButton("Horizontal")){
            carMovementProperties.MoveHorizontal();
		}
		//Retrieves up or down input
		else if(Input.GetButton("Vertical")){
            carMovementProperties.MoveVertical();
		}
	}

}
