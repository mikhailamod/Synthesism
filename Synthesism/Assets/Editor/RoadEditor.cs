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
    public float curveWidth = 4;
    public float nodeSize = 1;

    void OnEnable()
    {
        road = target as Road;

        if (road.roadPath == null)
            road.createPath();

    }

    private void drawScene()
    {


        Handles.color = nodeColor;
        for (int i = 0; i < road.roadPath.PointsCount; i++)
        {
            Vector3 updatePos;
            if (i % 3 == 0)
                updatePos = Handles.FreeMoveHandle(road.roadPath[i], Quaternion.identity, nodeSize, Vector3.zero, Handles.CylinderHandleCap);
            else
                updatePos = Handles.PositionHandle(road.roadPath[i], Quaternion.identity);
            
            if(updatePos != road.roadPath[i])
            {
                Undo.RecordObject(road, "Moved point");
                road.roadPath[i] = updatePos;
            }
        }

        for(int i = 0; i < road.roadPath.SegmentCount; i++)
        {
            Vector3[] segment = road.roadPath.getSegment(i);
            Handles.color = lineColor;
            Handles.DrawLine(segment[0], segment[1]);
            Handles.DrawLine(segment[3], segment[2]);

            Handles.DrawBezier(segment[0], segment[3], segment[1], segment[2], curveColor, null, curveWidth);

        }

    }

    private void manageInput()
    {
        Event currentEvent = Event.current;

        if(currentEvent.type == EventType.MouseDown && currentEvent.button == 0 && currentEvent.shift)
        {
            Vector3 mouseToWorldSpace = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition).origin;
            Undo.RecordObject(road, "Road Segment Creation");
            road.roadPath.addSegment(mouseToWorldSpace);
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
        base.DrawDefaultInspector();
    }

    void OnSceneGUI()
    {
        manageInput();
        drawScene();
    }

}
