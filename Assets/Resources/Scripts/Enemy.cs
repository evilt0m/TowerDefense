using UnityEngine;
using System;

public class Enemy : MonoBehaviour {


	public float speed = 10f;

	public float health;
	public float maxHealth;
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

	// Use this for initialization
	void Awake () {
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
		health -= Time.deltaTime * 10;

		if (health < 0)
		{
			Destroy(gameObject);
		}
		if (currentWaypoint >= waypointLength - 1)
		{
			//end of path reached
			//Destroy(gameObject);

			//debug
			currentWaypoint = -1;
		}

		Vector3 nextWaypoint = waypoints[currentWaypoint + 1].transform.position;

		Quaternion newRotation = Quaternion.LookRotation(nextWaypoint - transform.position, Vector3.up);
		transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10f);
		
		transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, speed * Time.deltaTime);
		//distance check
		float distanceToTarget = Vector3.Distance(transform.position, nextWaypoint);
		if (distanceToTarget < 0.1f)
		{
			currentWaypoint++;
		} 
	}

	void OnGUI()
	{
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
