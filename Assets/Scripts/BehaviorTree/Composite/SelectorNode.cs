public class SelectorNode : CompositeNode
{
    /// <summary>
    /// Attempts to process every child.
    /// </summary>
    /// <returns>If one succeeds, then this succeeds.
    /// While any child is running, it returns running.
    /// If a child fails, it continues.
    /// If all children fail, it fails.</returns>
    public override BehaviorResult Process()
    {
        foreach (var node in ChildrenNodes)
        {
            switch (node.Process())
            {
                case BehaviorResult.FAIL:
                    continue;
                case BehaviorResult.SUCCESS:
                    Print("A child succeeded. SUCCESS.");
                    Result = BehaviorResult.SUCCESS;
                    return Result;
                case BehaviorResult.RUNNING:
                    Print("A child is running. Cannot decide yet");
                    Result = BehaviorResult.RUNNING;
                    return Result;
                default:
                    continue;
            }
        }
        Print("All children failed. FAIL.");
        Result = BehaviorResult.FAIL;
        return Result;
    }
}