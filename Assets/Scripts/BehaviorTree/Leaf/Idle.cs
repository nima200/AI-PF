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
            FinishedIdle = true;
        }
        else
        {
            Print("Skipped idle");
            FinishedIdle = true;
        }
    }

    public override BehaviorResult Process()
    {
        switch (FinishedIdle)
        {
            case true:
                switch (Agent.ReachedTarget)
                {
                    case true:
                        Result = BehaviorResult.SUCCESS;
                        return Result;
                    default:
                        Result = BehaviorResult.RUNNING;
                        return Result;
                }
            default:
                Result = BehaviorResult.RUNNING;
                return Result;
        }
//        Result = !FinishedIdle ? BehaviorResult.RUNNING : BehaviorResult.SUCCESS;
//        return Result;
    }

    public override void Reset()
    {
        base.Reset();
        FinishedIdle = false;
    }
}