using System.Collections.Generic;

public abstract class CompositeNode : BehaviorNode
{
    protected List<BehaviorNode> ChildrenNodes;
    public string Professor;
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

    public override void SetProf(string professorName)
    {
        foreach (var node in ChildrenNodes)
        {
            node.SetProf(professorName);
        }
    }
}