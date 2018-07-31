using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoadMeshCreator
{
    public float roadSize;

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

    private int loopIndex(int index, int totalSize)
    {
        return (index + totalSize) % totalSize;
    }
}
