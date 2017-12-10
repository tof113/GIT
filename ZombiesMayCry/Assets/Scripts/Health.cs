using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My.Events;

public class Health : MonoBehaviour {


	public float initHealth;
	public float currentHealth;

	public ObjectBehavioursList dieBehavours;
	public ObjectBehaviour dieDamageDealerBehaviours;

	public EmptyEvent OnDie;

	void OnEnable(){
		currentHealth = initHealth;
	}


	public void TakeDamage(float dmg){


			currentHealth -= dmg;
			if (currentHealth <= 0) {	
				Die ();

			}

	}

	public void Die(){

		print ("I die for my country");

		OnDie.Invoke ();
		OnDie.RemoveAllListeners ();
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
