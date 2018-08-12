using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoadSettings
{
    public Color nodeColor = Color.green;
    public Color lineColor = Color.yellow;
    public Color curveColor = Color.red;
    public Color pathColor = Color.blue;
    public Color nodeForwardColor = Color.white;
    public Color nodeBinormalColor = Color.cyan;
    public Color nodeNormalColor = Color.black;
    public Color vertexColor = Color.white;
    public float curveWidth = 4;
    public float nodeSize = 1;
    public float nodeArrowSize = 1;
    public float spacing;
    public float size;

    public bool showCurveProperties = false;
    public bool showNodeProperties = false;
    public bool showPathProperties = false;
    public bool showPathArrowProperties = false;
    public bool showPathColorProperties = false;
    public bool showMeshProperties = false;
    public bool showPathCenter = false;

    public bool showCurve = true;
    public bool showPoints = false;
    public bool showPointForwards = false;
    public bool showPointBinormals = false;
    public bool showPointNormals = false;
    public bool showVertices = false;
    public bool showTris = false;
    public bool showMeshNormals = false;
    public bool showUVs = false;
}

public enum ShapeToDraw
{
    ROAD,
    WALL
}
