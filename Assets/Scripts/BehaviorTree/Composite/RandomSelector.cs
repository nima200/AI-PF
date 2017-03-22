using Random = UnityEngine.Random;

public class RandomSelector : CompositeNode
{
    private BehaviorNode _previousRandom;
    /// <summary>
    /// Randomly selects a child among the children nodes and proccesses it.
    /// Makes sure the selected child is not the same as the previous child.
    /// On Process() does not get a new random child unless the previous random child
    /// is done (either fail or success).
    /// </summary>
    /// <returns>The result of the random child chosen.</returns>
    public override BehaviorResult Process()
    {
        if (_previousRandom == null || _previousRandom.Process() == BehaviorResult.FAIL || _previousRandom.Process() == BehaviorResult.SUCCESS) 
        {
            var randomChild = GetRandomChild();
            switch (randomChild.Process())
            {
                case BehaviorResult.FAIL:
                    Print("Random child failed");
                    Result = BehaviorResult.FAIL;
                    return Result;
                case BehaviorResult.SUCCESS:
                    Print("Random child succeeded");
                    Result = BehaviorResult.SUCCESS;
                    return Result;
                case BehaviorResult.RUNNING:
                    Print("Random child running");
                    Result = BehaviorResult.RUNNING;
                    return Result;
                default:
                    Print("Default behavior: fail");
                    Result = BehaviorResult.FAIL;
                    return Result;
            }
        }
        // if random wasn't null, and wasn't done, then it was running.
        Result = BehaviorResult.RUNNING;
        return Result;
    }
    private BehaviorNode GetRandomChild()
    {
        while (true)
        {
            int randomChildIndex = Random.Range(0, ChildrenNodes.Count);
            if (ChildrenNodes[randomChildIndex] == _previousRandom) continue;
            _previousRandom = ChildrenNodes[randomChildIndex];
            return ChildrenNodes[randomChildIndex];
        }
    }
}