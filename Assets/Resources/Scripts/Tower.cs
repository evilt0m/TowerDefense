using UnityEngine;
using System.Collections;

public enum towerTypes{
	rocket, machinegun, cannon
}

public class Tower : MonoBehaviour {

	public GameObject towerMesh;
	public towerTypes towerType;
	public Transform[] emitter;

	public int range = 10;
	public int ammo = 1;
	public float cadence = 1f;
	public int reloadTime = 5;
	public float damagePerAmmo = 10f;
	public int turnSpeed = 10;
	public float nextAction;


	private bool activated = true;
	private bool hasTarget;
	private Transform target;
	private GameObject rocket;


	// Use this for initialization
	void Awake () {
		nextAction = Time.time + reloadTime;
		rocket = Resources.Load("Prefabs/Rocket") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		//if the tower is deactivated (while beeing placed e.g.) do nothing
		if (!activated)
			return;



		//no target? do nothing
		target = FindTarget();
		if (target == null)
			return;

		//rotate turret
		Vector3 lookAtPos = target.position;
		lookAtPos.y = towerMesh.transform.position.y;
		Quaternion newRotation = Quaternion.LookRotation(lookAtPos - towerMesh.transform.position, Vector3.up);
		towerMesh.transform.rotation = Quaternion.Slerp(towerMesh.transform.rotation, newRotation, Time.deltaTime * turnSpeed);

		Debug.DrawLine(transform.position + Vector3.up * 2, target.position, Color.cyan);

		//currently reloading or whatever
		if (Time.time < nextAction)
			return;

		if (towerType == towerTypes.rocket)
		{
			GameObject newRocket = Instantiate(rocket, emitter[0].position, emitter[0].rotation) as GameObject;
			newRocket.GetComponent<Rocket>().SetTarget(target);

		}

		nextAction = Time.time + reloadTime;


	}

	Transform FindTarget()
	{
		Vector3 towerPos = transform.position;
		float nearestDistance = Mathf.Infinity;
		Transform nearestTarget = null;

		//find all enemies
		GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag("Enemy");

		foreach (GameObject targetToCheck in possibleTargets)
		{
			//get the nearest enemy
			float distance = Vector3.Distance(towerPos, targetToCheck.transform.position);
			if (distance < nearestDistance && distance < range)
			{
				nearestTarget = targetToCheck.transform;
				nearestDistance = distance;
			}
		}

		if (nearestTarget)
		{
			return nearestTarget;
		}
		else
		{
			return null;
		}
	}
	

	public void ActivateTower(bool param)
	{
		activated = param;
	}
}
