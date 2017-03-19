using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Student : MonoBehaviour
{
    public List<Professor> AllProfessors { get; set; }
    public Professor NextProfessor { get; set; }
    public MyQueue<Professor> PreviousProfessors = new MyQueue<Professor>();
    [SerializeField]
    private StudentState _myState = StudentState.Idle;
    public StudentState MyState
    {
        get { return _myState; }
        set
        {
            switch (value)
            {
                case StudentState.Idle:
                    _myState = value;
                    StartCoroutine(Chill());
                    break;
                case StudentState.GettingAdvice:
                    _myState = value;
                    break;
                case StudentState.SearchingForProf:
                    int probability = Random.Range(0, 10);
                    if (probability < 5 && _myState != StudentState.Idle)
                    {
                        MyState = StudentState.Idle;
                    }
                    else
                    {
                        _myState = value;
                        StartCoroutine(NextProfessor.ReceiveAdvice(this));
                    }
                    break;
            }
        }
    }

    public IEnumerator Chill()
    {
        yield return new WaitForSecondsRealtime(3f);
        // go hangout in main area
        MyState = StudentState.SearchingForProf;
    }

    private void Start()
    {
        PreviousProfessors.Limit = 4;
        AllProfessors = GameObject.Find("Professors").GetComponentsInChildren<Professor>().ToList();

        int firstProfessorIndex = Random.Range(0, AllProfessors.Count);
        NextProfessor = AllProfessors[firstProfessorIndex];
        StartCoroutine(NextProfessor.ReceiveAdvice(this));
    }

    private void Update()
    {
        
    }

}
