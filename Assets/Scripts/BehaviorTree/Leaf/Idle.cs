using UnityEngine;

public class Idle : LeafNode
{
    public bool FinishedIdle { get; set; }

    public override void Initialize()
    {
        Initialized = true;
        int random = Random.Range(0, 10);
        if (random < 5)
        {
            Print("Going into idle");
            ActionManager.GetInstance().GoIdle(this);
        }
        else
        {
            Print("Skipped idle");
            FinishedIdle = true;
        }
    }

    public override BehaviorResult Process()
    {
        Result = !FinishedIdle ? BehaviorResult.RUNNING : BehaviorResult.SUCCESS;
        return Result;
    }

    public override void Reset()
    {
        base.Reset();
        FinishedIdle = false;
    }
}