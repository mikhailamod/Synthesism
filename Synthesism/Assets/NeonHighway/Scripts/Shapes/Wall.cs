using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Shape
{

    private float height;

    public Wall(float height) { this.height = height;  }

    public override int[] getMeshTris(int vertexCount, Winding winding, Vector2 completion, bool closed = false)
    {
        List<int> tris = new List<int>();
        for (int i = 0; i < vertexCount / 2; i++)
        {
            if (i != vertexCount / 2 - 1 || (closed && completion.magnitude == 1.0f))
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
        if (winding == Winding.INVERTED) tris.Reverse();
        else if(winding == Winding.TWOFACING)
        {
            int[] reverseTris = new int[tris.Count];
            tris.CopyTo(reverseTris);
            tris.AddRange(reverseTris);
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
            float dampingPercent = 1 - Mathf.Abs(2 * percentageComplete - 1);
            uvs[index] = new Vector2(0, dampingPercent);
            uvs[index + 1] = new Vector2(1, dampingPercent);
        }

        return uvs;
    }

    public override Vector3[] getMeshVertices(Point[] points, Vector3[] binormals, Vector3 offset, Vector3 up,Vector2 completion, bool closed = false)
    {
        List<Vector3> vertices = new List<Vector3>();

        for (int i = 0; i < binormals.Length; i++)
        {
            float percentage = i / (float)binormals.Length;
            if (percentage < completion.x || percentage > completion.y) continue;

            Vector3 bottomVertex = points[i].Position;
            bottomVertex = new Vector3(bottomVertex.x, bottomVertex.y + offset.y, bottomVertex.z + offset.z);
            bottomVertex += binormals[i] * offset.x;
            Vector3 topVertex = bottomVertex + up * height;

            vertices.Add(bottomVertex); vertices.Add(topVertex);
        }
        return vertices.ToArray();
    }

    public override Vector3[] getNormals(Point[] points, Vector3[] normals, Vector3 up, Winding winding, Vector2 completion, bool closed = false)
    {
        Vector3[] meshNormals = new Vector3[normals.Length * 2];
        for (int i = 0; i < meshNormals.Length; i++)
        {
            meshNormals[i] = normals[i / 2];
            if (winding == Winding.INVERTED)
                meshNormals[i] *= -1;
        }

        return meshNormals;
    }
}
