using System.Collections.Generic;

public class RandomProfSelector : RandomSelector
{
    public List<Professor> Professors;
    public List<Plaque> Plaques;
    public RandomProfSelector(List<Professor> professors, List<Plaque> plaques)
    {
        Professors = professors;
        Plaques = plaques;
    }

    public override void Initialize()
    {
        for (int i = 0; i < Professors.Count; i++)
        {
            ChildrenNodes[i].SetProf(Professors[i]);
            ChildrenNodes[i].SetPlaque(Plaques[i]);
            ChildrenNodes[i].Initialize();
        }
    }
}