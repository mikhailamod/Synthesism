using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoadMeshCreator
{
    public float roadWidth;

    public Vector3[] getPointsForward(Point[] points, bool closed = false)
    {
        List<Vector3> forwardDirs = new List<Vector3>();


        for(int i = 0; i < points.Length; i++)
        {
            if(i != points.Length-1 || closed)
                forwardDirs.Add(getPointForward(points[i].Position,points[loopIndex(i + 1,points.Length)].Position));
        }

        return forwardDirs.ToArray();

    }

    public Vector3 getPointForward(Vector3 position1, Vector3 position2)
    {
        Vector3 dir = position2 - position1;
        return dir/dir.magnitude;
    }

    public Vector3[] getPointsBinormal(Vector3[] forwardDirs, Vector3 up)
    {
        List<Vector3> binormals = new List<Vector3>();

        for (int i = 0; i < forwardDirs.Length; i++)
        {
            binormals.Add(getPointBinormal(forwardDirs[i],up));  
        }

        return binormals.ToArray();
    }

    public Vector3[] getPointsBinormal(Point[] points, Vector3 up, bool closed = false)
    {
        List<Vector3> binormals = new List<Vector3>();

        for (int i = 0; i < points.Length; i++)
        {
            if (i != points.Length - 1 || closed)
            {
                Vector3 dir = getPointForward(points[i].Position, points[loopIndex(i + 1, points.Length)].Position);
                binormals.Add(getPointBinormal(dir, up));
            }
        }

        return binormals.ToArray();
    }

    public Vector3 getPointBinormal(Vector3 forward, Vector3 up)
    {
        return Vector3.Cross(up, forward).normalized;
    }
    
    public Vector3[] getPointsNormal(Vector3[] forwardDirs, Vector3[] binormals)
    {
        List<Vector3> normals = new List<Vector3>();

        for(int i = 0; i < forwardDirs.Length; i++)
        {
            normals.Add(getPointNormal(forwardDirs[i], binormals[i]));
        }

        return normals.ToArray();
    }

    public Vector3[] getPointsNormal(Point[] points, Vector3 up, bool closed = false)
    {
        Vector3[] dirs = getPointsForward(points, closed);
        Vector3[] binormals = getPointsBinormal(dirs, up);
        return getPointsNormal(dirs, binormals);
    }

    public Vector3 getPointNormal(Vector3 forward, Vector3 binormal)
    {
        return Vector3.Cross(forward, binormal);
    }

    public Vector3[] getMeshVertices(Point[] points, Vector3 up, bool closed = false)
    {
        List<Vector3> vertices = new List<Vector3>();

        Vector3[] binormals = getPointsBinormal(points, up, closed);
        for(int i = 0; i < binormals.Length; i++)
        {
            Vector3 rightVertex = points[i].Position + binormals[i] * roadWidth * 0.5f;
            Vector3 leftVertex = points[i].Position - binormals[i] * roadWidth * 0.5f;

            vertices.Add(leftVertex); vertices.Add(rightVertex);

        }
        return vertices.ToArray();
    }

    public int[] getMeshTris(int vertexCount, bool closed = false)
    {
        List<int> tris = new List<int>();

        for(int i = 0; i < vertexCount/2; i++)
        {
            if (i != vertexCount/2 - 1 || closed)
            {
                //First Triangle
                tris.Add(i * 2);
                tris.Add(loopIndex((i+1) * 2, vertexCount));
                tris.Add((i * 2) + 1);
                //Second Triangle
                tris.Add((i * 2) + 1);
                tris.Add(loopIndex((i + 1) * 2, vertexCount));
                tris.Add(loopIndex(((i + 1) * 2) + 1, vertexCount));
            }
        }

        return tris.ToArray();

    }

    public Vector3[] pointToMeshNormals(Point[] points, Vector3 up, bool closed = false)
    {
        Vector3[] normals = getPointsNormal(points, up, closed);

        Vector3[] meshNormals = new Vector3[normals.Length * 2];
        for(int i = 0; i < meshNormals.Length; i++)
        {
            meshNormals[i] = normals[i / 2];
        }

        return meshNormals;
    }

    //Will have a bug
    public Vector2[] getMeshUV(int vertexCount)
    {
        Vector2[] uvs = new Vector2[vertexCount];

        for(int i = 0; i < vertexCount/2; i++)
        {
            int index = i * 2;
            float percentageComplete = i / (float)vertexCount * 2;
            uvs[index] = new Vector2(0, percentageComplete);
            uvs[index + 1] = new Vector2(1, percentageComplete);
        }

        return uvs;

    }

    public void generateRoadMesh(Point[] points, Vector3 up, bool closed = false)
    {
        GameObject meshObj = new GameObject("Generate Road Mesh");
        MeshFilter mf = meshObj.AddComponent<MeshFilter>();
        meshObj.AddComponent<MeshRenderer>();

        if (mf.sharedMesh == null)
            mf.sharedMesh = new Mesh();

        Mesh mesh = mf.sharedMesh;

        Vector3[] vertices = getMeshVertices(points, up, closed);
        Vector3[] normals = pointToMeshNormals(points, up, closed);
        Vector2[] UVs = getMeshUV(vertices.Length);
        int[] tris = getMeshTris(vertices.Length, closed);

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = UVs;
        mesh.triangles = tris;


    }

    private int loopIndex(int index, int totalSize)
    {
        return (index + totalSize) % totalSize;
    }
}
