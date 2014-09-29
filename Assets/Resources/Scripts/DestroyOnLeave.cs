using UnityEngine;
using System.Collections;

public class DestroyOnLeave : MonoBehaviour {

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Projectile")
		{
			Destroy(other.gameObject);
		}
	}
}
