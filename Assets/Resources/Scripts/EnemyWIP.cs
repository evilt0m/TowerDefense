using UnityEngine;
using System;

public class EnemyWIP : MonoBehaviour {

	public float speed;

	private int step;

	private GameObject ziel;
	private GameObject nextWaypoint;
	private int currentWaypointIndex;


	//step6
	private GameObject[] waypoints;

	// Use this for initialization
	void Start () {
		step = 12;

		//step 7
		ziel = GameObject.Find("Ziel");
		//step 8
		waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
		//step 9
		currentWaypointIndex = 0;
		nextWaypoint = waypoints[currentWaypointIndex];
		//step 10, nach namen sortieren
		Array.Sort(waypoints, delegate(GameObject x, GameObject y){return x.name.CompareTo(y.name);});
		nextWaypoint = waypoints[currentWaypointIndex];
	}
	
	// Update is called once per frame
	void Update () {

		if (step == 1)
		{
			//step 1: object bewegen
			Vector3 currentPosition = transform.position;
			transform.position = currentPosition + Vector3.forward;
		}
		else if (step == 2)
		{
			//step 2: intro time.deltatime (fps und so)
			Vector3 currentPosition = transform.position;
			transform.position = currentPosition + Vector3.forward * Time.deltaTime;
		}
		else if (step == 3)
		{
			//step 3: die richtung macht den unterschied, local und world space
			Vector3 currentPosition = transform.position;
			transform.position = currentPosition + transform.forward * Time.deltaTime;
		}
		else if (step == 4)
		{
			//step 4: speed. unsere erste public variable
			Vector3 currentPosition = transform.position;
			transform.position = currentPosition + transform.forward * Time.deltaTime * speed;
		}
		else if (step == 5)
		{
			//step 5: nutzen der unity eigenen methoden
			transform.Translate(Vector3.forward * Time.deltaTime * speed);
		}
		else if (step == 6)
		{
			//step 6: drehen
			transform.Rotate(Vector3.up * Time.deltaTime * 90);
			transform.Translate(Vector3.forward * Time.deltaTime * speed);
		}
		else if (step == 7)
		{
			//step 7: in richtung etwas gucken
			transform.LookAt(ziel.transform.position);
			transform.Translate(Vector3.forward * Time.deltaTime * speed);
		}
		else if (step == 8)
		{
			//step 8: waypoint anfahren

			transform.LookAt(ziel.transform.position);
			transform.Translate(Vector3.forward * Time.deltaTime * speed);
		}
		else if (step == 9)
		{
			//step 9: waypoints hintereinander anfahren
			transform.LookAt(nextWaypoint.transform.position);
			transform.Translate(Vector3.forward * Time.deltaTime * speed);

			//checken, ob wir waypoint erreicht haben
			float distanceToTarget = Vector3.Distance(transform.position, nextWaypoint.transform.position);
			if (distanceToTarget < 1f)
			{
				currentWaypointIndex++;
				nextWaypoint = waypoints[currentWaypointIndex];
				Debug.Log(nextWaypoint.name);
			}
		}
		else if (step == 11)
		{
			//step 11: waypoints hintereinander anfahren, wenn ende erreicht, neu anfangen
			transform.LookAt(nextWaypoint.transform.position);
			transform.Translate(Vector3.forward * Time.deltaTime * speed);
			
			//checken, ob wir waypoint erreicht haben
			float distanceToTarget = Vector3.Distance(transform.position, nextWaypoint.transform.position);
			if (distanceToTarget < 1f)
			{
				if (currentWaypointIndex < waypoints.Length - 1)
				{
					currentWaypointIndex++;
				}
				else
				{
					currentWaypointIndex = 0;
				}
				nextWaypoint = waypoints[currentWaypointIndex];
			}
		}
		else if (step == 12)
		{
			//step 12: schöner drehen
			Vector3 lookAtPos = nextWaypoint.transform.position;
			lookAtPos.y = transform.position.y;
			Quaternion newRotation = Quaternion.LookRotation(lookAtPos - transform.position, Vector3.up);
			transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 4f);


			transform.Translate(Vector3.forward * Time.deltaTime * speed);
			
			//checken, ob wir waypoint erreicht haben
			float distanceToTarget = Vector3.Distance(transform.position, nextWaypoint.transform.position);
			if (distanceToTarget < 1f)
			{
				if (currentWaypointIndex < waypoints.Length - 1)
				{
					currentWaypointIndex++;
				}
				else
				{
					currentWaypointIndex = 0;
				}
				nextWaypoint = waypoints[currentWaypointIndex];
			}
		}	
	}
}
