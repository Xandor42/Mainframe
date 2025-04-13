using UnityEngine;
using System.Collections;
using TMPro;

public class RunnerWinsCollision : MonoBehaviour {

	public TMP_Text you_win_label;


	GameObject[] nodes;
	//public string winMessage = "The Hacker Wins.";
	void OnTriggerEnter (Collider other)
	{

		nodes = GameObject.FindGameObjectsWithTag("Objective");
		if (nodes.Length == 0) 
		{
			you_win_label.text = "GAME OVER: HACKER WINS";
			Time.timeScale = 0;
			Cursor.visible = true;
			if(Input.GetButtonDown("Click") || Input.GetButtonDown("UseNode"))
			   Application.LoadLevel("MainMenu");
			Debug.Log ("A WINNER IS YOU");
		}
		else
			Debug.Log ("YOU STILL NEED " + nodes.Length + " NODES!");
	}

	void Update () {
		nodes = GameObject.FindGameObjectsWithTag("Objective");
		if (nodes.Length == 0 && you_win_label.text == "GAME OVER: HACKER WINS") 
		{
			Time.timeScale = 0;
			if(Input.GetButtonDown("Click") || Input.GetButtonDown("UseNode"))
				Application.LoadLevel("MainMenu");
				Time.timeScale = 1;
		}
	}
}
