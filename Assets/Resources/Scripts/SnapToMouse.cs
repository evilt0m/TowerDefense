using UnityEngine;
using System.Collections;

public class SnapToMouse : MonoBehaviour {

	private float gridSize = 1f;
	private Plane groundPlane;
	private bool picked = false;
	private bool blocked = false;

	private Tower towerScript;

	private Material defaultMat, pickedMat, blockedMat;

	private GameObject buildingMesh;
	private Projector rangeProjector;

	private ModeControl modeControl;

	// Use this for initialization
	void Awake () {
		groundPlane = new Plane(Vector3.up, Vector3.zero);
		defaultMat = Resources.Load("Models/Materials/MetalBase") as Material;
		pickedMat = Resources.Load("Models/Materials/Picked") as Material;
		blockedMat = Resources.Load("Models/Materials/Blocked") as Material;
		buildingMesh = transform.GetChild(0).gameObject;
		rangeProjector = GetComponentInChildren<Projector>();
		towerScript = GetComponent<Tower>();

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
		//Debug.Log("Collision: "+other);
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
				rangeProjector.enabled = false;
				modeControl.SetBuildMode(false);
				buildingMesh.animation.Play("BuildingPlace");
				picked = false;
				buildingMesh.renderer.material = defaultMat;
				buildingMesh.collider.enabled = true;
				towerScript.ActivateTower(true);
			}
		}
		else
		{
			if (modeControl.GetBuildMode() == false)
			{
				rangeProjector.enabled = true;
				modeControl.SetBuildMode(true);
				picked = true;
				buildingMesh.renderer.material = pickedMat;
				buildingMesh.collider.enabled = false;
				towerScript.ActivateTower(false);
			}
		}
	}

	public void SetPicked()
	{
		picked = true;
		rangeProjector.enabled = true;
		buildingMesh.renderer.material = pickedMat;
		buildingMesh.collider.enabled = false;
		towerScript.ActivateTower(false);
	}
}
