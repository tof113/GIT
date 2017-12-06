using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Behaviours/AmmoBonusBehaviour")]
public class AmmoPickUpBehaviour : ObjectBehaviour {

	public int ammo;

	#region implemented abstract members of ObjectBehaviour
	public override void Execute (GameObject obj)
	{
		if (obj) {
			GameObject machineGun = obj.transform.GetChild (1).gameObject;
			Gun playerGun = machineGun.GetComponent<Gun> ();
			if (playerGun) {
				playerGun.maxBullets += ammo;
			}
		}
	}
	#endregion
}
