using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My.Events;
using UnityEngine.UI;

public class Gun : MonoBehaviour {

	public Poolable projectilePrefab;

	public float fireInterval = 0.1f;

	private bool isShooting = false;

	GameObject moi;

	GameObject player;
	Player p;
	public IntEvent OnFire;

	//munitions
	public int maxBullets;
	public CanonController arme;

	public void Start(){
		arme =this.transform.parent.GetComponent<CanonController> ();
		Debug.Log (arme);	
		player = GameObject.Find ("Player");
		p = player.GetComponent<Player> ();
	}

	public void SetShooting(bool mode) {
		isShooting = mode;
		StopAllCoroutines ();
		if (mode) {
			StartCoroutine (FireCoroutine ());
		}
	}


	void Fire() {
		//Instantiate (projectilePrefab, transform.position, transform.rotation);
		GameObject obj = projectilePrefab.GetInstance();
		if (player) {
			Damage damage = obj.GetComponent<Damage> ();
			damage.dmg = p.damage;
		}
		
		obj.transform.position = transform.position;
		obj.transform.rotation = transform.rotation;

		if (arme.currentGun == arme.guns [1]) {
			maxBullets--;
			p.ammo--;
			OnFire.Invoke (maxBullets);
			ChangeBullets ("" + maxBullets);
		}else {
			ChangeBullets ("∞");
		}
	}

	IEnumerator FireCoroutine() {
		while (maxBullets > 0) {
			if(isShooting)
				Fire ();
			yield return new WaitForSeconds (fireInterval);
		}
	}

	public void ChangeBullets(string newBullets) {
		GameObject.Find ("Main Camera").transform.GetChild (0).GetChild (9).gameObject.GetComponent<Text>().text = newBullets;
	}
}
