using System;
using UnityEngine;

/// <summary>
/// Class that holds the data structures needed for a path to be requested.
/// </summary>
public struct PathRequest
{
    public Vector3 PathStart;
    public Vector3 PathEnd;
    public Action<Vector3[], bool> CallBack;
    public Agent RequestAgent;

    /// <summary>
    /// Creates a new path request.
    /// </summary>
    /// <param name="pathStart">The start of the path</param>
    /// <param name="pathEnd">The end of the path</param>
    /// <param name="callBack">The function to be called once the path finding is done</param>
    /// <param name="requestAgent">The agent requesting the path</param>
    public PathRequest(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callBack, Agent requestAgent)
    {
        PathStart = pathStart;
        PathEnd = pathEnd;
        CallBack = callBack;
        RequestAgent = requestAgent;
    }
}