using System;
using System.Collections.Generic;

public class RandomProfSelector : RandomSelector
{
    public List<string> ProfessorNames;
    public RandomProfSelector(List<string> professorNames)
    {
        ProfessorNames = professorNames;
    }

    public override void Initialize()
    {
        for (int i = 0; i < ProfessorNames.Count; i++)
        {
            ChildrenNodes[i].SetProf(ProfessorNames[i]);
            ChildrenNodes[i].Initialize();
        }
    }
}