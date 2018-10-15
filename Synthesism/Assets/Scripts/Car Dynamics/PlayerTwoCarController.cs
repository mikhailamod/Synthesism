using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO: Inherit from CarController base class
[RequireComponent(typeof(Rigidbody))]
public class PlayerTwoCarController : CarController {

    void FixedUpdate () {
		MoveVehicle();
	}

	public override void MoveVehicle()
    {
        //gets inital values to determine how the vehicle should move
        float inputSpeed = Input.GetAxis("Vertical2");
        
		//Retrieve left or right input
        carMovementProperties.MoveHorizontal(Input.GetAxis("Horizontal2"));      

        //looks for appropriate case to move the car otherwise the brake is applied
        if((inputSpeed > 0 && carMovementProperties.GetSpeed() >= 0) || (inputSpeed < 0 && carMovementProperties.GetSpeed() <= 0)) {
            carMovementProperties.MoveVertical(Input.GetAxis("Vertical2"));
        }
        else {
            carMovementProperties.brake();
        }
		
        //Force break
        if(Input.GetButton("Handbrake2"))
        {
            carMovementProperties.brake();
        }

        carMovementProperties.RotateWheels();
    }

    //not implement bc assuming we'll only use one PlayerController
    public override int getCurrentNodeCount()
    {
        throw new System.NotImplementedException();
    }

    //not implement bc assuming we'll only use one PlayerController
    protected override void UpdateWaypoint()
    {
        throw new System.NotImplementedException();
    }

    public override void SetPath(Path p)
    {
        throw new System.NotImplementedException();
    }
}