using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO: Inherit from CarController base class
[RequireComponent(typeof(Rigidbody))]
public class PlayerCarController : MonoBehaviour {

    //Add float carMass

	//Get Access to CarMovement Script
    [Header("Movement Properties")]
	public CarMovement carMovementProperties;

    private Rigidbody rigid; 

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void FixedUpdate () {
		MoveVehicle();
	}

	void MoveVehicle()
    {
        //gets inital values to determine how the vehicle should move
        float speed = carMovementProperties.GetSpeed();
        float inputSpeed = Input.GetAxis("Vertical");
        
		//Retrieve left or right input
        carMovementProperties.MoveHorizontal(Input.GetAxis("Horizontal"));      

        //looks for appropriate case to move the car otherwise the brake is applied
        if((inputSpeed > 0 && speed >= 0) || (inputSpeed < 0 && speed <= 0)) {
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