using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Behaviours/DamageBonusBehaviour")]
public class LifeUpGradeBehaviour : ObjectBehaviour {

	public float damage;

	#region implemented abstract members of ObjectBehaviour
	public override void Execute (GameObject obj)
	{
		if (obj) {
			GameObject p = GameObject.Find ("Player");

			Player player = p.GetComponent<Player> ();

			player.damage += damage;
		}
	}
	#endregion
}
