using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CarController : MonoBehaviour {
	
	//Get Access to CarMovement Script
    [Header("Movement Properties")]
	public CarMovement carMovementProperties;

	//Method which moves the vehicle 
	public abstract void MoveVehicle();

    //get the number of nodes the car has passed
    public abstract int getCurrentNodeCount();

    //if within a certain distance to the next node, mark it
    //as passed and update currentNode
    protected abstract void UpdateWaypoint();

    public abstract void SetPath(Path p);
}
