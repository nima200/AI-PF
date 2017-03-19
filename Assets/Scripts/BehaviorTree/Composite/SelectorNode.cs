using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class SelectorNode : BehaviorNode
{
    protected BehaviorNode[] BehaviorNodes;

    public SelectorNode(params BehaviorNode[] behaviorNodes)
    {
        BehaviorNodes = behaviorNodes;
    }

    public override BehaviorResult Tick()
    {
        foreach (var node in BehaviorNodes)
        {
            try
            {
                switch (node.Tick())
                {
                    // If one failure is seen, keep going
                    case BehaviorResult.Failure:
                        continue;
                    // If any success is seen, return success
                    case BehaviorResult.Success:
                        Result = BehaviorResult.Success;
                        return Result;
                    // If any child is still running, keep this running
                    case BehaviorResult.Running:
                        Result = BehaviorResult.Running;
                        return Result;
                    default:
                        continue;
                }
            }
            catch (Exception exception)
            {
                Print(exception.ToString());
            }
        }
        // If this line is reached, it means everything returned a failure, and hence return failure
        Result = BehaviorResult.Failure;
        return Result;
    }
}