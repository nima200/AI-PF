using System;
using UnityEngine;

public struct PathRequest
{
    public Vector3 PathStart;
    public Vector3 PathEnd;
    public Action<Vector3[], bool> CallBack;
    public Agent Caller;

    public PathRequest(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callBack, Agent caller)
    {
        PathStart = pathStart;
        PathEnd = pathEnd;
        CallBack = callBack;
        Caller = caller;
    }
}