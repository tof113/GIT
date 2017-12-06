using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonController : MonoBehaviour {

	public Gun[] guns;
	private Gun currentGun;
	//timer pour emepcher le spam du shoot
	public float timeBetweenBullets = 0.15f;
	float timer;


	void OnEnable() {
		foreach (Gun x in guns) {
			x.SetShooting (false);
		}
		currentGun = guns [0];
	}

	// Update is called once per frame
	void Update () {
		//demarre le timer;
		timer += Time.deltaTime;


		//gere arme courrante
		if(!Input.GetButton("Fire1")){
		
			if(Input.GetKeyDown(KeyCode.Alpha2)){
				currentGun = guns [1];
			}
		
			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				currentGun = guns [0];
			}
		}
		if (currentGun.maxBullets <= 0) {
			currentGun = guns [0];
		}
		if (currentGun == guns [0]) {
			currentGun.maxBullets = 100;
		}

		if (Input.GetButtonDown ("Fire1") && timer >= timeBetweenBullets) {
			currentGun.SetShooting(true);
		}
		if (Input.GetButtonUp("Fire1")) {
			//reinitailise le timer
			timer = 0f;
			currentGun.SetShooting (false);
		}
	}
}
