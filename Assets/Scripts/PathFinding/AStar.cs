using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    public Transform Seeker, Target;
    private Grid _grid;
    public bool FoundPath;

    private void Awake()
    {
        _grid = GetComponent<Grid>();
    }

    private void Update()
    {
        FindPath(Seeker.position, Target.position);
    }

    private void FindPath(Vector3 start, Vector3 target)
    {
        FoundPath = false;
        // Convert the two world positions into actual nodes.
        var startNode = _grid.NodeFromWorldPoint(start);
        var targetNode = _grid.NodeFromWorldPoint(target);
        // The sets
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
                FoundPath = true;
                RetracePath(startNode, targetNode);
                return;
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

    private void RetracePath(Node startNode, Node endNode)
    {
        var path = new List<Node>();
        var currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        path.Reverse();
        _grid.Path = path;
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
}
