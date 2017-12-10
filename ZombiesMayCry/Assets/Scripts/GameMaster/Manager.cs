using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : Singleton<Manager>{

	public int currentLevel;
	//private PowerLeveling powerLevelinglScript;
	public string gameScene;

	//stats of the player
	public float lifeLeft;
	public int ammoLeft;

	// Use this for initialization
	void Awake() {
		DontDestroyOnLoad (this);
		//powerLevelinglScript = GetComponent<PowerLeveling> ();
		InitLevel (currentLevel);
	}

	public void GameOver(){
		//SceneManager.LoadScene ("");
		print ("GameOver - YOU DIE");
		//Application.Quit ();
	}

	public void LevelWon(){
		print ("lvl won");
		//tell the player spawner not to use the default stats of the player but customs ones !
		GameObject spawn = GameObject.Find ("PlayerSpawner");
		SpawnPlayer spawnPlayer = spawn.GetComponent<SpawnPlayer> ();

		spawnPlayer.firstTime = false;

		//save the stats of the player !
		GameObject player = GameObject.Find("Player");
		Health health = player.GetComponent<Health> ();
		lifeLeft = health.currentHealth;

		//ammo
		GameObject machineGun = player.transform.GetChild (1).gameObject;
		Gun playerGun = machineGun.GetComponent<Gun> ();
		ammoLeft = playerGun.maxBullets;


		SceneManager.LoadScene (gameScene);
	}

	void InitLevel(int level){
		//powerLevelinglScript.SetUpLevel (level);
	}

}
