using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My.Events;

public class PickedUp : MonoBehaviour {

	public GameObject player;

	public ObjectBehaviour disapearBehaviours;
	public ObjectBehaviour playerBehaviour;
	public LayerMask layerMask;

	void OnCollisionEnter(Collision col) {
		
		if (layerMask == (layerMask | (1 << col.gameObject.layer))) {
			GameObject source = col.gameObject;
			if (source) {
				
				player = source;
			}

			Disapear ();
		}
	}

	public void Disapear(){

		if (disapearBehaviours) {
			disapearBehaviours.Execute (gameObject);

		}
		if (playerBehaviour) {
			playerBehaviour.Execute (player);
		}


	}
}
