using System;
using UnityEngine;

public abstract class BehaviorNode
{
    public bool Initialized;
    public Professor Professor;
    public Plaque ProfessorPlaque;
    public Agent Agent;
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

    public virtual void SetProfPlaque(Plaque plaque)
    {
        ProfessorPlaque = plaque;
    }

    public virtual void SetAgent(Agent agent)
    {
        Agent = agent;
    }
}