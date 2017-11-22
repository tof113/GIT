using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsCleaner : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter2D(Collider2D col){
		Rigidbody2D rb = col.attachedRigidbody;
		GameObject obj;
		if (rb) {
			obj = rb.gameObject;
		}else{
				obj = col.gameObject;
			}
		Poolable poolable = obj.GetComponent<Poolable> ();
		if (poolable) {
			poolable.TryPool ();
		}else{
			Destroy (obj);
				
		}
	}
}
