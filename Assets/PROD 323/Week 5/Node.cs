using System;
using System.Numerics;

public class Node: IComparable<Node> 
{
    public Vector2 nodePosition;
    public float nodeHeight;

    public Node(int x, int z)
    {
        nodePosition = new Vector2(x, z);   
    }

    // Interface implementation of IComparable
    public int CompareTo(Node other)
    {
        return 0;
    }
}
