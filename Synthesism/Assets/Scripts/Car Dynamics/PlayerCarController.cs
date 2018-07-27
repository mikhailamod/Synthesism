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

    void Update () {
		MoveVehicle();
	}

	void MoveVehicle()
    {
		//Retrieve left or right input
        carMovementProperties.MoveHorizontal(Input.GetAxis("Horizontal"));

        /*
         * If getAxis() > 0 wants to go forward else backwards or none.
         * Look at previous to determine function
         * 
        */
        if (Input.GetKey(KeyCode.LeftShift))
            carMovementProperties.boost(rigid, transform.forward);
        else
            carMovementProperties.MoveVertical(Input.GetAxis("Vertical"));
		
        if(Input.GetKey(KeyCode.Space))
        {
            carMovementProperties.brake(rigid);
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            carMovementProperties.setBrakeTorque(0);
        }
        
    }

}

enum MoveStates
{
    FORWARDS,
    BACKWARDS,
    NONE
}
