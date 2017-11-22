using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool  {

	private Queue<GameObject> pool = new Queue<GameObject>();

	private GameObject prefab;

	public ObjectPool(GameObject prefab) {
		this.prefab = prefab;
	}

	public GameObject GetObject() {
		GameObject obj;
		if (pool.Count == 0) {
			obj = GameObject.Instantiate<GameObject> (prefab);
			obj.name = prefab.name;
		} else {
			obj = pool.Dequeue ();
		}
		return obj;
	}

	public bool PoolObject(GameObject obj) {
		if (obj.name != prefab.name) {
			return false;
		}
		pool.Enqueue (obj);
		return true;
	}
}
