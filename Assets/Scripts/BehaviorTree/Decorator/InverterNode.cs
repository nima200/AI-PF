using System;

public class InverterNode : BehaviorNode
{
    protected BehaviorNode Behavior;

    public InverterNode(BehaviorNode behavior)
    {
        Behavior = behavior;
    }

    public override BehaviorResult Tick()
    {
        try
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            // Do not have a default behavior for an inverter. It comes from the child.
            switch (Behavior.Tick())
            {
                case BehaviorResult.Failure:
                    Result = BehaviorResult.Success;
                    return Result;
                case BehaviorResult.Success:
                    Result = BehaviorResult.Failure;
                    return Result;
                case BehaviorResult.Running:
                    Result = BehaviorResult.Running;
                    return Result;
            }
        }
        catch (Exception exception)
        {
            Print(exception.ToString());
            Result = BehaviorResult.Success;
            return Result;
        }
        Result = BehaviorResult.Success;
        return Result;
    }
}
