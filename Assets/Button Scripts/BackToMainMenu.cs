using UnityEngine;
using System.Collections;

public class BackToMainMenu : MonoBehaviour {
	public void BackToMenu() 
	{
		Application.LoadLevel("MainMenu");
	}
}
