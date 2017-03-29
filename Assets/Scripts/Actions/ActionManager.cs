using System.Collections;
using UnityEngine;

public class ActionManager : MonoBehaviour
{

    private static ActionManager _instance;

    public void Talk(GetAdvice advice)
    {
        advice.Agent.RequestPath(advice.Professor.transform);
        StartCoroutine(GetAdvice(advice));
    }


    public IEnumerator GetAdvice(GetAdvice advice)
    {
        while (!advice.Agent.ReachedTarget)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        advice.FinishedTalking = true;
        advice.Agent.GetRandomProf();
    }

    public void Read(ReadPlaque plaque)
    {
        StartCoroutine(ReadPlaque(plaque));
    }

    public IEnumerator ReadPlaque(ReadPlaque read)
    {
        yield return new WaitForSecondsRealtime(1f);
        read.DoneReading = true;
    }

    public void FindProfessor(FindProf findProf)
    {
        findProf.Agent.RequestPath(findProf.Plaque.transform);
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
