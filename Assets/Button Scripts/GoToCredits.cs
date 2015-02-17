using UnityEngine;
using System.Collections;

public class GoToCredits : MonoBehaviour {
	public void StartTheCredits() 
	{
		Application.LoadLevel("Credits");
	}
}
