using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoadPath
{
    [SerializeField, HideInInspector]
    List<Point> points;
    [SerializeField, HideInInspector]
    bool closed;
    [SerializeField, HideInInspector]
    public float deltaOffset;

    public RoadPath(Vector3 centre)
    {
        points = new List<Point>
        {
            new Point(centre + Vector3.left,Quaternion.identity),
            new Point(centre + Vector3.left + Vector3.forward,Quaternion.identity),
            new Point(centre + Vector3.right, Quaternion.identity),
            new Point(centre + Vector3.right + Vector3.forward,Quaternion.identity)
        };
    }

    /// <summary>
    /// Returns the number of points in the road path
    /// </summary>
    public int PointsCount
    {
        get
        {
            if (points == null) return 0;

            return points.Count;
        }
        private set { }
    }

    /// <summary>
    /// Returns the number of segments in the road path.
    /// </summary>
    public int SegmentCount
    {
        get
        {
            if (points == null) return 0;

            return points.Count / 3;
        }
        private set { }
    }

    /// <summary>
    /// Whether the raod is closed or not
    /// </summary>
    public bool Closed
    {
        get
        {
            return closed;
        }
        set
        {
            closed = value;
            if(closed)
            {
                points.Add(new Point(points[points.Count - 1].Position * 2 - points[points.Count - 2].Position,Quaternion.identity));
                points.Add(new Point(points[0].Position * 2 - points[1].Position,Quaternion.identity));
            }
            else
            {
                points.RemoveRange(points.Count - 2, 2);
            }
        }
    }

    /// <summary>
    /// Gets the point at index specified
    /// </summary>
    /// <param name="index">The index of the point that will be returned</param>
    /// <returns></returns>
    public Point this[int index]
    {
        get
        {
            return points[index];
        }
        private set { } 
    }


    public void movePosition(int index, Vector3 newPos)
    {
        Vector3 deltaMove = newPos - points[index].Position;
        points[index].Position = newPos;

        //Is point an anchor
        if (index % 3 == 0)
        {
            if (index + 1 < points.Count || closed)
            {
                points[wrapIndex(index + 1)].Position += deltaMove;
            }

            if (index - 1 >= 0 || closed)
            {
                points[wrapIndex(index - 1)].Position += deltaMove;
            }
        }
        else //Handle Point
        {
            bool nextPointIsAnchor = (index + 1) % 3 == 0;

            int coupleControlPoint = (nextPointIsAnchor) ? index + 2 : index - 2;
            int coupleAnchorPoint = (nextPointIsAnchor) ? index + 1 : index - 1;

            if (coupleControlPoint >= 0 && coupleControlPoint < points.Count || closed)
            {
                float distance = (points[wrapIndex(coupleAnchorPoint)].Position - points[wrapIndex(coupleControlPoint)].Position).magnitude;
                Vector3 direction = (points[wrapIndex(coupleAnchorPoint)].Position - points[index].Position).normalized;
                points[wrapIndex(coupleControlPoint)].Position = points[wrapIndex(coupleAnchorPoint)].Position + direction * distance;
            }

        }
    }

    /// <summary>
    /// Adds a road segment to the existing road path
    /// </summary>
    /// <param name="targetPos">The position at which a user would like there new road segment to end</param>
    public void addSegment(Vector3 targetPos)
    {
        points.Add(new Point(points[points.Count - 1].Position * 2 - points[points.Count - 2].Position,Quaternion.identity));
        points.Add(new Point(targetPos + Vector3.up,Quaternion.identity));
        points.Add(new Point(targetPos,Quaternion.identity));
    }

    /// <summary>
    /// Split the selected segment in two
    /// </summary>
    /// <param name="index">Index of the segment that will be split</param>
    public void divideSegment(int index)
    {
        Point[] selectedSegment = getSegment(index);
        Vector3 newAnchorPos = (selectedSegment[0].Position + selectedSegment[3].Position) * 0.5f;

        points.InsertRange(index * 3 + 2, new Point[] { new Point(newAnchorPos + Vector3.forward,Quaternion.identity), new Point(newAnchorPos,Quaternion.identity),new Point(newAnchorPos,Quaternion.identity), new Point(newAnchorPos + Vector3.back,Quaternion.identity) });

    }

    /// <summary>
    /// Removes a segment from the road
    /// </summary>
    /// <param name="index">The index of the segment that will be removed</param>
    public void removeSegment(int index)
    {
        if(SegmentCount > 2 || !closed && SegmentCount > 1)
        {
            if(index == 0)
            {
                if(closed)
                {
                    points[points.Count-1] = points[2];
                }
                points.RemoveRange(0, 3);
            }
            else if( index == points.Count - 1 && !closed)
            {
                points.RemoveRange(index - 2, 3);
            }
            else
            {
                points.RemoveRange(index - 1, 3);
            }
        }
    }

    /// <summary>
    /// Returns a segment (4 Vector3s) from the road path.
    /// </summary>
    /// <param name="i">The segment returned.</param>
    /// <returns></returns>
    public Point[] getSegment(int i)
    {
        return new Point[] { points[i * 3], points[i * 3 + 1], points[i * 3 + 2], points[wrapIndex(i * 3 + 3)] };
    }

    /// <summary>
    /// Returns a Vector3 that corresponds to a position along the curve
    /// </summary>
    /// <param name="segmentIndex">Segment index.</param>
    /// <param name="t">value from [0,1] indicating the distance along the curve</param>
    /// <returns></returns>
    public Vector3 getPoint(int segmentIndex, float t)
    {
        Point[] pts = getSegment(segmentIndex);

        float omt = 1f - t;
        float omt2 = omt * omt;
        float t2 = t * t;
        return pts[0].Position * (omt2 * omt) +
                pts[1].Position * (3f * omt2 * t) +
                pts[2].Position * (3f * omt * t2) +
                pts[3].Position * (t2 * t);
    }

    public Point[] getRoadPathPoints(float spacing)
    {
        List<Point> pointsToReturn = new List<Point>();
        pointsToReturn.Add(points[0]);
        Point previousPoint = points[0];

        float dstFromLastPoint = 0;

        for(int i =0; i < SegmentCount; i++)
        {
            float t = 0;
            
            while (t <= 1)
            {
                t += deltaOffset;
                Vector3 calcPoint = getPoint(i, t);
                dstFromLastPoint += Vector3.Distance(previousPoint.Position, calcPoint);

                while(dstFromLastPoint > spacing)
                {
                    float fixDistance = dstFromLastPoint - spacing;
                    Vector3 fixPos = calcPoint + (previousPoint.Position - calcPoint).normalized * fixDistance;
                    Point fixPoint = new Point(fixPos, Quaternion.identity); //Fix Here
                    pointsToReturn.Add(fixPoint); 
                    dstFromLastPoint = fixDistance;
                    previousPoint = fixPoint;
                }

                previousPoint = new Point(calcPoint,Quaternion.identity); //Fix Here
            }
             
        }

        return pointsToReturn.ToArray();
    }

    private int wrapIndex(int index)
    {
        return (index + points.Count) % points.Count;
    }

}
[System.Serializable]
public class Point
{
    private Vector3 position;
    private Quaternion rotation;

    public Point(Vector3 pos, Quaternion rot)
    {
        position = pos;
        rotation = rot;
    }

    public Vector3 Position
    {
        get
        {
            return position;
        }
        set
        {
            position = value;
        }
    }

    public Quaternion Rotation
    {
        get
        {
            return rotation;
        }
        set
        {
            rotation = value;
        }
    }

}

