using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;
using System.Diagnostics;
using System;

public class PathFinder : MonoBehaviour
{
    private PathRequestManager _pathRequestManager;
    private Grid _grid;

    private void Awake()
    {
        _pathRequestManager = GetComponent<PathRequestManager>();
        _grid = GetComponent<Grid>();
    }

    // A*
    private IEnumerator FindPath(Vector3 startPosition, Vector3 endPosition)
    {
        var sw = new Stopwatch();
        sw.Start();

        var waypoints = new Vector3[0];
        bool pathSuccess = false;

        var startNode = _grid.NodeFromWorld(startPosition);
        var endNode = _grid.NodeFromWorld(endPosition);

        if (startNode.Walkable && endNode.Walkable)
        {
            // The set of nodes to be evaluated
            var openSet = new Heap<Node>(_grid.MaxSize);
            // The set of nodes already evaluated
            var closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                var currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                // If we have reached the destination, retrace the path back to start and end
                if (currentNode == endNode)
                {
                    sw.Stop();
                    pathSuccess = true;
                    break;
                }

                /*
                 * Actual Algorithm core:
                 * for each neighbor of current node, if the neighbor is not traversable or it's already evaluated, 
                 * then skip to the next neighbor. If not, if the path from current node to the neighbor node is cheaper
                 * than the neighbor's cost of traversal, or if the neighbor is evaluated already, 
                 * then you can calculate the new FCost through H and G cost, and if it is not considered open for evaluation already,
                 * then you can add it to the list of nodes to be evaluated. 
                 * Note that stuff previously removed from open for evaluation set will not get added back to it as the optimal distance
                 * was chosen anyways.
                 */
                foreach (var neighbor in _grid.GetNeighbors(currentNode))
                {
                    if (!neighbor.Walkable || closedSet.Contains(neighbor)) continue;
                    int newCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor) + neighbor.MovementPenalty;
                    if (newCostToNeighbor >= neighbor.GCost && openSet.Contains(neighbor)) continue;
                    neighbor.GCost = newCostToNeighbor;
                    neighbor.HCost = GetDistance(neighbor, endNode);
                    neighbor.Parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                    else
                    {
                        openSet.UpdateItem(neighbor);
                    }
                }
            }
        }
        yield return null;
        if (pathSuccess)
        {
             waypoints = RetracePath(startNode, endNode);
        }
        _pathRequestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    /// <summary>
    /// Basic helper method to retrace the path taken through tracing the node parent repetitively until start is reached.
    /// </summary>
    /// <param name="start">The start of the path</param>
    /// <param name="end">The end of the path</param>
    private Vector3[] RetracePath(Node start, Node end)
    {
        var path = new List<Node>();
        var currentNode = end;
        while (currentNode != start) 
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        var waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;

    }

    /// <summary>
    /// Attempts to simplify path so that it creates waypoints only when the path is changing its direction, instead of a waypoint all along the path.
    /// </summary>
    /// <param name="path">The path to simplify</param>
    /// <returns>The simplified waypoints</returns>
    private static Vector3[] SimplifyPath(IList<Node> path)
    {
        var waypoints = new List<Vector3>();
        var directionOld = Vector2.zero;
        for (int i = 1; i < path.Count; i++)
        {
            var directionNew = new Vector2(path[i-1].GridX - path[i].GridX, path[i - 1].GridY - path[i].GridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].WorldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

     /// <summary>
     /// First count on the x axis how many nodes away you are from the destination, then do the same on the y axis
     /// Take the lowst number and that gives us the number of diagonal nodes we need to take to be inline with the end node
     /// To calculate howmany inline moves you need, subtract lower number from higher number, and that's the number of inline moves
     /// Diagonal distance = 14(sqrt(2) * 10)
     /// Inline distance = 10(1 * 10)
     /// </summary>
     /// <param name="a"></param>
     /// <param name="b"></param>
     /// <returns></returns>
    private static int GetDistance(Node a, Node b)
    {
        int distanceX = Mathf.Abs(a.GridX - b.GridX);
        int distanceY = Mathf.Abs(a.GridY - b.GridY);

        if (distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }
        return 14 * distanceX + 10 * (distanceY - distanceX);
    }

    public void StartFindPath(Vector3 pathStart, Vector3 pathEnd)
    {
        StartCoroutine(FindPath(pathStart, pathEnd));
    }
}
