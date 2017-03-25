using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class AStar : MonoBehaviour
{
    private PathRequestManager _requestManager;
    private Grid _grid;
    private Dictionary<string, Agent> _reservationTable = new Dictionary<string, Agent>();

    private void Awake()
    {
        _grid = GetComponent<Grid>();
        _requestManager = GetComponent<PathRequestManager>();
    }

    private IEnumerator FindPath(Vector3 start, Vector3 target, Agent caller)
    {

        var waypoints = new Vector3[0];
        bool foundPath = false;

        // Convert the two start and end positions into actual nodes.
        var startNode = _grid.NodeFromWorldPoint(start, 0);
        var targetNode = _grid.NodeFromWorldPoint(target, 0);


        // The sets
        if (startNode.Walkable)
        {
            var openSet = new List<Node>();
            var closedSet = new HashSet<Node>();

            openSet.Add(startNode);
            int time = 0;
            while (openSet.Count > 0)
            {

                // currentnode = node with lowest FCost. This is added to the path.
                var currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost)
                    {
                        if (openSet[i].HCost < currentNode.HCost)
                            currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                // If this succeeds, it means path is found.
                if (currentNode.X == targetNode.X && currentNode.Y == targetNode.Y)
                {
                    targetNode = _grid.NodeFromWorldPoint(target, time);
                    foundPath = true;
                    break;
                }

                foreach (var neighbor in _grid.GetNeighbors(currentNode, time))
                {
                    string key = neighbor.X + ":" + neighbor.Y + ":" + neighbor.T;
                    if (!neighbor.Walkable || closedSet.Contains(neighbor) || _reservationTable.ContainsKey(key)) continue;

                    int newMovementCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);
                    if (newMovementCostToNeighbor < neighbor.GCost || !openSet.Contains(neighbor))
                    {
                        neighbor.GCost = newMovementCostToNeighbor;
                        neighbor.HCost = GetDistance(neighbor, targetNode);
                        neighbor.Parent = currentNode;
                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
//                        else
//                        {
//                            openSet.UpdateItem(neighbor);
//                        }
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
            waypoints = RetracePath(startNode, targetNode, caller);
        }
        // Alert the Path Request Manager that the path finding was completed 
        // and hand over the path and result of pathfinding.
        _requestManager.FinishedProcessingPath(waypoints, foundPath);
    }

    private Vector3[] RetracePath(Node startNode, Node endNode, Agent caller)
    {
        var path = new List<Node>();
        var currentNode = endNode;
        string key = currentNode.X + ":" + currentNode.Y + ":" + currentNode.T;
        _reservationTable.Add(key, caller);
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
            key = currentNode.X + ":" + currentNode.Y + ":" + currentNode.T;
            _reservationTable.Add(key, caller);
        }
        var waypoints = NodesToVector3s(path);
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

    private Vector3[] NodesToVector3s(IEnumerable<Node> path)
    {
        return path.Select(node => node.WorldPosition).ToArray();
    }

    private int GetDistance(Node a, Node b)
    {
        int distanceX = Mathf.Abs(a.X - b.X);
        int distanceY = Mathf.Abs(a.Y - b.Y);

        if (distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }
        return 14 * distanceX + 10 * (distanceY * distanceX);
    }

    public void StartFindPath(Vector3 startPosition, Vector3 targetPosition, Agent caller)
    {
        StartCoroutine(FindPath(startPosition, targetPosition, caller));
    }
}
