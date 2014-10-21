using UnityEngine;
using System.Collections;

public class EnemyWIP : MonoBehaviour {

	public float speed;

	private int step;
	// Use this for initialization
	void Start () {
		step = 4;
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
	}


}
