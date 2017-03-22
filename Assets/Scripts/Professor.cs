using System.Collections;
using UnityEngine;

public class Professor : MonoBehaviour
{
    public string Name;
    public Plaque MyPlaque;
    private void Start ()
    {
        Name = gameObject.name;
    }

    public IEnumerator ReceiveAdvice(Student s)
    {
        yield return new WaitForSecondsRealtime(2f);
        s.PreviousProfessors.Enqueue(s.NextProfessor);
        s.NextProfessor = GetNextProf(s);
        s.MyState = StudentState.SearchingForProf;
    }

    public Professor GetNextProf(Student s)
    {
        bool gotProf = false;
        int myIndex = s.AllProfessors.IndexOf(this);
        int nextProfIndex = 0;
        while (gotProf == false)
        {
            nextProfIndex = Random.Range(0, s.AllProfessors.Count);
            if (nextProfIndex == myIndex)
            {
                continue;
            }
            gotProf = true;
        }
        return s.AllProfessors[nextProfIndex];
    }
}
