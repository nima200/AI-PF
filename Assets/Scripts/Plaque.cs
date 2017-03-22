using UnityEngine;

public class Plaque : MonoBehaviour
{
    public string Name;
    public Professor MyProfessor;

	private void Start ()
	{
	    Name = MyProfessor.Name;
	}
}
