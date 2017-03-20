using System.Collections;
using UnityEngine;

public class GetAdvice : LeafNode
{
    public bool FinishedTalking { get; set; }
    public override void Initialize()
    {
        Print("Initialized");
        ActionManager.GetInstance().Talk(this);
    }

    public override void Reset()
    {
        Print("Reseted");
    }

    public override BehaviorResult Process()
    {
        Print("Get advice node processing");
        return !FinishedTalking ? BehaviorResult.RUNNING : BehaviorResult.SUCCESS;
    }
}