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


			health.initHealth += life;
		}
	}
	#endregion
}
