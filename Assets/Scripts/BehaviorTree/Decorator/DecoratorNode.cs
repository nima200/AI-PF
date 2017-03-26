public abstract class DecoratorNode : BehaviorNode
{
    protected BehaviorNode ChildNode;

    protected DecoratorNode(BehaviorNode childNode)
    {
        ChildNode = childNode;
    }

    public override void Initialize()
    {
        Print("Calling onto child's initialize");
        ChildNode.Initialize();
    }

    public override void Reset()
    {
        ChildNode.Reset();
    }
}