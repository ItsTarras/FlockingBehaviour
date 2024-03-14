using System.Collections.Generic;

public class Graph
{
    private int nodeCount;
    private LinkedList<int>[] nodeLists;
    
    public Graph(int n)
    {
        nodeCount = n;
        nodeLists = new LinkedList<int>[n];

        for(int index = 0; index < n; index++)
        {
            nodeLists[index] = new LinkedList<int>();
        }
    }

    public void AddConnection(int nodeA, int nodeB)
    {
        nodeLists[nodeA].AddLast(nodeB);
    }
}

