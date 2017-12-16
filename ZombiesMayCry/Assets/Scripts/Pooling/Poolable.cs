using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour {

	public bool isPooled;

	public void SetPooled(bool mode) {
		gameObject.SetActive (!mode);
		isPooled = mode;
	}

	public bool TryPool() {
		if (isPooled) {
			Debug.LogWarning ("Trying to pool an already pooled object");
			return false;
		}
		SetPooled(ObjectPoolManager.Instance.PoolObject (gameObject));
		return isPooled;
	}

	public GameObject GetInstance() {
		GameObject obj = ObjectPoolManager.Instance.GetObject (gameObject);
		obj.GetComponent<Poolable>().SetPooled (false);
		return obj;
	}
}
