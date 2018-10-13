using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : Shape
{

    public float size;

    Wall leftWall;
    RoadMesh roof;
    Wall rightWall;
    RoadMesh floor;

    public Cube(float size)
    {
        this.size = size;
        leftWall = new Wall(size);
        roof = new RoadMesh(size);
        rightWall = new Wall(size);
        floor = new RoadMesh(size);
    }

    public override int[] getMeshTris(int vertexCount, Winding winding, Vector2 completion, bool closed = false)
    {
        List<int> tris = new List<int>();

        int[] leftWallTris = leftWall.getMeshTris(vertexCount/4, winding, completion, closed);
        tris.AddRange(leftWallTris);
        int[] roofTris = roof.getMeshTris(vertexCount/4, winding, completion, closed); 
        for (int i = 0; i < roofTris.Length; i++)
        {
            roofTris[i] += vertexCount / 4;
        }
        tris.AddRange(roofTris);
        int[] rightWallTris = rightWall.getMeshTris(vertexCount/4, winding == Winding.FORWARD ? Winding.INVERTED : winding, completion, closed);
        for(int i = 0; i < rightWallTris.Length; i++)
        {
            rightWallTris[i] += vertexCount/4 * 2;
        }
        tris.AddRange(rightWallTris);
        int[] floorTris = floor.getMeshTris(vertexCount/4, winding == Winding.FORWARD ? Winding.INVERTED : winding, completion, closed);
        for (int i = 0; i < floorTris.Length; i++)
        {
            floorTris[i] += vertexCount / 4 * 3;
        }
        tris.AddRange(floorTris);

        if(!closed || completion.magnitude != 1.0f)
        {
            tris.AddRange(new int[] { rightWallTris[rightWallTris.Length - 1], leftWallTris[2], rightWallTris[rightWallTris.Length - 3] });
            tris.AddRange(new int[] { leftWallTris[0], leftWallTris[2], rightWallTris[rightWallTris.Length - 1] });

            tris.AddRange(new int[] { rightWallTris[1], rightWallTris[0], leftWallTris[leftWallTris.Length - 1] });
            tris.AddRange(new int[] { leftWallTris[leftWallTris.Length - 1], leftWallTris[leftWallTris.Length - 2], rightWallTris[1] });

        }

        return tris.ToArray();
    }

    public override Vector2[] getMeshUV(int vertexCount)
    {

        List<Vector2> uvs = new List<Vector2>();

        uvs.AddRange(leftWall.getMeshUV(vertexCount));

        return uvs.ToArray();
    }

    public override Vector3[] getMeshVertices(Point[] points, Vector3[] binormals, Vector3 offset, Vector3 up, Vector2 completion, bool closed = false)
    {

        List<Vector3> vertices = new List<Vector3>();

        vertices.AddRange(leftWall.getMeshVertices(points, binormals, (offset + new Vector3(-size/2f, 0, 0)), up, completion, closed));
        vertices.AddRange(roof.getMeshVertices(points, binormals, offset + new Vector3(0, size, 0), up,completion, closed));
        vertices.AddRange(rightWall.getMeshVertices(points, binormals, (offset + new Vector3(size/2f, 0, 0)) , up, completion,closed));
        vertices.AddRange(floor.getMeshVertices(points, binormals,offset, up, completion, closed));

        return vertices.ToArray();

    }

    public override Vector3[] getNormals(Point[] points, Vector3[] normals, Vector3 up, Winding winding, Vector2 completion, bool closed = false)
    {
        List<Vector3> meshnormals = new List<Vector3>();


        meshnormals.AddRange(leftWall.getNormals(points, normals, up, winding, completion, closed));
        meshnormals.AddRange(roof.getNormals(points, normals, up, winding, completion, closed));
        meshnormals.AddRange(rightWall.getNormals(points, normals, up, winding, completion, closed));
        meshnormals.AddRange(floor.getNormals(points, normals, up, winding, completion, closed));

        return meshnormals.ToArray();

    }
}
