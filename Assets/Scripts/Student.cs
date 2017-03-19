using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
            // If state is set to idle then we can hangout for a few seconds 
            if (value == StudentState.Idle)
            {
                _myState = value;
//                print("Going to idle");
                StartCoroutine(Chill());
            }
            else
            {
                /* If not, half the time, we check if previous state was not idle (so we don't go idle -> idle)
                 * Then the state goes to 
                 */

                int probability = Random.Range(0, 10);
                if (probability < 5 && _myState != StudentState.Idle)
                {
                    MyState = StudentState.Idle;
                }
                else if (value == StudentState.SearchingForProf)
                {
//                    print("Going to " + value);
                    _myState = value;
                    // keep going
                    StartCoroutine(NextProfessor.ReceiveAdvice(this));
                }
                else
                {
                    _myState = value;
                }
            }
        }
    }

    private void Start()
    {
        PreviousProfessors.Limit = 4;
        AllProfessors = GameObject.Find("Professors").GetComponentsInChildren<Professor>().ToList();

        int firstProfessorIndex = Random.Range(0, AllProfessors.Count);
        NextProfessor = AllProfessors[firstProfessorIndex];
        StartCoroutine(NextProfessor.ReceiveAdvice(this));
    }

    public IEnumerator Chill()
    {
        yield return new WaitForSecondsRealtime(3f);
        // go hangout in main area
        MyState = StudentState.SearchingForProf;
    }
}
