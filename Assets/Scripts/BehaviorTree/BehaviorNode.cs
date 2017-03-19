using System;
using UnityEngine;

public abstract class BehaviorNode
{
    protected BehaviorResult Result;
    // Empty constructor made by compiler
    public abstract BehaviorResult Tick();

    protected void Print(string s)
    {
        Debug.Log(s);
    }
}