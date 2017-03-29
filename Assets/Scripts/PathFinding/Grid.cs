using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
    public List<IdleWaypoint> IdleWaypoints = new List<IdleWaypoint>();

    private void Awake()
    {
        _nodeDiameter = NodeRadius * 2;
        _sizeX = Mathf.RoundToInt(GridWorldSize.x / _nodeDiameter);
        _sizeY = Mathf.RoundToInt(GridWorldSize.y / _nodeDiameter);
        CreateGrid();
        IdleWaypoints = FindObjectsOfType<IdleWaypoint>().ToList();
    }
    /// <summary>
    /// Creates a 2D grid of cells. Evaluates whether a cell can be marked as walkable or not depending on whether
    /// there is an object of layer "unwalkable mask" over the cell (using Physics.CheckSphere).
    /// </summary>
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
    /// <summary>
    /// Retreives a cell given a world position.
    /// </summary>
    /// <param name="worldPosition">The position to fetch a cell from.</param>
    /// <returns>The cell at the given position.</returns>
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

    /// <summary>
    /// Gets a list of the neighbors of the cell, given the neighbors are still considered in the boundaries 
    /// of the grid. Ignores the neighbor at location 0 , 0 compared to the given cell, since that neighbor
    /// is the cell itself.
    /// </summary>
    /// <param name="cell">The cell to find neighbors for.</param>
    /// <returns>The list of neighbors of the cell.</returns>
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

    /// <summary>
    /// Resets the reservations of an agent over the whole grid.
    /// </summary>
    /// <param name="agent">The agent who's reservations should be cancelled.</param>
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
