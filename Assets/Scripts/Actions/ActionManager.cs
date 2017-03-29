using System.Collections;
using UnityEngine;

public class ActionManager : MonoBehaviour
{

    private static ActionManager _instance;
    private Grid _grid;

    /// <summary>
    /// Method that makes an agent request a path-finding calculation from its current location to 
    /// the node's professor's location.
    /// </summary>
    /// <param name="advice">The leaf node calling this method.</param>
    public void Talk(GetAdvice advice)
    {
        advice.Agent.RequestPath(advice.Professor.transform);
        StartCoroutine(GetAdvice(advice));
    }

    /// <summary>
    /// Method that makes the agent wait for a brief second once it has reached
    /// the professor it is trying to get advice from, to simulate the effect of talking
    /// </summary>
    /// <param name="advice"></param>
    /// <returns>Delay</returns>
    public IEnumerator GetAdvice(GetAdvice advice)
    {
        while (!advice.Agent.ReachedTarget)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        advice.FinishedTalking = true;
        advice.Agent.GetRandomProf();
    }

    /// <summary>
    /// Method that calls a coroutine to delay the agent for a brief moment to simulate
    /// the effect of reading.
    /// </summary>
    /// <param name="plaque">The leaf node calling this method</param>
    public void Read(ReadPlaque plaque)
    {
        StartCoroutine(ReadPlaque(plaque));
    }
    /// <summary>
    /// The coroutine that actually delays the agent for a brief moment to simulate the
    /// effect of reading.
    /// </summary>
    /// <param name="read">The leaf node calling Read.</param>
    /// <returns></returns>
    public IEnumerator ReadPlaque(ReadPlaque read)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        read.DoneReading = true;
    }

    /// <summary>
    /// Method that attempts to send a path-finding request for the agent from its current
    /// location to the location of the node's prof's plaque's tranform.
    /// </summary>
    /// <param name="findProf">The leaf node calling this method.</param>
    public void FindProfessor(FindProf findProf)
    {
        findProf.Agent.RequestPath(findProf.Plaque.transform);
    }

    /// <summary>
    /// Method that attempts to send a path-finding request for the agent from its current
    /// location to a random location through the level's main area.
    /// </summary>
    /// <param name="idle">The leaf node calling this method.</param>
    public void GoIdle(Idle idle)
    {
        idle.Agent.RequestPath(_grid.IdleWaypoints[Random.Range(0, _grid.IdleWaypoints.Count)].transform);
    }

    public IEnumerator Idle(Idle idle)
    {
        yield return new WaitForSecondsRealtime(5f);
        idle.FinishedIdle = true;
        print("Done with idle");
    }

    /// <summary>
    /// Attempts to create a singleton since the agents try to instantiate this.
    /// </summary>
    private void Awake()
    {
        if (_instance == null)
        {
            _grid = GameObject.Find("A*").GetComponent<Grid>();
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    /// <summary>
    /// Singleton design.
    /// </summary>
    /// <returns>The instance of the singleton.</returns>
    public static ActionManager GetInstance()
    {
        return _instance;
    }
}
