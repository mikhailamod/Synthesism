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
    public float maxCornerSpeed = 50f;

    [Header("Sensors")]
    public float sensorLength = 5f;
    public Vector3 frontSensorOffset = Vector3.zero;
    public Vector3 sideSensorOffset = Vector3.zero;
    public float frontSensorAngle = 30f;

    public bool isBraking = false;

    public Vector3 centreOfMass;

    // Use this for initialization
    void Start() {
        currentNode = 0;
        currentSpeed = 0f;
        initializePath(path);
        GetComponent<Rigidbody>().centerOfMass = centreOfMass;
    }

    void FixedUpdate() {
        MoveVehicle();
        UpdateWaypoint();
    }

    public void MoveVehicle()
    {
        float turnOffset = UseSensors();
        float steerAmount = SteerCar();
        float driveAmount = Drive();
        carMovement.MoveHorizontal(steerAmount*turnOffset);
        carMovement.MoveVertical(driveAmount);
        Brake();
        carMovement.RotateWheels();
    }

    private float UseSensors()
    {
        float turnOffset = 0f;
        bool oneHit = false;
        RaycastHit raycastHit;
        Vector3 frontSensorPos = transform.position;
        frontSensorPos += transform.forward * frontSensorOffset.z;
        frontSensorPos += transform.up * frontSensorOffset.y;

        Vector3 frontRightSensorPos = Vector3.Scale(frontSensorPos, new Vector3(1, 1, 1));
        frontRightSensorPos += transform.right * sideSensorOffset.x;

        Vector3 frontLeftSensorPos = Vector3.Scale(frontSensorPos, new Vector3(1, 1, 1));
        frontLeftSensorPos -= (transform.right * sideSensorOffset.x);

        //front
        if (Physics.Raycast(frontSensorPos, transform.forward, out raycastHit, sensorLength))
        {
            if (!raycastHit.collider.CompareTag("Track"))
            {
                oneHit = true;
                turnOffset += 0f;
            }
            Debug.DrawLine(frontSensorPos, raycastHit.point, Color.red);
        }

        //front left
        if (Physics.Raycast(frontLeftSensorPos, transform.forward, out raycastHit, sensorLength))
        {
            if (!raycastHit.collider.CompareTag("Track"))
            {
                oneHit = true;
                turnOffset += 1f;
            }
            Debug.DrawLine(frontLeftSensorPos, raycastHit.point, Color.red);
        }

        //front left angled
        else if (Physics.Raycast(frontLeftSensorPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up)*transform.forward, out raycastHit, sensorLength))
        {
            if (!raycastHit.collider.CompareTag("Track"))
            {
                oneHit = true;
                turnOffset += 0.5f;
            }
            Debug.DrawLine(frontLeftSensorPos, raycastHit.point, Color.red);
        }

        //front right
        if (Physics.Raycast(frontRightSensorPos, transform.forward, out raycastHit, sensorLength))
        {
            if (!raycastHit.collider.CompareTag("Track"))
            {
                oneHit = true;
                turnOffset -= 1f;
            }
            Debug.DrawLine(frontRightSensorPos, raycastHit.point, Color.red);
        }

        //front right angled
        else if (Physics.Raycast(frontRightSensorPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out raycastHit, sensorLength))
        {
            if (!raycastHit.collider.CompareTag("Track"))
            {
                oneHit = true;
                turnOffset -= 0.5f;
            }
            Debug.DrawLine(frontRightSensorPos, raycastHit.point, Color.red);
        }

        if (oneHit == true && turnOffset == 0)
        {
            isBraking = true;
            return turnOffset;
        }
        else
        {
            isBraking = false;
            return turnOffset;
        }
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
        if(currentSpeed < maxSpeed && !isBraking)
        {
            amount = 1.0f;
        } else {
            amount = 0.0f;
        }
        return amount;
    }

    private void Brake()
    {
        if(isBraking)
        {
            carMovement.brake();
        }
        else
        {
            carMovement.setBrakeTorque(0);
        }
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
