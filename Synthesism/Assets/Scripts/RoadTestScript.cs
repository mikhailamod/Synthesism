using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadTestScript : MonoBehaviour {

    public float spacing;
    public float size;

    public Road r;

	void Start ()
    {
        foreach(Point pos in r.roadPath.getRoadPathPoints(spacing))
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.position = pos.Position;
            go.transform.localScale = Vector3.one * spacing * size * 0.5f;
            go.transform.SetParent(transform);
        }
	}
	
}
