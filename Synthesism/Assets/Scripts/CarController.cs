using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CarMovement))]
public class CarController : MonoBehaviour {

	//Get Access to CarMovement Script
	private CarMovement car; 

	void Start () {
		//actually gets access to the instance of the class so you can use its methods
		car = GetComponent<CarMovement>();
	}
	
	void Update () {
		
		//Retrieve left or right input
		if(Input.GetButtonDown("Horizontal")){
			car.MoveHorizontal();
		}
		//Retrieves up or down input
		else if(Input.GetButtonDown("Vertical")){
			car.MoveVertical();
		}
	}

}
