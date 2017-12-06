using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My.Events;
public class Owner : MonoBehaviour {

	public GameObject owner;
	public ObjectEvent OnChangeOwner;

	public void SetOwner(GameObject newOwner){
		owner = newOwner;
		OnChangeOwner.Invoke (owner);
	}

	public void Own(GameObject target){
		if (target) {
			Owner owner = target.GetComponent<Owner> ();
			if (owner) {
				owner.SetOwner (this.owner);
			}
		}
	}
		
}
