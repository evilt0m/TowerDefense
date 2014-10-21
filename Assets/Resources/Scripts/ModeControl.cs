using UnityEngine;
using System.Collections;

public class ModeControl : MonoBehaviour {

	private bool buildMode = false;
	private GameObject testBuilding;
	private GameObject tutorial;

	// Use this for initialization
	void Start () {
		testBuilding = Resources.Load("Prefabs/TestBuilding") as GameObject;
		tutorial = GameObject.Find("PressA");
	}
	
	// Update is called once per frame
	void Update () {
		if (!buildMode)
		{
			if (Input.GetKeyDown(KeyCode.A))
			{
				tutorial.SetActive(false);
				buildMode = true;
				GameObject newBuilding = Instantiate(testBuilding) as GameObject;
				newBuilding.GetComponent<SnapToMouse>().SetPicked();
			}
		}

		if (Input.GetKeyDown(KeyCode.S))
		{

		}

	}

	public void SetBuildMode(bool param)
	{
		buildMode = param;
	}

	public bool GetBuildMode()
	{
		return buildMode;
	}
}
