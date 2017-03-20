public class SequenceNode : CompositeNode
{
    /// <summary>
    /// Attempts to proccess every child. If any fails, this fails. If all succeed it succeeds.
    /// If end is reached but a child was running in the check, then it returns running.
    /// </summary>
    /// <returns>The behavior result</returns>
    public override BehaviorResult Process()
    {
        bool childRunning = false;
        foreach (var node in ChildrenNodes)
        {
            switch (node.Process())
            {
                case BehaviorResult.FAIL:
                    Print("A child failed. FAIL.");
                    Result = BehaviorResult.FAIL;
                    return Result;
                case BehaviorResult.SUCCESS:
                    continue;
                case BehaviorResult.RUNNING:
                    Print("A child is still running");
                    childRunning = true;
                    continue;
                default:
                    Result = BehaviorResult.SUCCESS;
                    return Result;
            }
        }
        Result = childRunning ? BehaviorResult.RUNNING : BehaviorResult.SUCCESS;
        Print(childRunning ? "A child is running, keep running" : "All children succeeded. SUCCESS.");
        return Result;
    }
}