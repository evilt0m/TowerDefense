using UnityEngine;
using System.Collections;

public class SpawnStuff : MonoBehaviour {

	public GameObject spawnObject;
	public float spawnInterval = 0.5f;
	public int spawnAmount = 1;
	public float spreadX = 1;
	public float spreadY = 1;
	public float spreadZ = 1;
	public bool randomRotation = false;

	private int objCount;
	private float spawnTime;

	// Use this for initialization
	void Awake () {
		objCount = 0;
		spawnTime = Time.time + spawnTime;
		if (spawnObject == null)
		{
			Debug.Log("No spawn object assigned for: "+gameObject.name);
			Destroy(gameObject);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Time.time > spawnTime)
		{
			if (objCount > spawnAmount)
				return;

			spawnTime += spawnInterval;
			Vector3 ownPos = transform.position;
			Vector3 spawnPos = new Vector3(ownPos.x + Random.Range(spreadX * -10, spreadX * 10) / 10, ownPos.y, ownPos.z  + Random.Range(spreadZ * -10, spreadZ * 10) / 10);
			objCount++;
			if (randomRotation)
			{
				Instantiate(spawnObject, spawnPos, Random.rotation);
			}
			else
			{
				Instantiate(spawnObject, spawnPos, transform.rotation);
			}
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			Instantiate(spawnObject, transform.position, transform.rotation);
		}
	}
}
