using System.Collections.Generic;
using UnityEngine;

public class TerrainGraph : MonoBehaviour
{
    private TerrainData tData;
    private int tWidth; 
    private int tLength;
    private float gridOffset = 0.5f;
    public Node[,] grid; 

    public TerrainGraph()
    {
        // Get reference of the active terrain on the scene
        tData = Terrain.activeTerrain.terrainData;

        // Create a representation of the graph using the terrain size
        // Taking the x (width) and z (length) values only
        // Grid offset points to the center of the node cell
        tWidth = Mathf.FloorToInt(tData.size.x);
        tLength = Mathf.FloorToInt(tData.size.z);
        grid = new Node[tWidth, tLength];

        // Populate the grid with nodes
        for(int x = 0; x < tWidth; x++)
        {
            for(int z = 0; z < tLength; z++)
            {
                grid[x, z] = new Node(x, z);
                grid[x, z].nodeHeight = Terrain.activeTerrain.SampleHeight(new Vector3(x + gridOffset, 0, z + gridOffset));
            }
        }
    }

    public List<Node> GetNeighbours(Node n)
    {
        List<Node> neighbours = new List<Node>();

        // Take all the nodes from all cardinal and ordinal directions
        Vector2[] directions =
        {
            new Vector2(-1, 0), // west
            new Vector2(-1, 1), // north-west
            new Vector2(0, 1),  // north
            new Vector2(1, 1),  // north-east
            new Vector2(1, 0),  // east
            new Vector2(1, -1), // south-east
            new Vector2(0, -1), // south
            new Vector2(-1, -1) // south-west
        };

        // Find all nodes via the 8 directions
        foreach (Vector2 dir in directions)
        {
            Vector2 v = new Vector2(dir.x, dir.y) + new Vector2(n.nodePosition.X, n.nodePosition.Y);

            // Check if the neighbouring node actually exist in the terrain
            // y here is actually the z
            bool doExist = (v.x >= 0 && v.x < tWidth && v.y >=0 && v.y < tLength) ? true : false;

            if(doExist) 
            {
                neighbours.Add(grid[(int)v.x, (int)v.y]);
            }
        }

        return neighbours;
    }


}
