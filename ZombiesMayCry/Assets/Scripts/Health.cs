using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My.Events;

public class Health : MonoBehaviour {


	public int initHealth;
	public int currentHealth;
	public GameObject damageDealer;

	public ObjectBehavioursList dieBehavours;
	//public ObjectBehavioursList dieDamageDealerBehaviours;

	public ObjectEvent OnDie;

	void OnEnable(){
		currentHealth = initHealth;
	}


	public void TakeDamage(int dmg, GameObject source = null){
		
		if (source) {
			damageDealer = source;
		}
		currentHealth -= dmg;
		if (currentHealth <= 0) {
			Die ();
		}
	}

	public void Die(){
		
		OnDie.Invoke(damageDealer);

		if (dieBehavours) {
			dieBehavours.Execute (gameObject);

		}
		/*if (dieDamageDealerBehaviours) {
			dieDamageDealerBehaviours.Execute (damageDealer);
		}*/

		
	}
}
