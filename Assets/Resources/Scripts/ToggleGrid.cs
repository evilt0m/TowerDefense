using UnityEngine;
using System.Collections;

public class ToggleGrid : MonoBehaviour {

	private bool gridON = false;
	private Projector proj;
	// Use this for initialization
	void Start () {
		proj = transform.GetComponent<Projector>();
		proj.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.G))
		{
			if (gridON)
			{
				gridON = false;
				proj.enabled = false;
			}
			else
			{
				gridON = true;
				proj.enabled = true;
			}
		}
	}
}
