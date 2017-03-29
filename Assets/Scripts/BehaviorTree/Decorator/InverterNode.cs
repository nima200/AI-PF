public class InverterNode : DecoratorNode
{
    public InverterNode(BehaviorNode childNode) : base(childNode) { }
    /// <summary>
    /// Basic inverting process method that returns success on fail and fail on success.
    /// </summary>
    /// <returns>The rest of inversion.</returns>
    public override BehaviorResult Process()
    {
        // Default behavior comes from child's default
        switch (ChildNode.Process())
        {
            case BehaviorResult.FAIL:
                Result = BehaviorResult.SUCCESS;
                return Result;
            case BehaviorResult.SUCCESS:
                Result = BehaviorResult.FAIL;
                return Result;
            case BehaviorResult.RUNNING:
                Result = BehaviorResult.RUNNING;
                return Result;
        }
        Result = BehaviorResult.SUCCESS;
        return Result;
    }
}
