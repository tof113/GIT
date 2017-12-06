using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My.Events;

public class Gun : MonoBehaviour {

	public Poolable projectilePrefab;

	public float fireInterval = 0.1f;

	public ObjectEvent OnFire;

	private bool isShooting = false;

	//munitions
	public int maxBullets;

	public void SetShooting(bool mode) {
		isShooting = mode;
		StopAllCoroutines ();
		if (mode) {
			StartCoroutine (FireCoroutine ());
		}
	}


	void Fire() {
		//Instantiate (projectilePrefab, transform.position, transform.rotation);
		GameObject obj = projectilePrefab.GetInstance();
		obj.transform.position = transform.position;
		obj.transform.rotation = transform.rotation;
		OnFire.Invoke (obj);
		maxBullets--;
	}

	IEnumerator FireCoroutine() {
		while (true) {
			if(isShooting && maxBullets > 0)
				Fire ();
			yield return new WaitForSeconds (fireInterval);
		}
	}


}
