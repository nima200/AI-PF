using System;

public abstract class LeafNode : BehaviorNode
{
    public override void Initialize()
    {
        Print("Leaf node does not have an initializer");
    }

    public override void Reset()
    {
        Print("Leaf node does not have a reset");
    }
}
