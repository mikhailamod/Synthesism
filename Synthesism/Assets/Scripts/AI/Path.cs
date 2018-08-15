using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour {

    public Color lineColor;
    public bool showLines = true;
    public float pointRadius = 0.3f;

    public List<Node> nodes = new List<Node>();

    private void OnDrawGizmos()
    {
        if(showLines)
        {
            Gizmos.color = lineColor;

            Node[] transforms = GetComponentsInChildren<Node>();
            nodes = new List<Node>();

            for (int i = 0; i < transforms.Length; i++)
            {
                if (transforms[i] != transform)
                {
                    nodes.Add(transforms[i]);
                }
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                Vector3 currentNodePosition = nodes[i].transform.position;
                Vector3 previousNodePosition = Vector3.zero;
                if (i == 0 && nodes.Count > 1)
                {
                    previousNodePosition = nodes[nodes.Count - 1].transform.position;
                }
                else
                {
                    previousNodePosition = nodes[i - 1].transform.position;
                }

                Gizmos.DrawLine(previousNodePosition, currentNodePosition);
                Gizmos.DrawWireSphere(currentNodePosition, pointRadius);
            }
        }
    }

    public List<Node> getNodeList()
    {
        Node[] nodes = GetComponentsInChildren<Node>();
        List<Node> output = new List<Node>();

        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i] != transform)
            {
                output.Add(nodes[i]);
            }
        }
        return output;
    }
}
