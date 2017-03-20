using System;
using UnityEngine;

public abstract class BehaviorNode
{
    protected BehaviorResult Result;
    
    public virtual BehaviorResult Process()
    {
        throw new NotImplementedException();   
    }

    public virtual void Initialize()
    {
        throw new NotImplementedException();
    }

    public virtual void Reset()
    {
        throw new NotImplementedException();
    }

    protected void Print(string s)
    {
        Debug.Log(s);
    }
}