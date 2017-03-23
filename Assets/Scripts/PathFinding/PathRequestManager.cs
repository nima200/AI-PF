using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : MonoBehaviour {
    private Queue<PathRequest> _pathRequestQueue = new Queue<PathRequest>();
    private PathRequest currentPathRequest;
    private static PathRequestManager _instance;
    private PathFinder pathFinder;
    private bool isProcessingPath;

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callBack)
    {
        var newPathRequest = new PathRequest(pathStart, pathEnd, callBack);
        _instance._pathRequestQueue.Enqueue(newPathRequest);
        _instance.TryProcessNext();
    }

    private void Awake()
    {
        _instance = this;
        pathFinder = GetComponent<PathFinder>();
    }

    private void TryProcessNext()
    {
        if (!isProcessingPath && _pathRequestQueue.Count > 0)
        {
            currentPathRequest = _pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathFinder.StartFindPath(currentPathRequest.PathStart, currentPathRequest.PathEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.CallBack(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest
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
