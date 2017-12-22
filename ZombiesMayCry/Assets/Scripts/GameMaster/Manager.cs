using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : Singleton<Manager>{

	public int currentLevel;
	public string gameScene;
	public string menu;
	public string startMenu;
	public string gameOverMenu;
	public int nbEnemiesPerWaves;
	public float enemyDifficulty;
	public float minSpawnTime;
	public float maxSpawnTime;

	GameObject enemySpawner;
	SpawnEnemies spawnEnemies;

	//stats of the player 
	public float maxHealth;
	public float currentHealth;
	public float dmg;
	public int ammo;
	public int score;

	void Awake(){
		DontDestroyOnLoad (this);
	}
		

	public void Play (){
		StartCoroutine (WaitBeforePlay ());

		StartCoroutine( InitLevel());
	}
		

	public void NextLvl(){
		StopAllCoroutines ();
		currentLevel++;
		print ("lvl won");
		SavePlayerStats ();
		SaveHS ();

		SceneManager.LoadScene (gameScene);
		StartCoroutine( InitLevel());
	}

	public void GameOver(){
		SaveHS ();
		SceneManager.LoadScene (gameOverMenu);
		print ("GameOver - YOU DIE");
		Destroy(this);

	}
		

	IEnumerator InitLevel(){
		yield return new WaitForSeconds (1f);
		//everythings related to EnemySpawner
		enemySpawner  = GameObject.Find ("EnemySpawner");

		while (!enemySpawner) {
			enemySpawner  = GameObject.Find ("EnemySpawner");
			yield return new WaitForSeconds (1f);
		}
		spawnEnemies = enemySpawner.GetComponent<SpawnEnemies> ();
		if (spawnEnemies) {
			spawnEnemies.OnClear.AddListener (Manager.Instance.NextLvl);
		}

		spawnEnemies.maxEnemies = currentLevel * nbEnemiesPerWaves;
		spawnEnemies.enemiesStillAlive = currentLevel * nbEnemiesPerWaves;
		spawnEnemies.difficulty = currentLevel;
		if (enemyDifficulty < 40) {
			enemyDifficulty += Random.Range (1f, 5f);
		}
		spawnEnemies.difficultyEnemy = enemyDifficulty;
		if (currentLevel > 1) {
			LoadPlayerStats ();
		}
		if (minSpawnTime > 0.2f) {
			minSpawnTime -= 0.1f;
		}
			spawnEnemies.minTimeBetweenSpawn = minSpawnTime;
		
		if (maxSpawnTime > 0.5f) {
			maxSpawnTime -= 0.1f;
		}
			spawnEnemies.maxTimeBetweenSpawn = maxSpawnTime;
		
	}

	void SavePlayerStats(){
		GameObject p = GameObject.Find ("Player");
		Player player = p.GetComponent<Player> ();
		Health health = p.GetComponent <Health> ();

		ammo = player.ammo;
		dmg = player.damage;
		maxHealth = health.initHealth;
		currentHealth = health.currentHealth;
		score = player.score;


	}

	void LoadPlayerStats(){
		GameObject p = GameObject.Find ("Player");
		Player player = p.GetComponent<Player> ();
		Health health = p.GetComponent <Health> ();
		CanonController cc = p.GetComponent<CanonController> ();
		Gun machineGun = cc.guns [1];

		machineGun.maxBullets = ammo;
		player.ammo = ammo;

		player.damage = dmg;
		health.currentHealth = currentHealth;
		health.initHealth = maxHealth;
		player.score = score;

		player.ChangeText ("" + score, "" + player.highScore);
		health.SetHealthBar ();
	}
		



	void SaveHS(){
		Player player = GameObject.Find ("Player").GetComponent<Player> ();
		PlayerPrefs.SetInt ("highScore", player.highScore);
		PlayerPrefs.Save ();
	}

	IEnumerator WaitBeforePlay(){
		yield return new WaitForSeconds (1f);
		SceneManager.LoadScene (gameScene);
	}
}
