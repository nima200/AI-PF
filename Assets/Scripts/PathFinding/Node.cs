using UnityEngine;

[System.Serializable]
public class Node : IHeapItem<Node>
{
    public bool Walkable;
    public Vector3 WorldPosition;
    public int GCost;
    public int HCost;
    public Node Parent;
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

    public int GridX;
    public int GridY;

    public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY)
    {
        Walkable = walkable;
        WorldPosition = worldPosition;
        GridX = gridX;
        GridY = gridY;
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
