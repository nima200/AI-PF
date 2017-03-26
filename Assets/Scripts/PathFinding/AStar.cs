using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class AStar : MonoBehaviour
{
    private PathRequestManager _requestManager;
    private Grid _grid;
//    private Dictionary<string, Agent> _reservationTable = new Dictionary<string, Agent>();

    private void Awake()
    {
        _grid = GetComponent<Grid>();
        _requestManager = GetComponent<PathRequestManager>();
    }

    private IEnumerator FindPath(Vector3 start, Vector3 target, Agent agent)
    {

        var waypoints = new Vector3[0];
        bool foundPath = false;

        // Convert the two start and end positions into actual nodes.
        var startCell = _grid.CellFromWorldPoint(start);
        var targetCell = _grid.CellFromWorldPoint(target);


        // The sets
        if (startCell.GenericNode.Walkable)
        {
            var openSet = new List<Cell>();
            var closedSet = new HashSet<Cell>();

            openSet.Add(startCell);
            int time = 0;
            while (openSet.Count > 0)
            {

                // currentnode = node with lowest FCost. This is added to the path.
                var currentCell = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].GenericNode.FCost < currentCell.GenericNode.FCost || openSet[i].GenericNode.FCost == currentCell.GenericNode.FCost)
                    {
                        if (openSet[i].GenericNode.HCost < currentCell.GenericNode.HCost)
                            currentCell = openSet[i];
                    }
                }

                openSet.Remove(currentCell);
                closedSet.Add(currentCell);
                // Reserve the current cell at the current time and at the next time step => (to prevent head-head collision)
                currentCell.Reserve(agent, time);
                currentCell.Reserve(agent, time + 1);

                // If this succeeds, it means path is found.
                if (currentCell.X == targetCell.X && currentCell.Y == targetCell.Y)
                {
                    targetCell = _grid.CellFromWorldPoint(target);
                    foundPath = true;
                    break;
                }

                foreach (var neighbor in _grid.GetNeighbors(currentCell))
                {
                    if (!neighbor.GenericNode.Walkable || closedSet.Contains(neighbor) || neighbor.IsReserved(time + 1)) continue;

                    int newMovementCostToNeighbor = currentCell.GenericNode.GCost + GetDistance(currentCell, neighbor);
                    if (newMovementCostToNeighbor < neighbor.GenericNode.GCost || !openSet.Contains(neighbor))
                    {
                        neighbor.SetGCost(newMovementCostToNeighbor);
                        neighbor.SetHCost(GetDistance(neighbor, targetCell));
                        neighbor.SetParent(currentCell.GenericNode);
                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
                time++;
            }
        }
        else
        {
            print("Path finding is not possible as either the start or end node are not walkable.");
        }
        yield return null;
        if (foundPath)
        {
            // Attempt to retrace the path back if the path was actually found
            waypoints = RetracePath(startCell, targetCell);
        }
        // Alert the Path Request Manager that the path finding was completed 
        // and hand over the path and result of pathfinding.
        _requestManager.FinishedProcessingPath(waypoints, foundPath);
    }

    private static Vector3[] RetracePath(Cell startCell, Cell endCell)
    {
        var path = new List<Node>();
        var currentCell = endCell.GenericNode;
        while (currentCell != startCell.GenericNode)
        {
            path.Add(currentCell);
            currentCell = currentCell.Parent;
        }
        var waypoints = NodesToVector3S(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    private Vector3[] SimplifyPath(IList<Node> path)
    {
        var waypoints = new List<Vector3>();
        var directionOld = Vector2.zero;
        for (int i = 1; i < path.Count; i++)
        {
            var directionNew = new Vector2(path[i-1].X - path[i].X, path[i-1].Y - path[i].Y);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].WorldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    private static Vector3[] NodesToVector3S(IEnumerable<Node> path)
    {
        return path.Select(node => node.WorldPosition).ToArray();
    }

    private static int GetDistance(Cell a, Cell b)
    {
        int distanceX = Mathf.Abs(a.X - b.X);
        int distanceY = Mathf.Abs(a.Y - b.Y);

        if (distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }
        return 14 * distanceX + 10 * (distanceY * distanceX);
    }

    public void StartFindPath(Vector3 startPosition, Vector3 targetPosition, Agent agent)
    {
        StartCoroutine(FindPath(startPosition, targetPosition, agent));
    }
}
