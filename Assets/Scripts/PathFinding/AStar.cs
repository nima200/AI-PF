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

    private IEnumerator FindPath(Vector3 start, Vector3 target)
    {

        var waypoints = new Vector3[0];
        var foundPath = false;

        // Convert the two world positions into actual nodes.
        var startNode = _grid.NodeFromWorldPoint(start);
        var targetNode = _grid.NodeFromWorldPoint(target);


        // The sets
        if (startNode.Walkable && targetNode.Walkable)
        {
            var openSet = new Heap<Node>(_grid.MaxSize);
            var closedSet = new HashSet<Node>();

            openSet.Add(startNode);
            while (openSet.Count > 0)
            {
                var currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                // If this succeeds, it means path is found.
                if (currentNode == targetNode)
                {
                    foundPath = true;
                    break;
                }

                foreach (var neighbor in _grid.GetNeighbors(currentNode))
                {
                    if (!neighbor.Walkable || closedSet.Contains(neighbor)) continue;

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
                        else
                        {
                            openSet.UpdateItem(neighbor);
                        }
                    }
                }
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
            waypoints = RetracePath(startNode, targetNode);
        }
        // Alert the Path Request Manager that the path finding was completed 
        // and hand over the path and result of pathfinding.
        _requestManager.FinishedProcessingPath(waypoints, foundPath);
    }

    private Vector3[] RetracePath(Node startNode, Node endNode)
    {
        var path = new List<Node>();
        var currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        var  waypoints = NodesToVector3s(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    private Vector3[] SimplifyPath(IList<Node> path)
    {
        var waypoints = new List<Vector3>();
        var directionOld = Vector2.zero;
        for (int i = 1; i < path.Count; i++)
        {
            var directionNew = new Vector2(path[i-1].GridX - path[i].GridX, path[i-1].GridY - path[i].GridY);
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
        int distanceX = Mathf.Abs(a.GridX - b.GridX);
        int distanceY = Mathf.Abs(a.GridY - b.GridY);

        if (distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }
        return 14 * distanceX + 10 * (distanceY * distanceX);
    }

    public void StartFindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        StartCoroutine(FindPath(startPosition, targetPosition));
    }
}
