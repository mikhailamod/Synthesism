using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoadMeshCreator
{
    public float roadWidth;

    public Material roadMaterial;

    public List<DrawableShape> shapesToRender;

    public RoadMeshCreator()
    {
        shapesToRender = new List<DrawableShape>();
    }

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

    public Vector3[] getMeshVertices(Point[] points, Shape shape, Vector3 offset, Vector3 up, bool closed = false)
    {
        Vector3[] binormals = getPointsBinormal(points, up, closed);
        return shape.getMeshVertices(points, binormals, offset, up, closed);
    }

    public int[] getMeshTris(int vertexCount, Shape shape, Winding winding, bool closed)
    {
        return shape.getMeshTris(vertexCount, winding, closed);
    }

    public Vector3[] pointToMeshNormals(Point[] points, Shape shape, Vector3 up, Winding winding, bool closed = false)
    {
        Vector3[] normals = getPointsNormal(points, up, closed);
        return shape.getNormals(points, normals, up, winding, closed);
    }

    //Will have a bug
    public Vector2[] getMeshUV(int vertexCount, Shape shape)
    {
        return shape.getMeshUV(vertexCount);
    }


    public void generateMeshes(string parentName, Point[] points, Vector3 up,float spacing, bool isClosed = false)
    {
        GameObject parent = new GameObject(parentName);
        foreach(DrawableShape shape in shapesToRender)
        {
            generateMesh(shape, parent.transform,points, up, spacing, isClosed);
        }
    }

    public void generateMesh(DrawableShape drawShape, Transform parent, Point[] points, Vector3 up, float spacing, bool closed = false)
    {
        GameObject meshObj = new GameObject(drawShape.name);
        meshObj.transform.parent = parent;
        MeshFilter mf = meshObj.AddComponent<MeshFilter>();
        MeshRenderer mr = meshObj.AddComponent<MeshRenderer>();

        if (mf.sharedMesh == null)
            mf.sharedMesh = new Mesh();

        Mesh mesh = mf.sharedMesh;

        Shape shape = enumToShape(drawShape);

        Vector3[] vertices = getMeshVertices(points,shape, drawShape.offset, up, closed);
        Vector3[] normals = pointToMeshNormals(points, shape, up, drawShape.winding,closed);
        Vector2[] UVs = getMeshUV(vertices.Length, shape);
        int[] tris = getMeshTris(vertices.Length, shape,drawShape.winding, closed);

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = UVs;
        mesh.triangles = tris;

        mr.material = drawShape.meshMaterial;
        float tileVal = drawShape.tiling * points.Length * spacing * 0.05f;
        mr.sharedMaterial.mainTextureScale = new Vector2(1, tileVal);


    }

    private int loopIndex(int index, int totalSize)
    {
        return (index + totalSize) % totalSize;
    }

    public static Shape enumToShape(DrawableShape shape)
    {
        switch (shape.shape)
        {

            case ShapeToDraw.WALL:
                return new Wall(shape.size);
            case ShapeToDraw.CUBE:
                return new Cube(shape.size);
            default:
                return new RoadMesh(shape.size);
        }
    }
}

[System.Serializable]
public class DrawableShape
{
    public bool showShape;
    public string name;
    public ShapeToDraw shape;
    public float size;
    public float tiling;
    public Winding winding;
    public Vector3 offset;
    public Material meshMaterial;
}
