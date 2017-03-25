using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public LayerMask UnwalkableMask;
    public Vector2 GridWorldSize;
    public float NodeRadius;
    private float _nodeDiameter;
    public Node3D[,] Nodes { get; private set; }
    private int _sizeX, _sizeY;
    public List<Node> Path;
    public bool DrawGizmos;
    public int MaxSize { get { return _sizeX * _sizeY; } }

    private void Awake()
    {
        _nodeDiameter = NodeRadius * 2;
        _sizeX = Mathf.RoundToInt(GridWorldSize.x / _nodeDiameter);
        _sizeY = Mathf.RoundToInt(GridWorldSize.y / _nodeDiameter);
        CreateGrid();
    }

    private void CreateGrid()
    {
        Nodes = new Node3D[_sizeX,_sizeY];
        var worldBottomLeft = transform.position - Vector3.right * GridWorldSize.x / 2 -
                                  Vector3.forward * GridWorldSize.y / 2;
        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeY; y++)
            {
                var worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + NodeRadius) +
                                     Vector3.forward * (y * _nodeDiameter + NodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, NodeRadius, UnwalkableMask));
                Nodes[x, y] = new Node3D(walkable, worldPoint, x, y);
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition, int time)
    {
        float percentX = (worldPosition.x + GridWorldSize.x / 2) / GridWorldSize.x;
        float percentY = (worldPosition.z + GridWorldSize.y / 2) / GridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((_sizeX - 1) * percentX);
        int y = Mathf.RoundToInt((_sizeY - 1) * percentY);
        return Nodes[x, y].Nodes[time];
    }

    public List<Node> GetNeighbors(Node node, int currentTime)
    {
        var neighbors = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                int checkX = node.GridX + x;
                int checkY = node.GridY + y;
                if (checkX >= 0 && checkX < _sizeX && checkY >= 0 && checkY < _sizeY)
                {
                    neighbors.Add(Nodes[checkX, checkY].Nodes[currentTime + 1]);
                }
            }
        }
        return neighbors;
    }

    public int GetTimeStepCount()
    {
        return Nodes[0, 0].Nodes.Count;
    }

    public void RequestNewTimeStep()
    {
        foreach (var node3D in Nodes)
        {
            node3D.NewTimeStep();
        }
    }

    public void RequestDeleteTimeStep()
    {
        foreach (var node3D in Nodes)
        {
            node3D.DeleteTimeStep();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, 1, GridWorldSize.y));
        if (Nodes == null || !DrawGizmos) return;
        foreach (var n in Nodes)
        {
            Gizmos.color = (n.Nodes[0].Walkable) ? Color.white : Color.red;
            Gizmos.DrawCube(n.Nodes[0].WorldPosition, Vector3.one * (_nodeDiameter - .1f));
        }
    }
}
