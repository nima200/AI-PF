using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class PathRequestManager : MonoBehaviour
{
    private readonly Queue<PathRequest> _pathRequests = new Queue<PathRequest>();
    private PathRequest _currentPathRequest;
    private static PathRequestManager _instance;
    private AStar _pathFinder;
    private bool _isProcessingPath;

    private void Awake()
    {
        _instance = this;
        _pathFinder = GetComponent<AStar>();
    }

    /// <summary>
    /// Can be used to request a new path from the request manager. Needs a start and an end, as well as a method to be called
    /// once the path has been found. 
    /// </summary>
    /// <param name="pathStart">The start of the path</param>
    /// <param name="pathEnd">The end of the path</param>
    /// <param name="callback">The method to be called once the path has been found</param>
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        var newRequest = new PathRequest(pathStart, pathEnd, callback);
        _instance._pathRequests.Enqueue(newRequest);
        _instance.TryProcessNext();
    }

    /// <summary>
    /// Attempts to process a path if not currently processing one, and if path request queue is not empty.
    /// </summary>
    private void TryProcessNext()
    {
        if (!_isProcessingPath && _pathRequests.Count > 0)
        {
            _currentPathRequest = _pathRequests.Dequeue();
            _isProcessingPath = true;
            _pathFinder.StartFindPath(_currentPathRequest.PathStart, _currentPathRequest.PathEnd);
        }
    }

    /// <summary>
    /// Called by the pathfinding script (AStar) once it's finished finding the path.
    /// </summary>
    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        _currentPathRequest.CallBack(path, success);
        _isProcessingPath = false;
        TryProcessNext();
    }
}