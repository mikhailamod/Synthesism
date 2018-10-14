using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tunnel : Shape
{

    private float width;

    public Tunnel(float width)
    {
        this.width = width;
    }

    public override int[] getMeshTris(int vertexCount, Winding winding, Vector2 completion, bool closed = false)
    {
        List<int> tris = new List<int>();
        for (int i = 0; i < vertexCount / 10; i++)
        {
            if(i < vertexCount/10 - 1 || (closed && completion.magnitude == 1.0f))
            {
                //Face 1             
                tris.Add(i*10);
                tris.Add((i*10 + 11) % vertexCount);
                tris.Add((i*10 + 10) % vertexCount);

                //Face 2
                tris.Add(i*10);
                tris.Add((i*10 + 1));
                tris.Add((i*10 + 11) % vertexCount);
                
                //Face 3
                tris.Add((i*10 + 1));
                tris.Add((i*10 + 12) % vertexCount);
                tris.Add((i*10 + 11) % vertexCount);
                
                //Face 4
                tris.Add((i*10 + 1));
                tris.Add((i*10 + 2));
                tris.Add((i*10 + 12) % vertexCount);
                
                //Face 5
                tris.Add((i * 10 + 2));
                tris.Add((i * 10 + 13) % vertexCount);
                tris.Add((i * 10 + 12) % vertexCount);
                
                //Face 6
                tris.Add((i * 10 + 2));
                tris.Add((i * 10 + 3));
                tris.Add((i * 10 + 13) % vertexCount);
                
                //Face 7
                tris.Add((i * 10 + 3));
                tris.Add((i * 10 + 14) % vertexCount);
                tris.Add((i * 10 + 13) % vertexCount);
                
                //Face 8
                tris.Add((i * 10 + 3));
                tris.Add((i * 10 + 4));
                tris.Add((i * 10 + 14) % vertexCount);
                
                //Face 9
                tris.Add((i * 10 + 15) % vertexCount);
                tris.Add((i * 10 + 16) % vertexCount);
                tris.Add(i * 10 + 5);

                //Face 10
                tris.Add((i * 10 + 16) % vertexCount);
                tris.Add((i * 10 + 6));
                tris.Add(i * 10 + 5);

                //Face 11
                tris.Add((i * 10 + 16) % vertexCount);
                tris.Add((i * 10 + 17) % vertexCount);
                tris.Add((i * 10 + 6));

                //Face 12
                tris.Add((i * 10 + 17) % vertexCount);
                tris.Add((i * 10 + 7));
                tris.Add((i * 10 + 6));

                //Face 13
                tris.Add((i * 10 + 17) % vertexCount);
                tris.Add((i * 10 + 18) % vertexCount);
                tris.Add((i * 10 + 7));

                //Face 14
                tris.Add((i * 10 + 18) % vertexCount);
                tris.Add((i * 10 + 8));
                tris.Add((i * 10 + 7));

                //Face 15
                tris.Add((i * 10 + 18) % vertexCount);
                tris.Add((i * 10 + 19) % vertexCount);
                tris.Add((i * 10 + 8));

                //Face 16
                tris.Add((i * 10 + 19) % vertexCount);
                tris.Add((i * 10 + 9));
                tris.Add((i * 10 + 8));

            }

            if (!closed || completion.magnitude != 1.0f)
            {
                //Front Faces
                //Face 1
                tris.Add((0));
                tris.Add(5);
                tris.Add(6);
                //Face 2
                tris.Add((0));
                tris.Add(6);
                tris.Add(1);
                //Face 3
                tris.Add((1));
                tris.Add(6);
                tris.Add(7);
                //Face 4
                tris.Add((1));
                tris.Add(7);
                tris.Add(2);
                //Face 5
                tris.Add((2));
                tris.Add(7);
                tris.Add(8);
                //Face 6
                tris.Add((2));
                tris.Add(8);
                tris.Add(3);
                //Face 7
                tris.Add((3));
                tris.Add(8);
                tris.Add(9);
                //Face 8
                tris.Add((3));
                tris.Add(9);
                tris.Add(4);

                //BackFaces
                int reference = vertexCount - 1;

                //Face 1
                tris.Add((reference));
                tris.Add(reference - 6);
                tris.Add(reference - 5);

                //Face 2
                tris.Add((reference));
                tris.Add(reference - 1);
                tris.Add(reference - 6);

                //Face 3
                tris.Add((reference - 1));
                tris.Add(reference - 7);
                tris.Add(reference - 6);

                //Face 4
                tris.Add((reference - 1));
                tris.Add(reference - 2);
                tris.Add(reference - 7);

                //Face 5
                tris.Add((reference - 2));
                tris.Add(reference - 8);
                tris.Add(reference - 7);

                //Face 6
                tris.Add((reference - 2));
                tris.Add(reference - 3);
                tris.Add(reference - 8);

                //Face 7
                tris.Add((reference - 3));
                tris.Add(reference - 9);
                tris.Add(reference - 8);

                //Face 8
                tris.Add((reference - 3));
                tris.Add(reference - 4);
                tris.Add(reference - 9);


            }
        }
        if (winding == Winding.INVERTED) tris.Reverse();
        return tris.ToArray();
    }

    public override Vector2[] getMeshUV(int vertexCount)
    {
        Vector2[] uvs = new Vector2[vertexCount];

        for (int i = 0; i < vertexCount / 10; i++)
        {
            int index = i * 10;
            float percentageComplete = i / (float)vertexCount * 10;
            float dampingPercent = 1 - Mathf.Abs(2 * percentageComplete - 1);
            uvs[index] = new Vector2(0, dampingPercent);
            uvs[index + 1] = new Vector2(0.5f, dampingPercent);
            uvs[index + 2] = new Vector2(1, dampingPercent);
            uvs[index + 3] = new Vector2(0.5f, dampingPercent);
            uvs[index + 4] = new Vector2(0, dampingPercent);
            uvs[index + 5] = new Vector2(0, dampingPercent);
            uvs[index + 6] = new Vector2(0.5f, dampingPercent);
            uvs[index + 7] = new Vector2(1, dampingPercent);
            uvs[index + 8] = new Vector2(0.5f, dampingPercent);
            uvs[index + 9] = new Vector2(0, dampingPercent);
        }
        return uvs;
    }

    public override Vector3[] getMeshVertices(Point[] points, Vector3[] binormals, Vector3 offset, Vector3 up, Vector2 completion, bool closed = false)
    {
        List<Vector3> vertices = new List<Vector3>();
        for (int i = 0; i < binormals.Length; i++)
        {
            float percentage = i / (float)binormals.Length;
            if (percentage < completion.x || percentage > completion.y) continue;

            Vector3 rightVertex = points[i].Position + binormals[i] * width * 0.5f + binormals[i] * offset.x;
            rightVertex = new Vector3(rightVertex.x, rightVertex.y + offset.y, rightVertex.z);
            Vector3 leftVertex = points[i].Position - binormals[i] * width * 0.5f + binormals[i] * offset.x;
            leftVertex = new Vector3(leftVertex.x, leftVertex.y + offset.y, leftVertex.z);

            Vector3 bottomVertex = points[i].Position;
            bottomVertex = new Vector3(bottomVertex.x, bottomVertex.y + offset.y, bottomVertex.z);
            bottomVertex += binormals[i] * offset.x;
            Vector3 topVertex = bottomVertex + up * width * offset.z;

            Vector3 URC = new Vector3(rightVertex.x, topVertex.y, rightVertex.z);
            Vector3 ULC = new Vector3(leftVertex.x, topVertex.y, leftVertex.z);
            Vector3 URD = URC - bottomVertex;
            Vector3 ULD = ULC - bottomVertex;

            

            Vector3 upperRight = bottomVertex + 0.9f * URD;
            Vector3 upperLeft = bottomVertex + 0.9f * ULD;

            vertices.Add(leftVertex); vertices.Add(upperLeft); vertices.Add(topVertex); vertices.Add(upperRight); vertices.Add(rightVertex);

            Vector3 TTV = topVertex + up * width * offset.z;

            Vector3 FRV = points[i].Position + binormals[i] * width * 0.5f + binormals[i] * offset.x + binormals[i]*Vector3.Distance(topVertex,TTV);
            Vector3 FLV = points[i].Position - binormals[i] * width * 0.5f + binormals[i] * offset.x - binormals[i] * Vector3.Distance(topVertex, TTV);
            URC = new Vector3(FRV.x,TTV.y,FRV.z);
            ULC = new Vector3(FLV.x, TTV.y, FLV.z);

            URD = URC - bottomVertex;
            ULD = ULC - bottomVertex;

            Vector3 FUR = bottomVertex + 0.9f * URD;
            Vector3 FUL = bottomVertex + 0.9f * ULD;

            vertices.Add(FLV); vertices.Add(FUL); vertices.Add(TTV); vertices.Add(FUR); vertices.Add(FRV);

        }
        return vertices.ToArray();
    }

    public override Vector3[] getNormals(Point[] points, Vector3[] normals, Vector3 up, Winding winding, Vector2 completion, bool closed = false)
    {
        Vector3[] meshNormals = new Vector3[normals.Length * 10];
        for (int i = 0; i < meshNormals.Length; i++)
        {
            meshNormals[i] = normals[i / 10];
            if (winding == Winding.INVERTED)
                meshNormals[i] *= -1;
        }

        return meshNormals;
    }
}
