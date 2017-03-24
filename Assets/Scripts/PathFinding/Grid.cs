using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public LayerMask UnwalkableMask;
    public Vector2 GridWorldSize;
    public float NodeRadius;
    private float _nodeDiameter;
    private Node[,] _grid;
    private int _sizeX, _sizeY;
    public List<Node> Path;
    public bool DrawGizmos;
    public int MaxSize { get { return _sizeX * _sizeY; } }

    private void Start()
    {
        _nodeDiameter = NodeRadius * 2;
        _sizeX = Mathf.RoundToInt(GridWorldSize.x / _nodeDiameter);
        _sizeY = Mathf.RoundToInt(GridWorldSize.y / _nodeDiameter);
        CreateGrid();
    }

    private void CreateGrid()
    {
        _grid = new Node[_sizeX,_sizeY];
        var worldBottomLeft = transform.position - Vector3.right * GridWorldSize.x / 2 -
                                  Vector3.forward * GridWorldSize.y / 2;
        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeY; y++)
            {
                var worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + NodeRadius) +
                                     Vector3.forward * (y * _nodeDiameter + NodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, NodeRadius, UnwalkableMask));
                _grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            CreateGrid();
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + GridWorldSize.x / 2) / GridWorldSize.x;
        float percentY = (worldPosition.z + GridWorldSize.y / 2) / GridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((_sizeX - 1) * percentX);
        int y = Mathf.RoundToInt((_sizeY - 1) * percentY);
        return _grid[x, y];
    }

    public List<Node> GetNeighbors(Node node)
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
                    neighbors.Add(_grid[checkX, checkY]);
                }
            }
        }
        return neighbors;
    }

    private void OnDrawGizmos()
    {
        if (DrawGizmos)
        {
            Gizmos.DrawCube(transform.position, new Vector3(GridWorldSize.x, 1, GridWorldSize.y));
            if (_grid == null) return;
            foreach (var n in _grid)
            {
                Gizmos.color = (n.Walkable) ? Color.white : Color.red;
                if (Path != null)
                {
                    if (Path.Contains(n))
                    {
                        Gizmos.color = Color.black;
                    }
                }
                Gizmos.DrawCube(n.WorldPosition, Vector3.one * (_nodeDiameter - .1f));
            }
        }
    }
}
