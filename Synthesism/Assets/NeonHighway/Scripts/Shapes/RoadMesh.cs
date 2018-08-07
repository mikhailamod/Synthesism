using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadMesh : Shape
{

    private float roadWidth;

    public RoadMesh(float roadWidth)
    {
        this.roadWidth = roadWidth;
    }

    public override int[] getMeshTris(int vertexCount, bool closed = false)
    {
        List<int> tris = new List<int>();

        for (int i = 0; i < vertexCount / 2; i++)
        {
            if (i != vertexCount / 2 - 1 || closed)
            {
                //First Triangle
                tris.Add(i * 2);
                tris.Add(((i + 1) * 2) % vertexCount);
                tris.Add((i * 2) + 1);
                //Second Triangle
                tris.Add((i * 2) + 1);
                tris.Add(((i + 1) * 2) % vertexCount);
                tris.Add(((i + 1) * 2 + 1) % vertexCount);
            }
        }

        return tris.ToArray();
    }

    public override Vector2[] getMeshUV(int vertexCount)
    {
        Vector2[] uvs = new Vector2[vertexCount];

        for (int i = 0; i < vertexCount / 2; i++)
        {
            int index = i * 2;
            float percentageComplete = i / (float)vertexCount * 2;
            uvs[index] = new Vector2(0, percentageComplete);
            uvs[index + 1] = new Vector2(1, percentageComplete);
        }

        return uvs;
    }

    public override Vector3[] getMeshVertices(Point[] points, Vector3[] binormals, Vector3 offset, Vector3 up, bool closed = false)
    {
        List<Vector3> vertices = new List<Vector3>();

        for (int i = 0; i < binormals.Length; i++)
        {
            Vector3 rightVertex = points[i].Position + binormals[i] * roadWidth * 0.5f;
            rightVertex += offset;
            Vector3 leftVertex = points[i].Position - binormals[i] * roadWidth * 0.5f;
            leftVertex += offset;

            vertices.Add(leftVertex); vertices.Add(rightVertex);

        }
        return vertices.ToArray();
    }

    public override Vector3[] getNormals(Point[] points, Vector3[] normals, Vector3 up, bool invertNormals, bool closed = false)
    {
        Vector3[] meshNormals = new Vector3[normals.Length * 2];
        for (int i = 0; i < meshNormals.Length; i++)
        {
            meshNormals[i] = normals[i / 2];
            if (invertNormals)
                meshNormals[i] *= -1;
        }

        return meshNormals;
    }
}
