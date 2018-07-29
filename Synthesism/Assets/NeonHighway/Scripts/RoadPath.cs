using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoadPath
{
    [SerializeField, HideInInspector]
    List<Vector3> points;
    [SerializeField, HideInInspector]
    bool closed;

    public RoadPath(Vector3 centre)
    {
        points = new List<Vector3>
        {
            centre + Vector3.left,
            centre + Vector3.left + Vector3.forward,
            centre + Vector3.right,
            centre + Vector3.right + Vector3.forward
        };
    }

    /// <summary>
    /// Returns the number of points in the road path
    /// </summary>
    public int PointsCount
    {
        get
        {
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
                points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
                points.Add(points[0] * 2 - points[1]);
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
    public Vector3 this[int index]
    {
        get
        {
            return points[index];
        }
        set
        {

            Vector3 deltaMove = value - points[index];
            points[index] = value;

            //Is point an anchor
            if (index % 3 == 0)
            {
                if(index + 1 < points.Count || closed)
                {
                    points[wrapIndex(index + 1)] += deltaMove;
                }

                if(index - 1 >= 0 || closed)
                {
                    points[wrapIndex(index - 1)] += deltaMove;
                }
            }
            else //Handle Point
            {
                bool nextPointIsAnchor = (index + 1) % 3 == 0;

                int coupleControlPoint = (nextPointIsAnchor)? index + 2 : index - 2;
                int coupleAnchorPoint = (nextPointIsAnchor) ? index + 1 : index - 1;

                if(coupleControlPoint >= 0 && coupleControlPoint < points.Count || closed)
                {
                    float distance = (points[wrapIndex(coupleAnchorPoint)] - points[wrapIndex(coupleControlPoint)]).magnitude;
                    Vector3 direction = (points[wrapIndex(coupleAnchorPoint)] - points[index]).normalized;
                    points[wrapIndex(coupleControlPoint)] = points[wrapIndex(coupleAnchorPoint)] + direction * distance;
                }

            }
            
        }
    }

    /// <summary>
    /// Adds a road segment to the existing road path
    /// </summary>
    /// <param name="targetPos">The position at which a user would like there new road segment to end</param>
    public void addSegment(Vector3 targetPos)
    {
        points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
        points.Add(targetPos + Vector3.up);
        points.Add(targetPos);
    }

    /// <summary>
    /// Split the selected segment in two
    /// </summary>
    /// <param name="index">Index of the segment that will be split</param>
    public void divideSegment(int index)
    {
        Vector3[] selectedSegment = getSegment(index);
        Vector3 newAnchorPos = (selectedSegment[0] + selectedSegment[3]) * 0.5f;

        points.InsertRange(index * 3 + 2, new Vector3[] { newAnchorPos + Vector3.forward, newAnchorPos, newAnchorPos, newAnchorPos + Vector3.back });

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
    public Vector3[] getSegment(int i)
    {
        return new Vector3[] { points[i * 3], points[i * 3 + 1], points[i * 3 + 2], points[wrapIndex(i * 3 + 3)] };
    }

    /// <summary>
    /// Returns a Vector3 that corresponds to a position along the curve
    /// </summary>
    /// <param name="segmentIndex">Segment index.</param>
    /// <param name="t">value from [0,1] indicating the distance along the curve</param>
    /// <returns></returns>
    public Vector3 getPoint(int segmentIndex, float t)
    {
        Vector3[] pts = getSegment(segmentIndex);

        float omt = 1f - t;
        float omt2 = omt * omt;
        float t2 = t * t;
        return pts[0] * (omt2 * omt) +
                pts[1] * (3f * omt2 * t) +
                pts[2] * (3f * omt * t2) +
                pts[3] * (t2 * t);
    }

    public Vector3[] getRoadPathPoints(float spacing, float resolution = 1)
    {
        List<Vector3> pointsToReturn = new List<Vector3>();
        pointsToReturn.Add(points[0]);
        Vector3 previousPoint = points[0];

        float dstFromLastPoint = 0;

        for(int i =0; i < SegmentCount; i++)
        {
            float t = 0;
            
            while (t <= 1)
            {
                t += 0.1f;
                Vector3 calcPoint = getPoint(i, t);
                dstFromLastPoint += Vector3.Distance(previousPoint, calcPoint);

                while(dstFromLastPoint > spacing)
                {
                    float fixDistance = dstFromLastPoint - spacing;
                    Vector3 fixPos = calcPoint + (previousPoint - calcPoint).normalized * fixDistance;
                    pointsToReturn.Add(fixPos);
                    dstFromLastPoint = fixDistance;
                    previousPoint = fixPos;
                }

                previousPoint = calcPoint;
            }
             
        }

        return pointsToReturn.ToArray();
    }

    private int wrapIndex(int index)
    {
        return (index + points.Count) % points.Count;
    }

}
