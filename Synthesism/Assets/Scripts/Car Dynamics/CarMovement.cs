using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarMovement
{

    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have

    [Range(1f, 15000f)]
    public float speed;

    //adds force in either the left or right direction using the speed variable to determine how fast the car should move
    public void MoveHorizontal() {
        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                //axleInfo.leftWheel.steerAngle = steering;
                //axleInfo.rightWheel.steerAngle = steering;
            }
        }
    }
	
	//adds force in either the up or down direction using the speed variable to determine how fast the car should move
	public void MoveVertical() {
        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = speed;
                axleInfo.rightWheel.motorTorque = speed;
            }
        }
    }

	//Direction (vertical/horizontal) the force needs to be applied
	int GetAxisMovement(float axis) {
		return axis>0?1:-1;
	}
}

public abstract class CarController : MonoBehaviour
{
    public CarMovement carMovement;
}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}
