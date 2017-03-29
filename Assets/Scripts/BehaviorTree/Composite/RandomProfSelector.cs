using UnityEngine;
using System.Collections.Generic;

public class RandomProfSelector : RandomSelector
{
    public List<Professor> Professors;
    public List<Plaque> Plaques;
    public RandomProfSelector(List<Professor> professors, List<Plaque> plaques, Agent agent)
    {
        Professors = professors;
        Plaques = plaques;
        Agent = agent;
    }

    /// <summary>
    /// Sets the professor, plaque, and agent of all children node.
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();
        for (int i = 0; i < Professors.Count; i++)
        {
            ChildrenNodes[i].SetProf(Professors[i]);
            ChildrenNodes[i].SetPlaque(Plaques[i]);
            ChildrenNodes[i].SetAgent(Agent);
        }
    }
}