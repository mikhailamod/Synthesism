using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour {

    public Color lineColor;
    public bool showLines = true;
    public float pointRadius = 0.3f;

    public List<Transform> nodes = new List<Transform>();

    private void OnDrawGizmos()
    {
        if(showLines)
        {
            Gizmos.color = lineColor;

            Transform[] transforms = GetComponentsInChildren<Transform>();
            nodes = new List<Transform>();

            for (int i = 0; i < transforms.Length; i++)
            {
                if (transforms[i] != transform)
                {
                    nodes.Add(transforms[i]);
                }
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                Vector3 currentNodePosition = nodes[i].position;
                Vector3 previousNodePosition = Vector3.zero;
                if (i == 0 && nodes.Count > 1)
                {
                    previousNodePosition = nodes[nodes.Count - 1].position;
                }
                else
                {
                    previousNodePosition = nodes[i - 1].position;
                }

                Gizmos.DrawLine(previousNodePosition, currentNodePosition);
                Gizmos.DrawWireSphere(currentNodePosition, pointRadius);
            }
        }
    }
}
