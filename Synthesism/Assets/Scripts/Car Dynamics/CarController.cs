using UnityEngine;

//An object with CarController component is an object that has
//front and rear WheelColliders, which CarController will manipulate to move the object
//It does so by having a CarMovement component
//It also has a path (a set of nodes) which it will follow
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

    //set the Path that the car uses for it's waypoints
    public abstract void SetPath(Path p);
}
