using System.Collections;
using System.Collections.Generic;
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
