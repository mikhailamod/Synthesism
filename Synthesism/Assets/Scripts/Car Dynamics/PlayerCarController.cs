﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//TODO: Inherit from CarController base class
[RequireComponent(typeof(Rigidbody))]
public class PlayerCarController : CarController {

    public Path path;
    public float minNodeDistance = 20f;
    public bool debugMode = false;

    public Slider speedSlider;
    public Slider rpmSlider;

    EngineSound engineSound;

    private List<Node> nodes;
    private int currentNodeIndex;
    private int nodeCount;

    private int playerNum = 0;

    private Rigidbody temprb;
    private float pitchAmount = 0f;

    private void Start()
    {
        temprb = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        engineSound = GetComponent<EngineSound>();
        currentNodeIndex = 0;
        nodeCount = 0;
    }

    public void LoadNodes()
    {
        nodes = path.getNodeList();
    }

    public void setPlayerNum(int num)
    {
        playerNum = num;
    }

    private void Update()
    {
        float n = NormalizeSpeed();
        speedSlider.value = n;
        rpmSlider.value = pitchAmount;
    }

    float NormalizeSpeed()
    {
        float normalizedSpeed = temprb.velocity.magnitude;
        normalizedSpeed = Mathf.Abs(normalizedSpeed / 50f);
        return normalizedSpeed;
    }

    void FixedUpdate () {
        if (RaceManager.instance.raceStarted && !debugMode)
        {
            MoveVehicle();
            UpdateWaypoint();
        }
        else if (debugMode)
        {
            MoveVehicle();
            UpdateWaypoint();
        }
		
	}

    public void setPitchAmount(float p)
    {
        pitchAmount = p;
    }

	public override void MoveVehicle()
    {
        //gets inital values to determine how the vehicle should move
        float inputSpeed = Input.GetAxis(ControllerInfo.VERTICAL_MOVES[playerNum]);
        
		//Retrieve left or right input
        carMovementProperties.MoveHorizontal(Input.GetAxis(ControllerInfo.HORIZONTAL_MOVES[playerNum]));      

        //looks for appropriate case to move the car otherwise the brake is applied
        if((inputSpeed > 0 && carMovementProperties.GetSpeed() >= 0) || (inputSpeed < 0 && carMovementProperties.GetSpeed() <= 0)) {
            carMovementProperties.MoveVertical(Input.GetAxis(ControllerInfo.VERTICAL_MOVES[playerNum]));
        }
        else {
            //MusicManager.instance.PlaySoundEffectOnce(MusicManagerInfo.BRAKE_1);
            carMovementProperties.brake();
            engineSound.resetFactor();
        }
		
        //Force break
        if(Input.GetButton(ControllerInfo.HANDBRAKES[playerNum]))
        {
            MusicManager.instance.PlaySoundEffectOnce(MusicManagerInfo.BRAKE_1);
            carMovementProperties.brake();
            engineSound.resetFactor();
        }

        carMovementProperties.RotateWheels();
    }

    public void boost() {
       carMovementProperties.boost(carMovementProperties.carRigidBody, transform.forward);
    }

    protected override void UpdateWaypoint()
    {
        float distance = Vector3.Distance(transform.position, nodes[currentNodeIndex].transform.position);
        if (distance < minNodeDistance)
        {
            nodeCount++;
            if (currentNodeIndex == nodes.Count - 1)
            {
                currentNodeIndex = 0;
            }
            else
            {
                currentNodeIndex++;
            }
        }
    }

    public override int getCurrentNodeCount() { return nodeCount; }

    public override void SetPath(Path p)
    {
        path = p;
    }
}