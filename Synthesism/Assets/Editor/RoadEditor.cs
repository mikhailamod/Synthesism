using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Road))]
public class RoadEditor : Editor
{

    private Road road;

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
    public bool showCurve = true;
    public bool showPoints = false;
    public bool showPointForwards = false;
    public bool showPointBinormals = false;
    public bool showPointNormals = false;

    public bool showVertices = false;
    public bool showTris = false;


    void OnEnable()
    {
        road = target as Road;

        if (road.roadPath == null)
            road.createPath();

        if (road.meshCreator == null)
            road.meshCreator = new RoadMeshCreator();

    }

    private void drawScene()
    {
        Handles.color = nodeColor;
        for (int i = 0; i < road.roadPath.PointsCount; i++)
        {
            Vector3 updatePos;
            if (i % 3 == 0)
                updatePos = Handles.FreeMoveHandle(road.roadPath[i].Position, Quaternion.identity, nodeSize, Vector3.zero, Handles.CylinderHandleCap);
            else
                updatePos = Handles.PositionHandle(road.roadPath[i].Position, Quaternion.identity);
            
            if(updatePos != road.roadPath[i].Position)
            {
                Undo.RecordObject(road, "Moved point");
                road.roadPath.movePosition(i,updatePos);
            }
        }

        for(int i = 0; i < road.roadPath.SegmentCount; i++)
        {
            Point[] segment = road.roadPath.getSegment(i);
            Handles.color = lineColor;
            Handles.DrawLine(segment[0].Position, segment[1].Position);
            Handles.DrawLine(segment[3].Position, segment[2].Position);
            if(showCurve)
                Handles.DrawBezier(segment[0].Position, segment[3].Position, segment[1].Position, segment[2].Position, curveColor, null, curveWidth);

        }

        if(showPoints || showPointForwards || showPointBinormals || showPointNormals || showVertices || showTris)
        {
            Point[] roadPoints = road.roadPath.getRoadPathPoints(spacing);
            Vector3[] forwardDir = road.meshCreator.getPointsForward(roadPoints, road.roadPath.Closed);
            Vector3[] binormals = road.meshCreator.getPointsBinormal(forwardDir, Vector3.up);
            Vector3[] normals = road.meshCreator.getPointsNormal(forwardDir, binormals);

            if (showPoints)
            {
                Handles.color = pathColor;
                foreach (Point node in roadPoints)
                {
                    Handles.SphereHandleCap(1, node.Position, Quaternion.identity, size, EventType.Repaint);
                }
            }
            if(showPointForwards)
            {  
                Handles.color = nodeForwardColor;
                for(int i = 0; i < roadPoints.Length; i++)
                {
                    if (!road.roadPath.Closed && i == roadPoints.Length - 1)
                        break;

                    Handles.ArrowHandleCap(1, roadPoints[i].Position, Quaternion.LookRotation(forwardDir[i]), nodeArrowSize, EventType.Repaint);
                }
            }
            if(showPointBinormals)
            {
                Handles.color = nodeBinormalColor;
                for (int i = 0; i < roadPoints.Length; i++)
                {
                    if (!road.roadPath.Closed && i == roadPoints.Length - 1)
                        break;
                    Handles.ArrowHandleCap(1, roadPoints[i].Position, Quaternion.LookRotation(binormals[i]), nodeArrowSize, EventType.Repaint);
                }
            }
            if(showPointNormals)
            {
                Handles.color = nodeNormalColor;
                for (int i = 0; i < roadPoints.Length; i++)
                {
                    if (!road.roadPath.Closed && i == roadPoints.Length - 1)
                        break;
                    Handles.ArrowHandleCap(1, roadPoints[i].Position, Quaternion.LookRotation(normals[i]), nodeArrowSize, EventType.Repaint);
                }
            }

            if (showVertices || showTris)
            {
                Vector3[] vertices = road.meshCreator.getMeshVertices(roadPoints, Vector3.up, road.roadPath.Closed);
                int[] tris = road.meshCreator.getMeshTris(vertices.Length, road.roadPath.Closed);
                if (showVertices)
                {
                    Handles.color = vertexColor;
                    foreach (Vector3 vertexPos in vertices)
                    {
                        Handles.SphereHandleCap(1, vertexPos, Quaternion.identity, size, EventType.Repaint);
                    }
                }
                    
                if (showTris)
                {
                    Handles.color = Color.green;
                    for(int i = 0; i < tris.Length/3; i++)
                    {
                        int index = 3 * i;
                        Handles.DrawLine(vertices[tris[index]], vertices[tris[index + 1]]);
                        Handles.DrawLine(vertices[tris[index + 1]], vertices[tris[index + 2]]);
                        Handles.DrawLine(vertices[tris[index + 2]], vertices[tris[index]]);
                    }
                }
            }
        }   
    }

