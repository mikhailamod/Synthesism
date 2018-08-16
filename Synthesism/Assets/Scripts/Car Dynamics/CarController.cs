using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CarController : MonoBehaviour {
	
	//Get Access to CarMovement Script
    [Header("Movement Properties")]
	public CarMovement carMovementProperties;

	//Method which moves the vehicle 
	public abstract void MoveVehicle();
}
