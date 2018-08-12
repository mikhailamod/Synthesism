using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Road))]
public class RoadEditor : Editor
{

    private Road road;


    void OnEnable()
    {




        road = target as Road;

        if (road.roadPath == null)
            road.createPath();
    }

    private void drawScene()
    {

        Handles.color = road.roadSettings.nodeColor;
        for (int i = 0; i < road.roadPath.PointsCount; i++)
        {
            Vector3 updatePos;
            if (i % 3 == 0)
                updatePos = Handles.FreeMoveHandle(road.roadPath[i].Position, Quaternion.identity, road.roadSettings.nodeSize, Vector3.zero, Handles.CylinderHandleCap);
            else
                updatePos = Handles.PositionHandle(road.roadPath[i].Position, Quaternion.identity);

            if (updatePos != road.roadPath[i].Position)
            {
                Undo.RecordObject(road, "Moved point");
                road.roadPath.movePosition(i, updatePos);
            }
        }

        for (int i = 0; i < road.roadPath.SegmentCount; i++)
        {
            Point[] segment = road.roadPath.getSegment(i);
            Handles.color = road.roadSettings.lineColor;
            Handles.DrawLine(segment[0].Position, segment[1].Position);
            Handles.DrawLine(segment[3].Position, segment[2].Position);
            if (road.roadSettings.showCurve)
                Handles.DrawBezier(segment[0].Position, segment[3].Position, segment[1].Position, segment[2].Position, road.roadSettings.curveColor, null, road.roadSettings.curveWidth);

        }


        if(road.roadSettings.showPoints || road.roadSettings.showPointForwards || road.roadSettings.showPointBinormals
            || road.roadSettings.showPointNormals || road.roadSettings.showVertices || road.roadSettings.showTris || road.roadSettings.showMeshNormals || road.roadSettings.showUVs)
        {
            Point[] roadPoints = road.roadPath.getRoadPathPoints(road.roadSettings.spacing);
            Vector3[] forwardDir = road.meshCreator.getPointsForward(roadPoints, road.roadPath.Closed);
            Vector3[] binormals = road.meshCreator.getPointsBinormal(forwardDir, Vector3.up);
            Vector3[] normals = road.meshCreator.getPointsNormal(forwardDir, binormals);

            if (road.roadSettings.showPoints)
            {
                Handles.color = road.roadSettings.pathColor;
                foreach (Point node in roadPoints)
                {
                    Handles.SphereHandleCap(1, node.Position, Quaternion.identity, road.roadSettings.size, EventType.Repaint);
                }
            }
            if (road.roadSettings.showPointForwards)
            {
                Handles.color = road.roadSettings.nodeForwardColor;
                for (int i = 0; i < roadPoints.Length; i++)
                {
                    if (!road.roadPath.Closed && i == roadPoints.Length - 1)
                        break;

                    Handles.ArrowHandleCap(1, roadPoints[i].Position, Quaternion.LookRotation(forwardDir[i]), road.roadSettings.nodeArrowSize, EventType.Repaint);
                }
            }
            if (road.roadSettings.showPointBinormals)
            {
                Handles.color = road.roadSettings.nodeBinormalColor;
                for (int i = 0; i < roadPoints.Length; i++)
                {
                    if (!road.roadPath.Closed && i == roadPoints.Length - 1)
                        break;
                    Handles.ArrowHandleCap(1, roadPoints[i].Position, Quaternion.LookRotation(binormals[i]), road.roadSettings.nodeArrowSize, EventType.Repaint);
                }
            }
            if (road.roadSettings.showPointNormals)
            {
                Handles.color = road.roadSettings.nodeNormalColor;
                for (int i = 0; i < roadPoints.Length; i++)
                {
                    if (!road.roadPath.Closed && i == roadPoints.Length - 1)
                        break;
                    Handles.ArrowHandleCap(1, roadPoints[i].Position, Quaternion.LookRotation(normals[i]), road.roadSettings.nodeArrowSize, EventType.Repaint);
                }
            }

            foreach (DrawableShape shape in road.meshCreator.shapesToRender)
            {

                Shape renderShape = enumToShape(shape);
                if (road.roadSettings.showVertices || road.roadSettings.showTris || road.roadSettings.showPointNormals || road.roadSettings.showUVs)
                {
                    Vector3[] vertices = road.meshCreator.getMeshVertices(roadPoints, renderShape, shape.offset,Vector3.up, road.roadPath.Closed);
                    int[] tris = road.meshCreator.getMeshTris(vertices.Length, renderShape, road.roadPath.Closed);

                    if (road.roadSettings.showVertices)
                    {
                        Handles.color = road.roadSettings.vertexColor;
                        foreach (Vector3 vertexPos in vertices)
                        {
                            Handles.SphereHandleCap(1, vertexPos, Quaternion.identity, road.roadSettings.size, EventType.Repaint);
                        }
                    }

                    if (road.roadSettings.showTris)
                    {
                        Handles.color = Color.green;
                        for (int i = 0; i < tris.Length / 3; i++)
                        {
                            int index = 3 * i;
                            Handles.DrawLine(vertices[tris[index]], vertices[tris[index + 1]]);
                            Handles.DrawLine(vertices[tris[index + 1]], vertices[tris[index + 2]]);
                            Handles.DrawLine(vertices[tris[index + 2]], vertices[tris[index]]);
                        }
                    }

                    if (road.roadSettings.showMeshNormals)
                    {
                        Handles.color = Color.white;
                        Vector3[] vertexNormals = road.meshCreator.pointToMeshNormals(roadPoints, renderShape, Vector3.up, shape.invertNormals, road.roadPath.Closed);

                        for (int i = 0; i < vertexNormals.Length; i++)
                        {
                            Handles.ArrowHandleCap(1, vertices[i], Quaternion.LookRotation(vertexNormals[i]), road.roadSettings.nodeArrowSize, EventType.Repaint);
                        }
                    }

                    if (road.roadSettings.showUVs)
                    {
                        Vector2[] UVs = road.meshCreator.getMeshUV(vertices.Length, renderShape);
                        for (int i = 0; i < UVs.Length; i++)
                        {
                            Handles.Label(vertices[i], UVs[i].ToString());
                        }
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
        road.roadSettings.showNodeProperties = EditorGUILayout.Foldout(road.roadSettings.showNodeProperties,"Node Properties", EditorStyles.foldout);
        if(road.roadSettings.showNodeProperties)
        {
            road.roadSettings.nodeColor = EditorGUILayout.ColorField("Node Color: ", road.roadSettings.nodeColor);
            road.roadSettings.nodeSize = EditorGUILayout.Slider("Node Size", road.roadSettings.nodeSize, 1, 10);
        }

        road.roadSettings.showCurveProperties = EditorGUILayout.Foldout(road.roadSettings.showCurveProperties, "Curve Properties", EditorStyles.foldout);
        if(road.roadSettings.showCurveProperties)
        {
            bool tempShowCurve = EditorGUILayout.Toggle("Show Curve", road.roadSettings.showCurve);
            if (tempShowCurve != road.roadSettings.showCurve)
            {
                road.roadSettings.showCurve = tempShowCurve;
                SceneView.RepaintAll();
            }
            road.roadSettings.curveWidth = EditorGUILayout.Slider("Curve Width", road.roadSettings.curveWidth, 4, 10);
            road.roadSettings.lineColor = EditorGUILayout.ColorField("Line Color: ", road.roadSettings.lineColor);
            road.roadSettings.curveColor = EditorGUILayout.ColorField("Curve Color: ", road.roadSettings.curveColor); 
        }
        

        road.roadSettings.showPathProperties = EditorGUILayout.Foldout(road.roadSettings.showPathProperties,"Path Properties", EditorStyles.foldout);

        if(road.roadSettings.showPathProperties)
        {
            bool isClosed = EditorGUILayout.Toggle("Close road", road.roadPath.Closed);
            if (isClosed != road.roadPath.Closed)
            {
                road.roadPath.Closed = isClosed;
                SceneView.RepaintAll();
            }

            bool showPath = EditorGUILayout.Toggle("Show Node Path", road.roadSettings.showPoints);
            if (road.roadSettings.showPoints != showPath)
            {
                road.roadSettings.showPoints = showPath;
                SceneView.RepaintAll();
            }

            road.roadSettings.showPathArrowProperties = EditorGUILayout.Foldout(road.roadSettings.showPathArrowProperties, "Arrows", EditorStyles.foldout);
            if(road.roadSettings.showPathArrowProperties)
            {
                bool showForwardDir = EditorGUILayout.Toggle("Show Node Forward", road.roadSettings.showPointForwards);
                if (road.roadSettings.showPointForwards != showForwardDir)
                {
                    road.roadSettings.showPointForwards = showForwardDir;
                    SceneView.RepaintAll();
                }

                bool tempShowBi = EditorGUILayout.Toggle("Show Node Binormals", road.roadSettings.showPointBinormals);
                if (road.roadSettings.showPointBinormals != tempShowBi)
                {
                    road.roadSettings.showPointBinormals = tempShowBi;
                    SceneView.RepaintAll();
                }

                bool tempShowNormals = EditorGUILayout.Toggle("Show Node Normals", road.roadSettings.showPointNormals);
                if (road.roadSettings.showPointNormals != tempShowNormals)
                {
                    road.roadSettings.showPointNormals = tempShowNormals;
                    SceneView.RepaintAll();
                }

                float tempArrowSize = EditorGUILayout.Slider("Node Arrow Size", road.roadSettings.nodeArrowSize, 0.1f, 5);
                if (tempArrowSize != road.roadSettings.nodeArrowSize)
                {
                    road.roadSettings.nodeArrowSize = tempArrowSize;
                    SceneView.RepaintAll();
                }
            }

            float tempSpacing = EditorGUILayout.Slider("Spacing", road.roadSettings.spacing, 0.01f, 1);
            if (road.roadSettings.spacing != tempSpacing)
            {
                road.roadSettings.spacing = tempSpacing;
                SceneView.RepaintAll();
            }
            float tempOffset = EditorGUILayout.Slider("Delta offset", road.roadPath.deltaOffset, 0.01f, 1f);
            if (road.roadPath.deltaOffset != tempOffset)
            {
                road.roadPath.deltaOffset = tempOffset;
                SceneView.RepaintAll();
            }

            float tempSize = EditorGUILayout.Slider("Size", road.roadSettings.size, 0.1f, 5);
            if (tempSize != road.roadSettings.size)
            {
                road.roadSettings.size = tempSize;
                SceneView.RepaintAll();
            }

            road.roadSettings.showPathColorProperties = EditorGUILayout.Foldout(road.roadSettings.showPathColorProperties, "Colors", EditorStyles.foldout);
            if(road.roadSettings.showPathColorProperties)
            {
                road.roadSettings.pathColor = EditorGUILayout.ColorField("Node Path Color: ", road.roadSettings.pathColor);
                road.roadSettings.nodeForwardColor = EditorGUILayout.ColorField("Node Forward Color: ", road.roadSettings.nodeForwardColor);
                road.roadSettings.nodeBinormalColor = EditorGUILayout.ColorField("Node Binormal Color: ", road.roadSettings.nodeBinormalColor);
                road.roadSettings.nodeNormalColor = EditorGUILayout.ColorField("Node Normal Color: ", road.roadSettings.nodeNormalColor);
            }
        }

        road.roadSettings.showMeshProperties = EditorGUILayout.Foldout(road.roadSettings.showMeshProperties, "Mesh Properties", EditorStyles.foldout);
        if(road.roadSettings.showMeshProperties)
        {
            bool tempShowVertices = EditorGUILayout.Toggle("Show Vertices", road.roadSettings.showVertices);
            if (road.roadSettings.showVertices != tempShowVertices)
            {
                road.roadSettings.showVertices = tempShowVertices;
                SceneView.RepaintAll();
            }

            bool tempShowTris = EditorGUILayout.Toggle("Show Triangles", road.roadSettings.showTris);
            if (road.roadSettings.showTris != tempShowTris)
            {
                road.roadSettings.showTris = tempShowTris;
                SceneView.RepaintAll();
            }

            bool tempShowMeshNormals = EditorGUILayout.Toggle("Show Mesh Normals", road.roadSettings.showMeshNormals);
            if (road.roadSettings.showMeshNormals != tempShowMeshNormals)
            {
                road.roadSettings.showMeshNormals = tempShowMeshNormals;
                SceneView.RepaintAll();
            }

            bool tempShowUVs = EditorGUILayout.Toggle("Show UVs", road.roadSettings.showUVs);
            if (road.roadSettings.showUVs != tempShowUVs)
            {
                road.roadSettings.showUVs = tempShowUVs;
                SceneView.RepaintAll();
            }
            if(road.meshCreator.shapesToRender.Count != 0)
            {
                try
                {
                    EditorGUILayout.BeginVertical("Box");
                    foreach (DrawableShape shape in road.meshCreator.shapesToRender)
                    {
                        shape.showShape = EditorGUILayout.Foldout(shape.showShape, shape.name, EditorStyles.foldout);
                        if (shape.showShape)
                        {
                            shape.shape = (ShapeToDraw)EditorGUILayout.EnumPopup("Shape Type:", shape.shape);
                            string tempName = EditorGUILayout.TextField("Name:", shape.name);
                            if(tempName != shape.name)
                            {
                                shape.name = tempName;
                            }
                            float tempSize = EditorGUILayout.FloatField("Size:", shape.size);
                            if(tempSize != shape.size)
                            {
                                shape.size = tempSize;
                                SceneView.RepaintAll();
                            }

                            bool tempInvertNormals = EditorGUILayout.Toggle("Invert Normals: ", shape.invertNormals);
                            if(tempInvertNormals != shape.invertNormals)
                            {
                                shape.invertNormals = tempInvertNormals;
                                SceneView.RepaintAll();
                            }

                            shape.meshMaterial = EditorGUILayout.ObjectField("Road Material", shape.meshMaterial, typeof(Material), false) as Material;

                            Vector3 tempOffset = EditorGUILayout.Vector3Field("Offset:", shape.offset);
                            if(!Vector3.Equals(tempOffset,shape.offset))
                            {
                                shape.offset = tempOffset;
                                SceneView.RepaintAll();
                            }

                            if (GUILayout.Button("Remove"))
                            {
                                road.meshCreator.shapesToRender.Remove(shape);
                                SceneView.RepaintAll();
                            }
                        }
                    }
                }
                catch (System.InvalidOperationException) { }
                EditorGUILayout.EndVertical();
            }

            if (GUILayout.Button("Add Mesh"))
            {
                road.meshCreator.shapesToRender.Add(new DrawableShape());
            }
            
            
        }

        if (GUILayout.Button("Generate Mesh"))
        {
            foreach (DrawableShape shape in road.meshCreator.shapesToRender)
            {
                generateMesh(shape);
            }
        }
    }

    void OnSceneGUI()
    {
        manageInput();
        drawScene();
    }

    void generateMesh(DrawableShape shapeToDraw)
    {
        
        road.meshCreator.generateRoadMesh(road.roadPath.getRoadPathPoints(road.roadSettings.spacing), enumToShape(shapeToDraw), shapeToDraw.offset, Vector3.up, shapeToDraw.meshMaterial, shapeToDraw.invertNormals, road.roadPath.Closed);  
    }

    Shape enumToShape(DrawableShape shape)
    {
        switch(shape.shape)
        {

            case ShapeToDraw.WALL:
                return new Wall(shape.size);
            default:
                return new RoadMesh(shape.size);
        }
    }

}
