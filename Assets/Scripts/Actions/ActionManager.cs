using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{

    private static ActionManager _instance;

    public void Talk(GetAdvice advice)
    {
        print("Trying to talk");
        StartCoroutine(GetAdvice(advice));
    }

    public IEnumerator GetAdvice(GetAdvice advice)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        advice.FinishedTalking = true;
        print("Got advice");
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
