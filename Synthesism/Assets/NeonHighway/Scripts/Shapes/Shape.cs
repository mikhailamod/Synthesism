using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Shape
{
    public abstract Vector3[] getMeshVertices(Point[] points, Vector3[] binormals, Vector3 offset, Vector3 up, Vector2 completion, bool closed = false);
    public abstract int[] getMeshTris(int vertexCount, Winding winding, Vector2 completion, bool closed = false);
    public abstract Vector3[] getNormals(Point[] points, Vector3[] normals, Vector3 up, Winding winding, Vector2 completion, bool closed = false);
    public abstract Vector2[] getMeshUV(int vertexCount);
}

public enum Winding
{
    FORWARD,
    INVERTED,
    TWOFACING
}

