using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My.Events;

public class Health : MonoBehaviour {


	public float initHealth;
	public float currentHealth;

	public ObjectBehavioursList dieBehavours;
	public ObjectBehaviour dieDamageDealerBehaviours;

	private GameObject player;

	public ObjectEvent OnDie;

	void OnEnable(){
		currentHealth = initHealth;
		player = GameObject.Find ("Player");
	}


	public void TakeDamage(float dmg){
		
		currentHealth -= dmg;
		if (currentHealth <= 0) {
			Die ();
		}
	}

	public void Die(){

		OnDie.Invoke (player);
		if (dieBehavours) {
			dieBehavours.Execute (gameObject);

		}
		if (dieDamageDealerBehaviours) {
			GameObject player = GameObject.Find ("Player");
			dieDamageDealerBehaviours.Execute (player);
		}

		
	}

	public void SetHealth(int bonus){

		currentHealth += bonus;
		if (currentHealth > initHealth) {
			currentHealth = initHealth;
		}
	}
}
