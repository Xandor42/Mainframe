using UnityEngine;
using System.Collections;
using TMPro;

public class RunnerInputHandler : MonoBehaviour 
{
	public GameObject replacementObject;
	public TMP_Text remaining_objectives_label;
	public TMP_Text hack_traced_label;
	public TMP_Text game_over_label;
	public TMP_Text hacking_message_label;
	public TMP_Text complete_label;
	public float traceTime;
	public Texture2D emptyHackingBar;
	public Texture2D fullHackingBar;
	public AudioClip sound1;
	public AudioClip sound2;
	public AudioClip sound3;
	
	private bool haxxor = false;
	private bool timerActive;
	private float hackProgress = 0.0F;
	private float displayTime = 1.5F;
	private int randomSound;

	private GameObject nodeBeingHaxxed;

	static System.Random _random = new System.Random(); //for random key click sound selection



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
		Cursor.visible = false;
		nodes = GameObject.FindGameObjectsWithTag("Objective");
		remaining_objectives_label.text = "Remaining Firewalls: " + nodes.Length;

		hack_traced_label.text = "Hack Traced in " + traceTime.ToString() + " Seconds";

		hacking_message_label.text = "";

		complete_label.text = "";

	}

	void Update () 
	{
		if (timerActive == true) 
		{
			displayTime -= Time.deltaTime;
			if (displayTime <= 0)
				complete_label.text="";
		}
		Vector3 fwd = this.gameObject.transform.TransformDirection(Vector3.forward);
		RaycastHit strike;
		nodes = GameObject.FindGameObjectsWithTag("Objective");
		if (nodes.Length == 0) 
		{
			remaining_objectives_label.text = "All firewalls down, escape to the exit!";
		}
		else
			remaining_objectives_label.text = "Remaining Firewalls: " + nodes.Length;

		traceTime -= Time.deltaTime;
		hack_traced_label.text = "Hack Traced in " + traceTime.ToString() + " Seconds";

		if (traceTime < 0) 
		{
			game_over_label.text = "GAME OVER: SYSTEM WINS";
			Time.timeScale = 0;
			Cursor.visible = true;
			if(Input.GetButtonDown("Click") || Input.GetButtonDown("UseNode"))
				Application.LoadLevel("MainMenu");
		}


		if (haxxor == true) 
		{
			this.gameObject.GetComponent<FPSInputController>().enabled = false;
			this.gameObject.GetComponent<CharacterMotor>().enabled = false;
			this.gameObject.GetComponent<Rigidbody>().linearVelocity=Vector3.zero;
			this.gameObject.GetComponent<Rigidbody>().angularVelocity=Vector3.zero;
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
				hacking_message_label.text = "";



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
				complete_label.text = "Firewall Hacked!";

				this.gameObject.GetComponent<CharacterMotor>().enabled = true;
				this.gameObject.GetComponent<FPSInputController>().enabled = true;

			}

		}



		if (Input.GetButtonDown ("UseNode")) 
		{
			if(Physics.Raycast(transform.position, fwd, out strike, 0.75F))
			{
				if(strike.collider.tag == "Objective")
				{
					haxxor = true;
					nodeBeingHaxxed = strike.collider.gameObject;
					hacking_message_label.text = "Hack in Progress:";

				}

			}
		}




	}

}
