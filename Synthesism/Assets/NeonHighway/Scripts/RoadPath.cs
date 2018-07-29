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
    /// Returns a segment (4 Vector3s) from the road path.
    /// </summary>
    /// <param name="i">The segment returned.</param>
    /// <returns></returns>
    public Vector3[] getSegment(int i)
    {
        return new Vector3[] { points[i * 3], points[i * 3 + 1], points[i * 3 + 2], points[wrapIndex(i * 3 + 3)] };
    }

    private int wrapIndex(int index)
    {
        return (index + points.Count) % points.Count;
    }

}
