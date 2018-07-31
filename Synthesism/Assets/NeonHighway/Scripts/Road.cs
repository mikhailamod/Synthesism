using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour {

    [HideInInspector]
    public RoadPath roadPath;
    [HideInInspector]
    public RoadMeshCreator meshCreator;

    public void createPath()
    {
        roadPath = new RoadPath(transform.position);
    }

}
