using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public int gCost;
    public int hCost;
    public int gridX;
    public int gridY;
    int heapIndex;
    public bool walkable;
    public Vector3 worldPos;
    public Node parent;

    public Node(bool _walkable, Vector3 _wordPos, int gridx, int gridy)
    {
        walkable = _walkable;
        worldPos = _wordPos;
        gridX = gridx;
        gridY = gridy;
    }

    public int fCost
    {
        get{
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node node2Compare)
    {
        int compare = fCost.CompareTo(node2Compare.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(node2Compare.hCost);
        }
        return -compare;
    }
}
