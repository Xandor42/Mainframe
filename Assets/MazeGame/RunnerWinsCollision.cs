using UnityEngine;
using System.Collections;

public class RunnerWinsCollision : MonoBehaviour {

//	void Start() 
//	{
//		Screen.SetResolution(3360, 900, false);
//	}
	public GUIText element1;


	private GUIText hackerWins;

	GameObject[] nodes;
	//public string winMessage = "The Hacker Wins.";
	void OnTriggerEnter (Collider other)
	{

		nodes = GameObject.FindGameObjectsWithTag("Objective");
		if (nodes.Length == 0) 
		{
			hackerWins = Instantiate (element1) as GUIText;
			hackerWins.text = "GAME OVER: HACKER WINS";
			Time.timeScale = 0;
			Screen.showCursor = true;
			if(Input.GetButtonDown("Click") || Input.GetButtonDown("UseNode"))
			   Application.LoadLevel("MainMenu");
			Debug.Log ("A WINNER IS YOU");
		}
		else
			Debug.Log ("YOU STILL NEED " + nodes.Length + " NODES!");
	}

//	void OnGUI()
//	{
//		if (_hackerWins = true) 
//		{
//			GUI.Label (new Rect (100, 100, 1000, 200), _winMessage);
//		}
//	}
}
