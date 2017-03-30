using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Agent : MonoBehaviour
{
    [HideInInspector]
    public Vector3[] Path;
    public Transform Target;
    public Professor TargetProfessor;
    [HideInInspector]
    public Professor PreviousProfessor;
    public Vector3 CurrentWaypoint;
    [Range(1, 30)]
    public float Speed = 5;
    public int WaypointIndex;
    private Grid _grid;
    public List<Plaque> Plaques;
    public List<Professor> Memory = new List<Professor>();
    public List<Professor> Professors;
    public bool ReachedTarget;
    public float DistanceToTarget;
    public int PreviousDistance;
    public Stopwatch Timer = new Stopwatch();

    private void Awake()
    {
        _grid = GameObject.Find("A*").GetComponent<Grid>();
        Professors = new List<Professor>(Plaques.Count);
        foreach (var p in Plaques)
        {
            Professors.Add(p.Professor);
        }
        TargetProfessor = Professors[Random.Range(0, Professors.Count)];
    }

    private void ResetPath()
    {
        _grid.ResetGridReservations(this);
        ReachedTarget = false;
        WaypointIndex = 0;
    }
    /// <summary>
    /// Method that moves the target towards its target. It is called automatically 
    /// by the path request manager once the path has been found for the request.
    /// </summary>
    /// <param name="newPath">The path</param>
    /// <param name="pathFound">The result of calculating a path</param>
    public void OnPathFound(Vector3[] newPath, bool pathFound)
    {
        if (!pathFound)
        {
            ResetPath();
            return;
        }
        Path = newPath;
        StopCoroutine("FollowPath");
        StartCoroutine("FollowPath");
    }

    public void StopPath()
    {
        StopCoroutine("FollowPath");
    }
    /// <summary>
    /// Moves the agent towards the waypoint it's supposed to be processing in the path.
    /// The waypoint starts from 0, and goes on proceeding through the path until the waypointindex
    /// has reached the length of the path, meaning the agent has reached the last waypoint in the path.
    /// Attempts to stop the path and recalculate another A* once the waypoint index has reached the 
    /// cap of the reservation window. In this way, we can limit our reservation window to whatever we want without
    /// worrying about the depth of the tree cause it will get replaced once the request to re-pathfind is sent.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FollowPath()
    {
        if (Path.Length > 0)
        {
            CurrentWaypoint = Path[0];
            if (Timer.IsRunning && Vector3.Distance(CurrentWaypoint, Target.position) < DistanceToTarget)
            {
                Timer.Stop();
                if (Timer.ElapsedMilliseconds > 100 && Timer.ElapsedMilliseconds < 3000)
                {
                    GetComponentInChildren<Renderer>().material.color = Color.red;
                }
                else if (Timer.ElapsedMilliseconds > 3000)
                {
                    GetComponentInChildren<Renderer>().material.color = Color.black;
                }
            }
            else
            {
                Timer.Reset();
                GetComponentInChildren<Renderer>().material.color = Color.green;
            }
            if (Vector3.Distance(CurrentWaypoint, Target.position) < DistanceToTarget)
            {
                if (!Timer.IsRunning)
                    Timer.Start();
            }
            while (true)
            {
                // Recalculate A* at end of reservation window
                if (Path.Length > _grid.TimeStepWindow && WaypointIndex == _grid.TimeStepWindow)
                {
                    _grid.ResetGridReservations(this);
                    ResetPath();
                    PathRequestManager.RequestPath(transform.position, Target.position, OnPathFound, this);
                    yield break;
                }
                if (transform.position == CurrentWaypoint)
                {
                    WaypointIndex++;
                    // agent has reached target
                    if (WaypointIndex >= Path.Length)
                    {
                        ReachedTarget = true;
                        yield break;
                    }
                    CurrentWaypoint = Path[WaypointIndex];
                }
                transform.position = Vector3.MoveTowards(transform.position, CurrentWaypoint, Speed * Time.deltaTime);
                DistanceToTarget = Vector3.Distance(transform.position, Target.position);
                yield return null;
            }
        }
    }
    /// <summary>
    /// Function used by other classes to make the agent request a path from the path request manager.
    /// </summary>
    /// <param name="target">The transform of the location to send the agent to</param>
    public void RequestPath(Transform target)
    {
        Target = target;
        StopPath();
        ResetPath();
        PathRequestManager.RequestPath(transform.position, Target.position, OnPathFound, this);
    }

    /// <summary>
    /// Method that is used by the GetAdvice leaf to make the agent get another random professor as the new target.
    /// It makes sure that the previous and current random professor are not the same.
    /// </summary>
    public void GetRandomProf()
    {
        PreviousProfessor = TargetProfessor;
        do
        {
            int random = Random.Range(0, Professors.Count);
            TargetProfessor = Professors[random];
        } while (TargetProfessor == PreviousProfessor);
    }

    public void OnDrawGizmos()
    {
        if (Path == null) return;
        for (int i = WaypointIndex; i < Path.Length; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawCube(Path[i], Vector3.one * 2);
        }
    }
}
