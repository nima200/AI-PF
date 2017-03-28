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

    public override void SetProf(Professor professor)
    {
        Professor = professor;
        foreach (var node in ChildrenNodes)
        {
            node.SetProf(professor);
        }
    }

    public override void SetPlaque(Plaque plaque)
    {
        Plaque = plaque;
        foreach (var node in ChildrenNodes)
        {
            node.SetPlaque(plaque);
        }
    }
}