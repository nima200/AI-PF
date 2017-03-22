using System.Collections;
using UnityEngine;

public class ActionManager : MonoBehaviour
{

    private static ActionManager _instance;

    public void Talk(GetAdvice advice)
    {
        print("Trying to talk to: " + advice.Professor);
        StartCoroutine(GetAdvice(advice));
    }


    public IEnumerator GetAdvice(GetAdvice advice)
    {
        yield return new WaitForSecondsRealtime(2f);
        advice.FinishedTalking = true;
        print("Got advice from: " + advice.Professor);
    }

    public void Read(ReadPlaque plaque)
    {
        print("Reading: " + plaque.Professor + "'s plaque");
        plaque.AdjacentToPlaque = true;
        StartCoroutine(ReadPlaque(plaque));
    }

    public IEnumerator ReadPlaque(ReadPlaque read)
    {
        yield return new WaitForSecondsRealtime(1f);
        read.DoneReading = true;
        print("Done reading " + read.Professor + "'s plaque");
    }

    public void FindProfessor(FindProf findProf)
    {
        print("Searching for professor: " + findProf.Professor);
        StartCoroutine(FindPath(findProf));
    }

    public IEnumerator FindPath(FindProf findProf)
    {
        yield return new WaitForSecondsRealtime(4f);
        findProf.FoundProfessorPlaque = true;
        print("Found professor: " + findProf.Professor);
    }

    public void GoIdle(Idle idle)
    {
        StartCoroutine(Idle(idle));
    }

    public IEnumerator Idle(Idle idle)
    {
        yield return new WaitForSecondsRealtime(5f);
        idle.FinishedIdle = true;
        print("Done with idle");
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public static ActionManager GetInstance()
    {
        return _instance;
    }
}
