using System;
using UnityEngine;

public abstract class BehaviorNode
{
    public bool Initialized;
    public string Professor;
    public BehaviorResult Result { get; protected set; }
    
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

    public virtual void SetProf(string professorName)
    {
        Professor = professorName;
    }
}