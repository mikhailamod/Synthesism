using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarController : MonoBehaviour, ICarController {

    public Transform path;

    [Header("Movement Properties")]
    public CarMovement carMovement;

    private List<Transform> nodes;
    private int currentNode;
    public float minNodeDistance = 0.5f;
    public float currentSpeed;
    public float maxSpeed = 100f;
 
    // Use this for initialization
    void Start () {
        currentNode = 0;
        currentSpeed = 0f;
        initializePath(path);
	}
	
	void FixedUpdate () {
        MoveVehicle();
        UpdateWaypoint();
    }

    public void MoveVehicle()
    {
        float steerAmount = SteerCar();
        float driveAmount = Drive();
        carMovement.MoveHorizontal(steerAmount);
        carMovement.MoveVertical(driveAmount);
        carMovement.RotateWheels();
    }

    private void UpdateWaypoint()
    {
        float distance = Vector3.Distance(transform.position, nodes[currentNode].position);
        if (distance < minNodeDistance)
        {
            if(currentNode == nodes.Count-1)
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
            }
        }
    }

    private float SteerCar()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        relativeVector = relativeVector/relativeVector.magnitude;//Vector3.Normalize return positive number
        return relativeVector.x;
    }

    private float Drive()
    {
        float amount;
        currentSpeed = carMovement.GetSpeed();
        if(currentSpeed < maxSpeed)
        {
            amount = 1.0f;
        } else {
            amount = 0.0f;
        }
        return amount;
    }

    private void initializePath(Transform path)
    {
        Transform[] transforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < transforms.Length; i++)
        {
            if (transforms[i] != path.transform)
            {
                nodes.Add(transforms[i]);
            }
        }
    }//end method

    public float getCurrentSpeed()
    {
        return currentSpeed;
    }

    public float getRpm()
    {
        return carMovement.GetRpm();
    }
}
