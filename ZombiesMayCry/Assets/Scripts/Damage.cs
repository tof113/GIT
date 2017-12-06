using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour {

	public float dmg;


	void OnCollisionEnter(Collision col){
		//Owner myOwner = gameObject.GetComponent<Owner> ();

		//GameObject damageSource = myOwner ? myOwner.owner : null;
		GameObject obj = col.gameObject;
		if (obj) {
			Health health = obj.GetComponent<Health> ();
			if (health) {
				health.TakeDamage (dmg);
			}
		}
	}
}
