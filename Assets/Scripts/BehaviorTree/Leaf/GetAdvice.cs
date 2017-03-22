using System;
using System.Collections;
using UnityEngine;

public class GetAdvice : LeafNode
{
    public string Professor;
    public bool FinishedTalking { get; set; }
    public bool Initialized { get; private set; }

    public override void SetProf(string professorName)
    {
        Professor = professorName;
    }

    public override void Initialize()
    {
        Print("Get advice Initialized");
        Initialized = true;
        ActionManager.GetInstance().Talk(this);
    }

    public override void Reset()
    {
        Print("Get advice Reseted");
    }

    public override BehaviorResult Process()
    {
        Print("Get advice node processing");
        return !FinishedTalking ? BehaviorResult.RUNNING : BehaviorResult.SUCCESS;
    }
}