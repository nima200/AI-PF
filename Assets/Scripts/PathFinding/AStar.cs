using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class AStar : MonoBehaviour
{
    private PathRequestManager _requestManager;
    private Grid _grid;

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

                // Check all neighbors and get the one with the lowest FCost to be the next node to include in the path.
                foreach (var neighbor in _grid.GetNeighbors(currentCell))
                {
                    if (!neighbor.GenericNode.Walkable || closedSet.Contains(neighbor) || neighbor.IsReserved(time + 1) || neighbor.IsReserved(time)) continue;

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
                        else
                        {
                            openSet[openSet.IndexOf(neighbor)] = neighbor;
                        }
                    }
                }
                time++;
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

    /// <summary>
    /// Method that retraces a path through the cell parents, given two parents.
    /// Starts from the end cell (end of the path), all the way up to the start cell (start of the path)
    /// </summary>
    /// <param name="startCell">The start of the path</param>
    /// <param name="endCell">The end of the path</param>
    /// <returns></returns>
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

    /// <summary>
    /// Static method that retrieves a node, given a world position.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private static Vector3[] NodesToVector3S(IEnumerable<Node> path)
    {
        return path.Select(node => node.WorldPosition).ToArray();
    }

    /// <summary>
    /// Method that gets the distance between two cells.
    /// </summary>
    /// <param name="a">The first cell</param>
    /// <param name="b">The second cell</param>
    /// <returns>Distance between the two cells given</returns>
    private static int GetDistance(Cell a, Cell b)
    {
        int distanceX = Mathf.Abs(a.X - b.X);
        int distanceY = Mathf.Abs(a.Y - b.Y);
        if (distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }
        return 14 * distanceX + 10 * (distanceY - distanceX);
    }

    /// <summary>
    /// A wrapper method that covers the find path method. This is used to spread out the path finding requests over several frames,
    /// so that different pathfinding requests are done on different frames, rather than having the game pause for a brief second until
    /// all agents find their path, and only continuing once they are all done.
    /// </summary>
    /// <param name="startPosition">The start of the path</param>
    /// <param name="targetPosition">The end of the path</param>
    /// <param name="agent">The agent requesting a path to be found</param>
    public void StartFindPath(Vector3 startPosition, Vector3 targetPosition, Agent agent)
    {
        StartCoroutine(FindPath(startPosition, targetPosition, agent));
    }
}
