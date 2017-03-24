using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : MonoBehaviour {

    private readonly Queue<PathRequest> _pathRequestQueue = new Queue<PathRequest>();
    private PathRequest _currentPathRequest;
    private static PathRequestManager _instance;
    private PathFinder _pathFinder;
    private bool _isProcessingPath;

    /// <summary>
    /// Puts a path defined by the path start and path end on the request queue.
    /// </summary>
    /// <param name="pathStart">The start of the path</param>
    /// <param name="pathEnd">The end of the path</param>
    /// <param name="callBack">The method passed which is called once the path is calculated</param>
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callBack)
    {
        var newPathRequest = new PathRequest(pathStart, pathEnd, callBack);
        _instance._pathRequestQueue.Enqueue(newPathRequest);
        _instance.TryProcessNext();
    }

    private void Awake()
    {
        _instance = this;
        _pathFinder = GetComponent<PathFinder>();
    }

    /// <summary>
    /// Attempts to process the first path in the queue if not currently processing and queue not empty.
    /// </summary>
    private void TryProcessNext()
    {
        if (_isProcessingPath || _pathRequestQueue.Count <= 0) return;
        _currentPathRequest = _pathRequestQueue.Dequeue();
        _isProcessingPath = true;
        _pathFinder.StartFindPath(_currentPathRequest.PathStart, _currentPathRequest.PathEnd);
    }

    /// <summary>
    /// Called by the pathfinding script once its finished finding the path.
    /// Calls the callback function afterwards.
    /// </summary>
    /// <param name="path">The path</param>
    /// <param name="success">The result of the path finding</param>
    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        _currentPathRequest.CallBack(path, success);
        _isProcessingPath = false;
        TryProcessNext();
    }

    /// <summary>
    /// Struct that contains information regarding the request of a path.
    /// </summary>
    public struct PathRequest
    {
        public Vector3 PathStart;
        public Vector3 PathEnd;
        public Action<Vector3[], bool> CallBack;

        public PathRequest(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callBack)
        {
            PathStart = pathStart;
            PathEnd = pathEnd;
            CallBack = callBack;
        }
    }
}
