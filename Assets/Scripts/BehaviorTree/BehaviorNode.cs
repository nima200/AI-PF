using System;
using UnityEngine;

public abstract class BehaviorNode
{
    public bool Initialized;
    public BehaviorResult Result { get; protected set; }
    public Professor Professor;
    public Plaque Plaque;
    
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
        Initialized = false;
    }

    protected void Print(string s)
    {
        Debug.Log(s);
    }

    public virtual void SetProf(Professor professor)
    {
        Professor = professor;
    }

    public virtual void SetPlaque(Plaque plaque)
    {
        Plaque = plaque;
    }
}