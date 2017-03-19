using System;

public class SequenceNode : BehaviorNode
{
    protected BehaviorNode[] BehaviorNodes;

    public SequenceNode(params BehaviorNode[] behaviorNodes)
    {
        BehaviorNodes = behaviorNodes;
    }

    public override BehaviorResult Tick()
    {
        bool runningChild = false;
        foreach (var node in BehaviorNodes)
        {
            try
            {
                switch (node.Tick())
                {
                    // If one child failed, return failure
                    case BehaviorResult.Failure:
                        Result = BehaviorResult.Failure;
                        return Result;
                    // If checked child succeeded try to keep going
                    case BehaviorResult.Success:
                        continue;
                    // If child is running, take account of it and keep going. We'll come back to this later.
                    case BehaviorResult.Running:
                        runningChild = true;
                        continue;
                    default:
                        Result = BehaviorResult.Success;
                        return Result;
                }
            }
            catch (Exception exception)
            {
                Print(exception.ToString());
                Result = BehaviorResult.Failure;
                return Result;
            }
        }
        // If there was a running child, return running, else all succeeded so return true
        Result = runningChild ? BehaviorResult.Running : BehaviorResult.Success;
        return Result;
    }
}