    private void manageInput()
    {
        Event currentEvent = Event.current;
        
        if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0 && currentEvent.shift)
        {
            Vector3 mouseToWorldSpace = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition).origin;
            Undo.RecordObject(road, "Road Segment Creation");
            road.roadPath.addSegment(mouseToWorldSpace);
        }

        if(currentEvent.type == EventType.MouseDown && currentEvent.button == 1 && currentEvent.shift)
        {
            //Delete
            float minDistance = 40;
            int closestAnchor = -1;

            for(int i = 0; i < road.roadPath.PointsCount; i ++)
            {
                float distance = Vector2.Distance(currentEvent.mousePosition, HandleUtility.WorldToGUIPoint(road.roadPath[i].Position));
                if (distance <= minDistance)
                {
                    minDistance = distance;
                    closestAnchor = i;
                }
            }

            if(closestAnchor != -1)
            {
                Undo.RecordObject(road, "Removed Segment");
                road.roadPath.removeSegment(closestAnchor);
            }

        }

        


    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Node Properties");
        nodeColor = EditorGUILayout.ColorField("Node Color: ", nodeColor);
        nodeSize = EditorGUILayout.Slider("Node Size", nodeSize, 1, 10);
        EditorGUILayout.LabelField("Curve Properties");
        lineColor = EditorGUILayout.ColorField("Line Color: ", lineColor);
        curveColor = EditorGUILayout.ColorField("Curve Color: ", curveColor);
        curveWidth = EditorGUILayout.Slider("Curve Width",curveWidth, 4, 10);
        bool tempShowCurve =  EditorGUILayout.Toggle("Show Curve", showCurve);
        if (tempShowCurve != showCurve)
        {
            showCurve = tempShowCurve;
            SceneView.RepaintAll();
        }

        EditorGUILayout.LabelField("Path Properties");
        bool isClosed = EditorGUILayout.Toggle("Close road", road.roadPath.Closed);
        if(isClosed != road.roadPath.Closed)
        {
            road.roadPath.Closed = isClosed;
            SceneView.RepaintAll();
        }
        pathColor = EditorGUILayout.ColorField("Node Path Color: ", pathColor);
        nodeForwardColor = EditorGUILayout.ColorField("Node Forward Color: ", nodeForwardColor);
        nodeBinormalColor = EditorGUILayout.ColorField("Node Binormal Color: ", nodeBinormalColor);
        nodeNormalColor = EditorGUILayout.ColorField("Node Normal Color: ", nodeNormalColor);

        float tempSpacing = EditorGUILayout.Slider("Spacing", spacing, 0.01f, 1);
        if(spacing != tempSpacing)
        {
            spacing = tempSpacing;
            SceneView.RepaintAll();
        }
        float tempSize = EditorGUILayout.Slider("Size", size, 0.1f,5);
        if(tempSize != size)
        {
            size = tempSize;
            SceneView.RepaintAll();
        }
        bool showPath = EditorGUILayout.Toggle("Show Node Path", showPoints);
        if (showPoints != showPath)
        {
            showPoints = showPath;
            SceneView.RepaintAll();
        }

        bool showForwardDir = EditorGUILayout.Toggle("Show Node Forward", showPointForwards);
        if(showPointForwards != showForwardDir)
        {
            showPointForwards = showForwardDir;
            SceneView.RepaintAll();
        }

        bool tempShowBi = EditorGUILayout.Toggle("Show Node Binormals", showPointBinormals);
        if (showPointBinormals != tempShowBi)
        {
            showPointBinormals = tempShowBi;
            SceneView.RepaintAll();
        }

        bool tempShowNormals = EditorGUILayout.Toggle("Show Node Normals", showPointNormals);
        if (showPointNormals != tempShowNormals)
        {
            showPointNormals = tempShowNormals;
            SceneView.RepaintAll();
        }

        float tempArrowSize = EditorGUILayout.Slider("Node Arrow Size", nodeArrowSize, 0.1f, 5);
        if (tempArrowSize != nodeArrowSize)
        {
            nodeArrowSize = tempArrowSize;
            SceneView.RepaintAll();
        }

        EditorGUILayout.LabelField("Mesh Properties");

        float tempRoadWidth =  EditorGUILayout.Slider("Road Width", road.meshCreator.roadWidth, 0.1f, 10.0f);
        if(tempRoadWidth != road.meshCreator.roadWidth)
        {
            road.meshCreator.roadWidth = tempRoadWidth;
            SceneView.RepaintAll();
        }

        bool tempShowVertices = EditorGUILayout.Toggle("Show Vertices", showVertices);
        if (showVertices != tempShowVertices)
        {
            showVertices = tempShowVertices;
            SceneView.RepaintAll();
        }

        bool tempShowTris = EditorGUILayout.Toggle("Show Triangles", showTris);
        if (showTris != tempShowTris)
        {
            showTris = tempShowTris;
            SceneView.RepaintAll();
        }

        if(GUILayout.Button("Generate Mesh"))
        {
            Debug.Log("Generating Mesh");
        }
    }

    void OnSceneGUI()
    {
        manageInput();
        drawScene();
    }

}
