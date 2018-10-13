using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO: Inherit from CarController base class
[RequireComponent(typeof(Rigidbody))]
public class PlayerCarController : CarController {

    public Path path;
    public float minNodeDistance = 20f;
    public bool debugMode = false;

    private List<Node> nodes;
    private int currentNodeIndex;
    private int nodeCount;

    private void Awake()
    {
        currentNodeIndex = 0;
        nodeCount = 0;
    }

    public void LoadNodes()
    {
        nodes = path.getNodeList();
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

	public override void MoveVehicle()
    {
        //gets inital values to determine how the vehicle should move
        float inputSpeed = Input.GetAxis("Vertical");
        
		//Retrieve left or right input
        carMovementProperties.MoveHorizontal(Input.GetAxis("Horizontal"));      

        //looks for appropriate case to move the car otherwise the brake is applied
        if((inputSpeed > 0 && carMovementProperties.GetSpeed() >= 0) || (inputSpeed < 0 && carMovementProperties.GetSpeed() <= 0)) {
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