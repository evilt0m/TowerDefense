using UnityEngine;
using System.Collections;

public class SnapToMouse : MonoBehaviour {

	private float gridSize = 1f;
	private Plane groundPlane;
	private bool picked = false;
	private bool blocked = false;

	private Material defaultMat, pickedMat, blockedMat;

	private GameObject buildingMesh;

	private ModeControl modeControl;

	// Use this for initialization
	void Awake () {
		groundPlane = new Plane(Vector3.up, Vector3.zero);
		defaultMat = Resources.Load("Models/Materials/Default") as Material;
		pickedMat = Resources.Load("Models/Materials/Picked") as Material;
		blockedMat = Resources.Load("Models/Materials/Blocked") as Material;
		buildingMesh = transform.GetChild(0).gameObject;

		modeControl = GameObject.Find("GameController").GetComponent<ModeControl>();
		if (modeControl == null)
			Debug.Log("ERROR: No GameController found");
	}
	
	// Update is called once per frame
	void Update () {

		if (picked)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			float rayDistance;

			if (groundPlane.Raycast(ray, out rayDistance))
			{
				Vector3 hit = ray.GetPoint(rayDistance);
				int x = Mathf.RoundToInt(hit.x / gridSize);
				int z = Mathf.RoundToInt(hit.z / gridSize);

				transform.position = new Vector3(x, 0, z);
			}
		}
	}

	void OnTriggerStay(Collider other)
	{
		Debug.Log("Collision: "+other);
		if (picked && other.gameObject.tag == "Building")
		{
			buildingMesh.renderer.material = blockedMat;
			blocked = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (picked && other.gameObject.tag == "Building")
		{
			buildingMesh.renderer.material = pickedMat;
			blocked = false;
		}
	}

	void OnMouseUp()
	{
		if (picked)
		{
			if (blocked)
			{
				buildingMesh.animation.Play("BuildingBlocked");
			}
			else
			{
				modeControl.SetBuildMode(false);
				buildingMesh.animation.Play("BuildingPlace");
				picked = false;
				buildingMesh.renderer.material = defaultMat;
			}
		}
		else
		{
			if (modeControl.GetBuildMode() == false)
			{
				modeControl.SetBuildMode(true);
				picked = true;
				buildingMesh.renderer.material = pickedMat;
			}
		}
	}

	public void SetPicked()
	{
		picked = true;
		buildingMesh.renderer.material = pickedMat;
	}
}
