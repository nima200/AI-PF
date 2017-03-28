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
            node.Initialize();
        }
    }

    public override void Reset()
    {
        Initialized = false;
        foreach (var node in ChildrenNodes)
        {
            node.Reset();
        }
    }

    public override void SetAgent(Agent agent)
    {
        foreach (var node in ChildrenNodes)
        {
            node.SetAgent(Agent);
        }
    }
}