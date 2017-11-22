using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour {


	public LayerMask layerMask;

	public Poolable explosionPrefab;

	void OnCollisionEnter(Collision col){
		if (layerMask == (layerMask | (1 << col.gameObject.layer))) {// si ne differe pas alors meme layer
			print ("Damned i was killed by " + col.gameObject.name);
			Poolable poolable = gameObject.GetComponent<Poolable> ();
			//SpawnExplosion ();
			if (!poolable) {
				Destroy (gameObject);
			} else {
				poolable.TryPool ();
			}
		}
	}

	/*void SpawnExplosion(){
		if (explosionPrefab) {
			
			GameObject explosion = explosionPrefab.GetInstance ();
			explosion.transform.position = transform.position;
		}
	}*/
}
