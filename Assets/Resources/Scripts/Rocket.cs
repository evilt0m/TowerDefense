using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour {

	public float explosionRadius = 3f;
	public float minDamage = 10f;
	public float maxDamage = 25f;

	public GameObject explosionParticle;

	private Transform target;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//no target, just sit and relax
		if (target == null)
			return;

		Quaternion newRotation = Quaternion.LookRotation(target.transform.position - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10f);
		transform.position += transform.forward * Time.deltaTime * 20f;

	}

	public void SetTarget(Transform toEliminate)
	{
		target = toEliminate;
	}

	void OnCollisionEnter(Collision other)
	{
		Vector3 explosionPos = transform.position + Vector3.up;
		Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
		foreach (Collider hit in colliders) 
		{
			if (hit && hit.rigidbody)
			{
				hit.rigidbody.AddExplosionForce(2.5f, explosionPos + Vector3.up, explosionRadius, 3.0F, ForceMode.Impulse);
				if (hit.tag == "Enemy")
				{
					hit.GetComponent<Enemy>().AddDamage(Random.Range(minDamage, maxDamage));
				}
			}
		}

		Instantiate(explosionParticle, transform.position, Quaternion.identity);

		Destroy(transform.GetChild(0).gameObject, 1f);
		transform.DetachChildren();

		Destroy(gameObject);
	}
}
