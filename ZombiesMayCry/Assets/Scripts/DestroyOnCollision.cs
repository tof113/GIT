using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour {

	public LayerMask layerMask;

	void OnCollisionEnter(Collision col) {

		if (layerMask == (layerMask | (1 << col.gameObject.layer))) {
			Poolable poolable = gameObject.GetComponent<Poolable> ();
			if (!poolable)
				Destroy (gameObject);
			else
				poolable.TryPool ();
		}
	}

}
