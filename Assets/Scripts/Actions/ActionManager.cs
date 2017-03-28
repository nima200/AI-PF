using System.Collections;
using UnityEngine;

public class ActionManager : MonoBehaviour
{

    private static ActionManager _instance;

    public void Talk(GetAdvice advice)
    {
        print("Trying to talk to: " + advice.Professor.Name);
        advice.Agent.RequestPath(advice.Professor.gameObject.transform);
        StartCoroutine(GetAdvice(advice));
    }


    public IEnumerator GetAdvice(GetAdvice advice)
    {
        while (!advice.Agent.FinishedTalking)
        {
            if (advice.Agent.Target == advice.Agent.TargetProfessor.transform && advice.Agent.ReachedTarget)
            {
                yield return new WaitForSeconds(1f);
                advice.Agent.FinishedTalking = true;
                advice.Agent.ChangeTargetProfessor(
                    advice.Agent.GetComponent<Behavior>().Professors[
                        Random.Range(0, advice.Agent.GetComponent<Behavior>().Professors.Count)]);
                print("Got advice from: " + advice.Professor.Name);
            }
            yield return null;
        }
    }

    public void Read(ReadPlaque plaque)
    {
        print("Reading: " + plaque.Professor.Name + "'s plaque");
        plaque.AdjacentToPlaque = true;
        StartCoroutine(ReadPlaque(plaque));
    }

    public IEnumerator ReadPlaque(ReadPlaque read)
    {
        yield return new WaitForSecondsRealtime(1f);
        read.DoneReading = true;
        print("Done reading " + read.Professor.Name + "'s plaque");
    }

    public void FindProfessorPlaque(FindProf findProf)
    {
        print("Searching for professor: " + findProf.Professor.Name);
        findProf.Agent.RequestPath(findProf.ProfessorPlaque.gameObject.transform);
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
