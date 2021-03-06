﻿using System.Collections;
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
    public float maxSpeed = 80f;

    public Rigidbody carRigidBody;

    public bool hasSpike; //powerup
    public bool hasOil; //powerup

	public float stiffnessValue; // original stiffness
	public float slipValue; // new stiffness
	public float slipTime; // how long the new stiffness should last
    
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
                axleInfo.leftWheel.wheelCollider.steerAngle = steering;
                axleInfo.rightWheel.wheelCollider.steerAngle = steering;
            }
        }
    }

    //
    /// <summary>
    /// Adds force in either the up or down direction using the speed variable to determine how fast the car should move
    /// </summary>
    public void MoveVertical(float delta) {
        float speed = delta * maxMotorTorque;
        setMotorTorque(speed);
        setBrakeTorque(0);
    }

    //Rotates wheels vertically and horrizontally 
    public void RotateWheels() {
        foreach (AxleInfo info in axleInfos)
        {
            ChangeWheelPosition(info.leftWheel.wheelCollider, info.leftWheel.wheel);
            ChangeWheelPosition(info.rightWheel.wheelCollider, info.rightWheel.wheel);
        }
    }

    //Modifies a single wheels rotation - helper function
	private void ChangeWheelPosition(WheelCollider collider, Transform transform)
	{
        Quaternion quaternion = transform.rotation;
		Vector3 position = transform.position;
		collider.GetWorldPose(out position, out quaternion);
		transform.position = position;
		transform.rotation = quaternion;
	}

    //increases the braketorque to halt the vehicle and reduces motortorque to 0
    public void brake()
    {
        setBrakeTorque(brakeTorque);
        setMotorTorque(0);
    }

    //sets the brake torque on each wheel for each axle
    public void setBrakeTorque(float val)
    {
        foreach (AxleInfo info in axleInfos)
        {
            info.leftWheel.wheelCollider.brakeTorque = val;
            info.rightWheel.wheelCollider.brakeTorque = val;
        }
    }

    public void setAxelStiffness(float val) {
        foreach (AxleInfo info in axleInfos)
        {
            WheelFrictionCurve leftFriction = info.leftWheel.wheelCollider.sidewaysFriction;
            leftFriction.stiffness = val;
            info.leftWheel.wheelCollider.sidewaysFriction = leftFriction;

            WheelFrictionCurve rightFriction = info.rightWheel.wheelCollider.sidewaysFriction;
            rightFriction.stiffness = val;
            info.rightWheel.wheelCollider.sidewaysFriction = rightFriction;
        }
    }

    //sets the motor torque on each wheel for each axle
    public void setMotorTorque(float val)
    {
        foreach (AxleInfo info in axleInfos)
        {
            if(val > maxMotorTorque)
            {
                info.leftWheel.wheelCollider.motorTorque = maxMotorTorque;
                info.rightWheel.wheelCollider.motorTorque = maxMotorTorque;
            }
            else
            {
                info.leftWheel.wheelCollider.motorTorque = val;
                info.rightWheel.wheelCollider.motorTorque = val;
            }
        }
    }

    //get the average current speed of all wheels based on the circumference
    public float GetSpeed() {
        float fl =  (float)(2 * Mathf.PI *
            axleInfos[0].leftWheel.wheelCollider.radius *
            axleInfos[0].leftWheel.wheelCollider.rpm * 0.06);

        float fr = (float)(2 * Mathf.PI *
            axleInfos[0].rightWheel.wheelCollider.radius *
            axleInfos[0].rightWheel.wheelCollider.rpm * 0.06);

        float bl = (float)(2 * Mathf.PI *
            axleInfos[1].leftWheel.wheelCollider.radius *
            axleInfos[1].leftWheel.wheelCollider.rpm * 0.06);

        float br = (float)(2 * Mathf.PI *
            axleInfos[1].rightWheel.wheelCollider.radius *
            axleInfos[1].rightWheel.wheelCollider.rpm * 0.06);

        float x = (fl + fr + bl + br) / 4;
        //if (x > maxSpeed) { x = maxSpeed; }//this is a hack for engine sound at the moment
        return x;
    }

    //get the average rpm of all four wheels
    public float GetRpm()
    {
        
        float fl = axleInfos[0].leftWheel.wheelCollider.rpm;
        float fr = axleInfos[0].rightWheel.wheelCollider.rpm;
        float bl = axleInfos[1].leftWheel.wheelCollider.rpm;
        float br = axleInfos[1].rightWheel.wheelCollider.rpm;

        float x = (fl + fr + bl + br) / 4;
        if(x > maxMotorTorque) { x = maxMotorTorque; }
        return x;
    }

    //changes stiffness for time period sliptime
	public IEnumerator slip() {
		setAxelStiffness(slipValue);
		yield return new WaitForSeconds(slipTime);
		setAxelStiffness(stiffnessValue);
	}


}

[System.Serializable]
public class AxleInfo
{
    public WheelInfo leftWheel;
    public WheelInfo rightWheel;
    public bool motor;
    public bool steering;
}

[System.Serializable]
public class WheelInfo
{
    public Transform wheel;
    public WheelCollider wheelCollider;
}
