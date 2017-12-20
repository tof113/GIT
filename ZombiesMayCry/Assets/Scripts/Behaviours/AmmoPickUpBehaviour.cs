using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Behaviours/AmmoBonusBehaviour")]
public class AmmoPickUpBehaviour : ObjectBehaviour {

	public int ammunition;

	#region implemented abstract members of ObjectBehaviour
	public override void Execute (GameObject obj)
	{
		if (obj) {
			GameObject player = GameObject.Find ("Player");
			CanonController cc = player.GetComponent<CanonController> ();
			GameObject machineGun = obj.transform.GetChild (1).gameObject;
			Gun playerGun = machineGun.GetComponent<Gun> ();
			Player p = player.GetComponent<Player> ();
			p.ammo += ammunition;
			playerGun.maxBullets += ammunition;
			if (cc.currentGun == cc.guns [1]) {
				playerGun.ChangeBullets ("" + playerGun.maxBullets);

			}
		}
	}
	#endregion
}
