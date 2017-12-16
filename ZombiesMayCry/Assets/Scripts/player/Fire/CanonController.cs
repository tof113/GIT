using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanonController : MonoBehaviour {

	public Gun[] guns;
	public Gun currentGun;
	//timer pour emepcher le spam du shoot
	public float timeBetweenBullets = 0.15f;
	float timer;


	void OnEnable() {

		foreach (Gun x in guns) {
			x.SetShooting (false);
		}
		currentGun = guns [0];
		ChangeText("Handgun :", "∞");
	}

	// Update is called once per frame
	void Update () {
		//demarre le timer;
		timer += Time.deltaTime;


		//gere arme courrante
		if(!Input.GetButton("Fire1")){
		
			if(Input.GetKeyDown(KeyCode.Alpha2)){
				currentGun = guns [1];
				ChangeText("Gatling :" , "" + currentGun.maxBullets);
			}
		
			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				currentGun = guns [0];
				ChangeText("Handgun :" , "∞");
			}
		}
		if (currentGun.maxBullets <= 0) {
			currentGun = guns [0];
			ChangeText("Handgun :" , "∞");
		}
		if (currentGun == guns [0]) {
			currentGun.maxBullets = 100;
		}

		if (Input.GetButtonDown ("Fire1") && timer >= timeBetweenBullets) {
			currentGun.SetShooting(true);
			if (currentGun == guns [1]) {
				ChangeText("Gatling :" , "" + currentGun.maxBullets);
			}
		}
		if (Input.GetButtonUp("Fire1")) {
			//reinitailise le timer
			timer = 0f;
			currentGun.SetShooting (false);
		}
	}

	public void ChangeText(string newText, string newBullets) {
		GameObject.Find ("Main Camera").transform.GetChild (0).GetChild (8).gameObject.GetComponent<Text>().text = newText;
		GameObject.Find ("Main Camera").transform.GetChild (0).GetChild (9).gameObject.GetComponent<Text>().text = newBullets;
	}
	
}
