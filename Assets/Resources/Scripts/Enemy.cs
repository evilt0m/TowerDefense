using UnityEngine;
using System;

public class Enemy : MonoBehaviour {


	public float speed = 10f;
	public float traction = 2f;

	public float health;
	public float maxHealth;

	public GameObject tank;
	public GameObject turret;

	private float adjustment = 2.9f;
	private Vector3 worldPosition;
	private Vector3 screenPosition;
	private int healthBarHeight = 5;
	private int healthBarWidth = 50;
	private int barTop = 1;
	private Transform myTransform;
	private Camera myCam;
	private Texture barTexture;
	private Texture barBackTexture;

	private GameObject[] waypoints;
	private int currentWaypoint;
	private int waypointLength;
	private bool grounded;
	private bool isDead;

	private Material deadMaterial;

	// Use this for initialization
	void Awake () {
		deadMaterial = Resources.Load("Models/Materials/Dead") as Material;
		barTexture = Resources.Load("Textures/healthbar") as Texture;
		barBackTexture = Resources.Load("Textures/black") as Texture;
		myTransform = transform;
		myCam = Camera.main;
		waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
		Array.Sort(waypoints, delegate(GameObject x, GameObject y){return x.name.CompareTo(y.name);});

		//init stuff
		health = maxHealth;

	}

	void Start () {
		currentWaypoint = -1;
		waypointLength = waypoints.Length;
	}
	
	// Update is called once per frame
	void Update () {
		//hack
		//health -= Time.deltaTime * 10;

		if (health <= 0)
		{
			if (!isDead)
			{
				isDead = true;
				gameObject.tag = "Untagged";
				tank.renderer.material = deadMaterial;
				turret.renderer.material = deadMaterial;
				turret.transform.GetChild(0).renderer.material = deadMaterial;
			}
			return;
		}
		if (currentWaypoint >= waypointLength - 1)
		{
			//end of path reached

			//debug
			currentWaypoint = -1;
		}

		Vector3 nextWaypoint = waypoints[currentWaypoint + 1].transform.position;

		//rotate turret

		if (turret != null)
		{

		}
		
		//old non physics movement
		//transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, speed * Time.deltaTime);



		Vector3 normForward = transform.forward.normalized;
		Vector3 velocity = rigidbody.velocity;
		Vector3 pointToMirror = normForward * Vector3.Dot(normForward, velocity);
		Vector3 newDir = pointToMirror + pointToMirror - velocity;

		//check if grounded

		if (Physics.Raycast(myTransform.position + Vector3.up / 2, -transform.up, 0.8f))
		{
			grounded = true;
			Debug.DrawRay(myTransform.position, -transform.up, Color.red);
		}
		else
		{
			grounded = false;
		}

		//distance check
		float distanceToTarget = Vector3.Distance(transform.position, nextWaypoint);

		//only if on ground
		if (grounded)
		{

			Vector3 directionVector = newDir + transform.forward;

			//check if target is in front
			Vector3 target = nextWaypoint - transform.position;


			//rotate tank
			Vector3 lookAtPos = nextWaypoint;
			lookAtPos.y = transform.position.y;
			Quaternion newRotation = Quaternion.LookRotation(lookAtPos - transform.position, Vector3.up);
			transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 4f);


			//break if drifting too hard
			float angle = Vector3.Angle(normForward, target);
			Vector3 breakVector = -rigidbody.velocity.normalized * angle / 10f;

			//add speed if away from waypoint
			if (velocity.magnitude < speed && distanceToTarget >= 2.5f)
			{
				rigidbody.AddForce(normForward.normalized * speed, ForceMode.Acceleration);
			}


			Vector3 rightVel = transform.right * Vector3.Dot(rigidbody.velocity, transform.right);


			rigidbody.AddForce(-rightVel * traction, ForceMode.Force);

			//funny lines
			/*
			Debug.DrawRay(transform.position + Vector3.up, normForward.normalized * 4, Color.red);
			Debug.DrawRay(transform.position + Vector3.up, velocity.normalized * 4, Color.green);
			Debug.DrawRay(transform.position + Vector3.up, directionVector.normalized * 4, Color.yellow);
			Debug.DrawRay(transform.position + Vector3.up, breakVector, Color.blue);
			Debug.DrawRay(transform.position + Vector3.up, target, Color.magenta);
			Debug.DrawRay(transform.position + Vector3.up, rightVel, Color.white);
			*/
		}

		if (distanceToTarget < 2.5f)
		{
			currentWaypoint++;


		} 
	}
	/*
	void OnCollisionStay(Collision other)
	{
		if (other.gameObject.name == "Terrain")
		{
			grounded = true;
		}
	}
	void OnCollisionExit(Collision other)
	{
		if (other.gameObject.name == "Terrain")
		{
			grounded = false;
		}
	}
	*/
	public void AddDamage(float damage)
	{
		health -= damage;
	}

	void OnGUI()
	{
		if (health <= 0)
			return;
		worldPosition = new Vector3(myTransform.position.x, myTransform.position.y + adjustment, myTransform.position.z);
		screenPosition = myCam.WorldToScreenPoint(worldPosition);

		float left =  screenPosition.x - healthBarWidth / 2;
		float top = Screen.height - screenPosition.y - barTop;


		//GUI.color = Color.black;
		GUI.DrawTexture(new Rect (left-2, top-2, healthBarWidth+2, healthBarHeight+2), barBackTexture, ScaleMode.ScaleAndCrop);
		//displays a healthbar
		GUI.DrawTexture(new Rect (left, top, (health/maxHealth) * healthBarWidth, healthBarHeight), barTexture, ScaleMode.ScaleAndCrop);


	
	}
}
