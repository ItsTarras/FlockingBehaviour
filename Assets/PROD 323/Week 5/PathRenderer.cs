using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRenderer : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public LineRenderer lr;

    private TerrainGraph graph;
    private float gridOffset = 0.5f;
    void Start()
    {
        graph = new TerrainGraph();
    }

    void Update()
    {
        RenderPath();
    }

    private void RenderPath()
    {
        // Start node position
        int startX = (int)start.position.x;
        int startZ = (int)start.position.z;

        Node startNode = graph.grid[startX, startZ];
        
        // End node position
        int endX = (int)end.position.x;
        int endZ = (int)end.position.z;

        Node endNode = graph.grid[endX, endZ];

        // Get list of nodes making the path
        List<Node> path = BreadthFirstSearch.FindPath(graph, startNode, endNode);
        
        // Create the line using an array of vertices based on the nodes in the path
        // Grid offset points to the center of the node cell
        Vector3[] lineVertices = new Vector3[path.Count];
        int index = 0;

        foreach(Node n in path) 
        {
            float x = n.nodePosition.X + gridOffset;
            float y = n.nodeHeight + gridOffset;
            float z = n.nodePosition.Y + gridOffset;

            lineVertices[index++] = new Vector3(x, y, z);
        }

        // Render the path
        lr.positionCount = path.Count;
        lr.SetPositions(lineVertices);
    }
}
