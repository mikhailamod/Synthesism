﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO: Inherit from CarController base class
[RequireComponent(typeof(Rigidbody))]
public class PlayerCarController : CarController {

    float playerCarSpeed;

    void Start() {
        playerCarSpeed = carMovementProperties.GetSpeed();
    }

    void FixedUpdate () {
		MoveVehicle();
	}

	public override void MoveVehicle()
    {
        //gets inital values to determine how the vehicle should move
        float inputSpeed = Input.GetAxis("Vertical");
        
		//Retrieve left or right input
        carMovementProperties.MoveHorizontal(Input.GetAxis("Horizontal"));      

        //looks for appropriate case to move the car otherwise the brake is applied
        if((inputSpeed > 0 && playerCarSpeed >= 0) || (inputSpeed < 0 && playerCarSpeed <= 0)) {
            carMovementProperties.MoveVertical(Input.GetAxis("Vertical"));
        }
        else {
            carMovementProperties.brake();
        }
		
        //Force break
        if(Input.GetButton("Handbrake"))
        {
            carMovementProperties.brake();
        }

        carMovementProperties.RotateWheels();
    }
}