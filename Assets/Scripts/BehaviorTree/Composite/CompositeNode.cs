using System.Collections.Generic;

public abstract class CompositeNode : BehaviorNode
{
    protected List<BehaviorNode> ChildrenNodes;

    protected CompositeNode()
    {
        ChildrenNodes = new List<BehaviorNode>();
    }

    public void Add(BehaviorNode child)
    {
        ChildrenNodes.Add(child);
    }

    public override void Initialize()
    {
        foreach (var node in ChildrenNodes)
        {
            Print("Calling onto a child's initialize");
            node.Initialize();
        }
    }

    public override void Reset()
    {
        foreach (var node in ChildrenNodes)
        {
            node.Reset();
        }
    }
}