using System.Collections.Generic;

public class RandomProfSelector : RandomSelector
{
    public List<Professor> Professors;
    public List<Plaque> ProfessorPlaques;
    public RandomProfSelector(List<Professor> professors, List<Plaque> profPlaques, Agent agent)
    {
        Professors = professors;
        ProfessorPlaques = profPlaques;
        Agent = agent;
    }

    public override void Initialize()
    {
        for (int i = 0; i < Professors.Count; i++)
        {
            ChildrenNodes[i].SetProf(Professors[i]);
            ChildrenNodes[i].SetProfPlaque(ProfessorPlaques[i]);
            ChildrenNodes[i].SetAgent(Agent);
            ChildrenNodes[i].Initialize();
        }
    }
}