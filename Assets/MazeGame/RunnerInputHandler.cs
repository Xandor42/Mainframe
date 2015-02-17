using UnityEngine;
using System.Collections;

public class RunnerInputHandler : MonoBehaviour 
{
	public GameObject replacementObject;
	public GUIText element1;
	public GUIText element2;
	public GUIText element3;
	public GUIText element4;
	public GUIText element5;
	public float traceTime;
	public Texture2D emptyHackingBar;
	public Texture2D fullHackingBar;
	public AudioClip sound1;
	public AudioClip sound2;
	public AudioClip sound3;
	

	private GUIText remainingObjectives;
	private GUIText remainingTime;
	private GUIText systemWins;
	private GUIText hackingMsg;
	private GUIText completeMsg;
	private bool haxxor = false;
	private bool timerActive;
	private float hackProgress = 0.0F;
	private float displayTime = 1.5F;
	private int randomSound;

	private GameObject nodeBeingHaxxed;

	static System.Random _random = new System.Random();//for random key click sound selection



	GameObject[] nodes;
	

	void OnGUI()
	{
		if (haxxor == true) 
		{
			Debug.Log(hackProgress);
			GUI.DrawTexture (new Rect(Screen.width * 0.5F - 250, Screen.height * 0.5F, 500, 50), emptyHackingBar, ScaleMode.StretchToFill, true, 0.0f);
			GUI.DrawTexture (new Rect(Screen.width * 0.5F - 250, Screen.height * 0.5F, 500*hackProgress, 50), fullHackingBar, ScaleMode.ScaleAndCrop, true, 0.0f);
		} 
	}

	void Start()
	{
		Screen.showCursor = false;
		nodes = GameObject.FindGameObjectsWithTag("Objective");
		remainingObjectives = Instantiate(element1) as GUIText;
		remainingObjectives.text = "Remaining Firewalls: " + nodes.Length;

		remainingTime = Instantiate (element2) as GUIText;
		remainingTime.text = "Hack Traced in " + traceTime + " Seconds";

		hackingMsg = Instantiate(element4) as GUIText;
		hackingMsg.text = "";

		completeMsg = Instantiate (element5) as GUIText;
		completeMsg.text = "";

	}

	void Update () 
	{
		if (timerActive == true) 
		{
			displayTime -= Time.deltaTime;
			if (displayTime <= 0)
				completeMsg.text="";
		}
		Vector3 fwd = this.gameObject.transform.TransformDirection(Vector3.forward);
		RaycastHit strike;
		nodes = GameObject.FindGameObjectsWithTag("Objective");
		if (nodes.Length == 0) 
		{
			remainingObjectives.text = "All firewalls down, escape to the exit!";
		}
		else
			remainingObjectives.text = "Remaining Firewalls: " + nodes.Length;

		traceTime -= Time.deltaTime;
		remainingTime.text = "Hack Traced in " + traceTime + " Seconds";

		if (traceTime < 0) 
		{
			systemWins = Instantiate (element3) as GUIText;
			systemWins.text = "GAME OVER: SYSTEM WINS";
			Time.timeScale = 0;
			Screen.showCursor = true;
			if(Input.GetButtonDown("Click") || Input.GetButtonDown("UseNode"))
				Application.LoadLevel("MainMenu");


		}


		if (haxxor == true) 
		{
			this.gameObject.GetComponent<FPSInputController>().enabled = false;
			this.gameObject.GetComponent<CharacterMotor>().enabled = false;
			this.gameObject.rigidbody.velocity=Vector3.zero;
			this.gameObject.rigidbody.angularVelocity=Vector3.zero;
			if (Input.anyKeyDown){
				hackProgress += 0.05F;
				randomSound = _random.Next(100)+1;
				if(randomSound < 33)
					this.gameObject.GetComponent<AudioSource>().PlayOneShot(sound1);
				if(randomSound >= 33 && randomSound < 66)
					this.gameObject.GetComponent<AudioSource>().PlayOneShot(sound2);
				if(randomSound >= 66)
					this.gameObject.GetComponent<AudioSource>().PlayOneShot(sound3);
			}
		
			if (hackProgress >= 1.0F)		
			{
				hackProgress = 0.0F;
				haxxor = false;
				hackingMsg.text = "";



				if(nodeBeingHaxxed != null)
				{
					Transform nodeTransform = nodeBeingHaxxed.transform;
					Destroy(nodeBeingHaxxed);
					GameObject wall = null;
					wall = Instantiate(replacementObject) as GameObject;
					
					if(wall != null)
					{
						wall.transform.position = nodeTransform.position;
						
					}
				}
				timerActive= true;
				displayTime = 1.5F;
				completeMsg.text = "Firewall Hacked!";

				this.gameObject.GetComponent<CharacterMotor>().enabled = true;
				this.gameObject.GetComponent<FPSInputController>().enabled = true;

			}

		}



		if (Input.GetButtonDown ("UseNode")) 
		{
			Debug.Log("used a thing!");
			if(Physics.Raycast(transform.position, fwd, out strike, 0.75F))
			{
				Debug.Log("Hit a thing!");
				if(strike.collider.tag == "Objective")
				{
					haxxor = true;
					nodeBeingHaxxed = strike.collider.gameObject;
					hackingMsg.text = "Hack in Progress:";

				}

			}
		}




	}

}
