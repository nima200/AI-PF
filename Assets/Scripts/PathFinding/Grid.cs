using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public LayerMask UnwalkableMask;
    public Vector2 GridWorldSize;
    public float NodeRadius;
    private float _nodeDiameter;
    public Cell[,] Cells { get; private set; }
    private int _sizeX, _sizeY;
    public List<Node> Path;
    public bool DrawGizmos;
    public int MaxSize { get { return _sizeX * _sizeY; } }
    public int TimeStepWindow;

    private void Awake()
    {
        _nodeDiameter = NodeRadius * 2;
        _sizeX = Mathf.RoundToInt(GridWorldSize.x / _nodeDiameter);
        _sizeY = Mathf.RoundToInt(GridWorldSize.y / _nodeDiameter);
        CreateGrid();
    }

    private void CreateGrid()
    {
        Cells = new Cell[_sizeX,_sizeY];
        var worldBottomLeft = transform.position - Vector3.right * GridWorldSize.x / 2 -
                                  Vector3.forward * GridWorldSize.y / 2;
        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeY; y++)
            {
                var worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + NodeRadius) +
                                    Vector3.forward * (y * _nodeDiameter + NodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, NodeRadius, UnwalkableMask));
                var node = new Node(walkable, worldPoint, x, y);
                Cells[x, y] = new Cell(x, y, TimeStepWindow, node);
            }
        }
    }

    public Cell CellFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + GridWorldSize.x / 2) / GridWorldSize.x;
        float percentY = (worldPosition.z + GridWorldSize.y / 2) / GridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((_sizeX - 1) * percentX);
        int y = Mathf.RoundToInt((_sizeY - 1) * percentY);
        return Cells[x, y];
    }

    public List<Cell> GetNeighbors(Cell cell)
    {
        var neighbors = new List<Cell>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                // pause neighbor
                if (x == 0 && y == 0)
                {
                    continue;
                }
                int checkX = cell.X + x;
                int checkY = cell.Y + y;
                if (checkX >= 0 && checkX < _sizeX && checkY >= 0 && checkY < _sizeY)
                {
                    neighbors.Add(Cells[checkX, checkY]);
                }
            }
        }
        return neighbors;
    }

    public void ResetGridReservations(Agent agent)
    {
        foreach (var cell in Cells)
        {
            cell.ResetReservationTable(agent);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, 1, GridWorldSize.y));
        if (Cells == null || !DrawGizmos) return;
        foreach (var cell in Cells)
        {
            Gizmos.color = (cell.GenericNode.Walkable) ? Color.white : Color.red;
            Gizmos.DrawCube(cell.GenericNode.WorldPosition, Vector3.one * (_nodeDiameter - .1f));
        }
    }
}
