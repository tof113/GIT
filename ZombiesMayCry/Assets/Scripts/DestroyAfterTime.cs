using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {

	public float delay;

	void OnEnable(){
		StartCoroutine (WaitAndDestroyCoroutine ());
	}

	void OnDisable(){
		StopAllCoroutines ();
	}

	IEnumerator WaitAndDestroyCoroutine(){
		yield return new WaitForSeconds (delay);
		Poolable poolable = GetComponent<Poolable> ();
		if (poolable) {
			poolable.TryPool ();
		} else {
			Destroy (gameObject);
		}
	}
	

}
