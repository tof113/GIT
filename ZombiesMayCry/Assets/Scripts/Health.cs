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

	public EmptyEvent OnDie;

	public GameObject healthBar;

	void OnEnable(){
		currentHealth = initHealth;
		player = GameObject.Find ("Player");
		if (this.transform.parent == player) {
			healthBar = GameObject.Find ("Main Camera").transform.GetChild (0).GetChild (2).gameObject;
		}
	}


	public void TakeDamage(float dmg){
		
		currentHealth -= dmg;
		if(healthBar!=null)
			SetHealthBar();
		if (currentHealth <= 0) {
			Die ();
		}
	}

	public void Die(){

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
		if(healthBar!=null)
			SetHealthBar();
	}

	public void SetHealthBar(){
		var myHealth = 0.158f / initHealth * currentHealth;
		if (Mathf.Sign (myHealth) < 0) {
			myHealth = 0f;
		}
		//met à jour l'affichage avec la vie courrante (entre 0 - 1)
		healthBar.transform.localScale = new Vector3 (myHealth,healthBar.transform.localScale.y,healthBar.transform.localScale.z);
	}
}
