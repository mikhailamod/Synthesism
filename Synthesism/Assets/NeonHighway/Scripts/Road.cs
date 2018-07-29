using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour {

    [HideInInspector]
    public RoadPath roadPath;

    public void createPath()
    {
        roadPath = new RoadPath(transform.position);
    }

}
