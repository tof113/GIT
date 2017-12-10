using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemies : MonoBehaviour {

	public float dmg;

	void OnCollisionStay(Collision col){
		//Owner myOwner = gameObject.GetComponent<Owner> ();

		//GameObject damageSource = myOwner ? myOwner.owner : null;
		GameObject obj = col.gameObject;
		if (obj) {
			Health health = obj.GetComponent<Health> ();
			if (health) {
				health.TakeDamage (dmg/4);
			}
		}
	}
}
