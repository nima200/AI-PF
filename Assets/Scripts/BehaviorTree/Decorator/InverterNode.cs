using System;

public class InverterNode : DecoratorNode
{
    public InverterNode(BehaviorNode childNode) : base(childNode) { }

    public override BehaviorResult Process()
    {
        // ReSharper disable once SwitchStatementMissingSomeCases
        // Default behavior comes from child's default
        switch (ChildNode.Process())
        {
            case BehaviorResult.FAIL:
                Print("Child failed. SUCCESS.");
                Result = BehaviorResult.SUCCESS;
                return Result;
            case BehaviorResult.SUCCESS:
                Print("Child succeeded. FAIL.");
                Result = BehaviorResult.FAIL;
                return Result;
            case BehaviorResult.RUNNING:
                Print("Child running. RUNNING.");
                Result = BehaviorResult.RUNNING;
                return Result;
        }
        Result = BehaviorResult.SUCCESS;
        return Result;
    }
}
