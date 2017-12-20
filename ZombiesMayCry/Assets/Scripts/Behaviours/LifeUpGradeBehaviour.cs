using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Behaviours/LifeBonusBehaviour")]
public class LifeUpGradeBehaviour : ObjectBehaviour {

	public float life;

	#region implemented abstract members of ObjectBehaviour
	public override void Execute (GameObject obj)
	{
		if (obj) {
			GameObject p = GameObject.Find ("Player");
			Health health = p.GetComponent<Health> ();
			float vieBase = health.initHealth;


			health.initHealth += life;
			float lifeMaj = health.currentHealth / vieBase * health.initHealth;
			health.currentHealth = lifeMaj;
			health.SetHealthBar ();
		}
	}
	#endregion
}
