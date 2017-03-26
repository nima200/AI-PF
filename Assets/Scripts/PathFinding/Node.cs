using System.Collections;
using UnityEngine;
using System.Reflection.Emit;

[System.Serializable]
public class Node : IHeapItem<Node>
{
    public int X;
    public int Y;
    public Node Parent;
    public bool Walkable { get; set; }
    public Vector3 WorldPosition;
    public int GCost;
    public int HCost;
    
    private int _heapIndex;
    public int HeapIndex
    {
        get { return _heapIndex; }
        set { _heapIndex = value; }
    }

    public int FCost
    {
        get { return GCost + HCost; }
    }

    

    public Node(bool walkable, Vector3 worldPosition, int x, int y)
    {
        Walkable = walkable;
        WorldPosition = worldPosition;
        X = x;
        Y = y;
    }

    public int CompareTo(Node other)
    {
        int compare = FCost.CompareTo(other.FCost);
        if (compare == 0)
        {
            compare = HCost.CompareTo(other.HCost);

        }
        return -compare;
    }
}
