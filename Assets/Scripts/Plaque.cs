using System.Collections;
using System.Collections.Generic;
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
