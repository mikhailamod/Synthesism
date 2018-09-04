using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarController : CarController {

    public GameObject path;
    public Vector3 centreOfMass;

    [Header("AI Movement Properties")]
    public float minNodeDistance = 0.5f;
    public float currentSpeed;
    public float maxSpeed = 100f;
    public float minSpeed = 40f;
    public float maxCornerSpeed = 50f;
    public bool isBraking = false;

    [Header("Sensor Properties")]
    public float sensorLength = 5f;
    public Vector3 frontSensorOffset = Vector3.zero;
    public Vector3 sideSensorOffset = Vector3.zero;
    public float frontSensorAngle = 30f;

    public List<string> listOfTags;

    //Private
    private List<Node> nodes;
    private int currentNode;

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

    public override void MoveVehicle()
    {
        float turnOffset = UseSensors();
        float steerAmount = SteerCar();
        float driveAmount = Drive();
        carMovementProperties.MoveHorizontal(steerAmount+turnOffset);
        carMovementProperties.MoveVertical(driveAmount);
        Brake();
        carMovementProperties.RotateWheels();
        if(isBraking && currentSpeed < (maxSpeed * driveAmount))
        {
            isBraking = false;
        }
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

        //front left
        if (Physics.Raycast(frontLeftSensorPos, transform.forward, out raycastHit, sensorLength))
        {
            string theTag = raycastHit.collider.tag;
            if (listOfTags.Contains(theTag))
            {
                oneHit = true;
                turnOffset += 1f;
                Debug.DrawLine(frontLeftSensorPos, raycastHit.point, Color.red);
            }
        }

        //front left angled
        if (Physics.Raycast(frontLeftSensorPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up)*transform.forward, out raycastHit, sensorLength))
        {
            string theTag = raycastHit.collider.tag;
            if (listOfTags.Contains(theTag))
            {
                oneHit = true;
                turnOffset += 0.75f;
                Debug.DrawLine(frontLeftSensorPos, raycastHit.point, Color.red);
            }
            
        }

        //front right
        if (Physics.Raycast(frontRightSensorPos, transform.forward, out raycastHit, sensorLength))
        {
            string theTag = raycastHit.collider.tag;
            if (listOfTags.Contains(theTag))
            {
                oneHit = true;
                turnOffset -= 1f;
                Debug.DrawLine(frontRightSensorPos, raycastHit.point, Color.red);
            }
            
        }

        //front right angled
        if (Physics.Raycast(frontRightSensorPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out raycastHit, sensorLength))
        {
            string theTag = raycastHit.collider.tag;
            if (listOfTags.Contains(theTag))
            {
                oneHit = true;
                turnOffset -= 0.75f;
                Debug.DrawLine(frontRightSensorPos, raycastHit.point, Color.red);
            }
            
        }

        if (oneHit == true && turnOffset == 0)
        {
            //front
            if (Physics.Raycast(frontSensorPos, transform.forward, out raycastHit, sensorLength))
            {
                string theTag = raycastHit.collider.tag;
                if (listOfTags.Contains(theTag))
                {
                    oneHit = true;
                    if (raycastHit.normal.x < 0) { turnOffset = -1f; }
                    else { turnOffset = 1f; }
                    Debug.DrawLine(frontSensorPos, raycastHit.point, Color.red);
                }
                
            }
            return turnOffset;
        }
        return turnOffset;
    }

    private void UpdateWaypoint()
    {
        float distance = Vector3.Distance(transform.position, nodes[currentNode].transform.position);
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
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].transform.position);
        relativeVector = relativeVector/relativeVector.magnitude;//Vector3.Normalize returns positive number only
        return relativeVector.x;
    }

    private float Drive()
    {
        currentSpeed = carMovementProperties.GetSpeed();
        Node nodeInfo = nodes[currentNode];

        //check current Node if we should break and what speed we should be going at
        if(nodeInfo != null)
        {
            if (nodeInfo.isABreakingZone && currentSpeed > minSpeed) { isBraking = true; }
            else { isBraking = false; }

            if (currentSpeed < maxSpeed) { return nodeInfo.speedFactor; }
            else { return 0.0f; }
        }

        //should there be no Node info for some reason, go full speed/brake
        else if(currentSpeed < maxSpeed && !isBraking)
        {
            return 1.0f;
        } else {
            return 0.0f;
        }
    }

    private void Brake()
    {
        if(isBraking)
        {
            carMovementProperties.brake();
        }
        else
        {
            carMovementProperties.setBrakeTorque(0);
        }
    }

    //populate nodes list with nodes from Path
    private void initializePath(GameObject path)
    {
        Path p = path.GetComponent<Path>();
        nodes = p.getNodeList();
    }//end method

    public float getCurrentSpeed()
    {
        return currentSpeed;
    }

    public float getRpm()
    {
        return carMovementProperties.GetRpm();
    }
}
