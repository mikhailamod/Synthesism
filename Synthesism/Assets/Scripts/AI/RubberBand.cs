using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubberBand : MonoBehaviour {

    float originalMaxSpeed;
    float originalMaxTorque;

    public float decreaseSpeedFactor = 0.75f;
    public float decreaseTorqueFactor = 0.5f;
    public float increaseSpeedFactor = 1.25f;
    public float increaseTorqueFactor = 1.5f;

    public int nodeLimit;
    public PlayerCarController player;
    AICarController aiCarController;

	// Use this for initialization
	void Start () {
        aiCarController = GetComponent<AICarController>();
        originalMaxSpeed = aiCarController.maxSpeed;
        originalMaxTorque = aiCarController.carMovementProperties.maxMotorTorque;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isFarAhead())
        {
            //slow car down
            aiCarController.maxSpeed = originalMaxSpeed * decreaseSpeedFactor;
            aiCarController.carMovementProperties.maxMotorTorque = originalMaxTorque * decreaseTorqueFactor;
        }
        else if (isFarBehind())
        {
            //speed up
            aiCarController.maxSpeed = originalMaxSpeed * increaseSpeedFactor;
            aiCarController.carMovementProperties.maxMotorTorque = originalMaxTorque * increaseTorqueFactor;
        }
        else
        {
            //return to normal
            aiCarController.maxSpeed = originalMaxSpeed;
            aiCarController.carMovementProperties.maxMotorTorque = originalMaxTorque;
        }
	}

    //return true if AI is far ahead of Player
    bool isFarAhead()
    {
        int playerPos = player.getCurrentNodeCount();
        int aiPos = aiCarController.getCurrentNodeCount();

        if(aiPos - playerPos > nodeLimit) { return true; }
        else { return false; }
    }
    bool isFarBehind()
    {
        int playerPos = player.getCurrentNodeCount();
        int aiPos = aiCarController.getCurrentNodeCount();

        if (playerPos - aiPos > nodeLimit) { return true; }
        else { return false; }
    }
}
