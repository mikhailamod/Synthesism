﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO: Inherit from CarController base class
[RequireComponent(typeof(Rigidbody))]
public class PlayerCarController : CarController {

    public ActivatePickup pickupHandler;

    void FixedUpdate () {
		MoveVehicle();
        pickupHandler = GetComponent<ActivatePickup>();
	}

	public override void MoveVehicle()
    {
        //gets inital values to determine how the vehicle should move

        //Retrieve left or right input
        carMovementProperties.MoveHorizontal(Input.GetAxis("Horizontal"));

        //looks for appropriate case to move the car otherwise the brake is applied
        float inputSpeed = Input.GetAxis("Vertical");

        if (inputSpeed > 0 || (inputSpeed < 0 && carMovementProperties.GetSpeed() <= 0))
        {
            carMovementProperties.MoveVertical(Input.GetAxis("Vertical"));
        }
        else if (inputSpeed < 0 && carMovementProperties.GetSpeed() > 0)
        {
            carMovementProperties.brake();
        }
        else
        {
            carMovementProperties.setMotorTorque(0);
        }

        //Force break
        if (Input.GetButton("Handbrake"))
        {
            carMovementProperties.brake();
        }

        //Force break
        if(Input.GetButton("FireSpike") && carMovementProperties.hasSpike)
        {
            pickupHandler.FireSpike(transform.forward);
            carMovementProperties.hasSpike = false;
        }


        carMovementProperties.RotateWheels();
    }

}