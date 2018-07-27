using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarMovement
{

    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    //[Range(1f, 80f)]
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    public float boostSpeed;
    public float brakeTorque;

    public float brakeSpeed;


    /// <summary>
    /// Adds force in either the left or right direction using the speed variable to determine how fast the car should move
    /// </summary>
    /// <param name="delta"> Value from -1.0f to 1.0f</param>
    public void MoveHorizontal(float delta) {

        float steering = maxSteeringAngle * delta;

        foreach (AxleInfo axleInfo in axleInfos)
        {       
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
        }
    }

    //
    /// <summary>
    /// Adds force in either the up or down direction using the speed variable to determine how fast the car should move
    /// </summary>
    public void MoveVertical(float delta) {

        float speed = delta * maxMotorTorque;

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = speed;
                axleInfo.rightWheel.motorTorque = speed;
            }
        }
    }

    public void brake(Rigidbody rigid)
    {
        setBrakeTorque(brakeTorque);
        //MoveVertical(0);
        //rigid.velocity = Vector3.Lerp(rigid.velocity, Vector3.zero, brakeSpeed * Time.deltaTime);
    }

    public void setBrakeTorque(float val)
    {
        foreach (AxleInfo info in axleInfos)
        {
            info.leftWheel.brakeTorque = val;
            info.rightWheel.brakeTorque = val;
        }
    }

    public void boost(Rigidbody rigid, Vector3 dir)
    {
        rigid.AddForce(dir * boostSpeed, ForceMode.Impulse);
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
    public bool motor;
    public bool steering;
}
