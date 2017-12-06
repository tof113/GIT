﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Behaviours/HealthBonusBehaviour")]
public class HealthBonusBehaviour : ObjectBehaviour {

	public int health;

	#region implemented abstract members of ObjectBehaviour
	public override void Execute (GameObject obj)
	{
		if (obj) {
			Health playerHealth = obj.GetComponent<Health> ();
			if (playerHealth) {
				playerHealth.SetHealth (health);
			}
		}
	}
	#endregion
}
