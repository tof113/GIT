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
	public bool inMenu;

	GameObject enemySpawner;
	SpawnEnemies spawnEnemies;

	//stats of the player !
	public float maxHealth;
	public float currentHealth;
	public float dmg;
	public int ammo;
	public int score;

	void Awake(){
		DontDestroyOnLoad (this);
	}

	void Start(){
		/*playerSpawner = GameObject.Find ("PlayerSpawner");
		spawnPlayer = playerSpawner.GetComponent<SpawnPlayer> ();
		enemySpawner  = GameObject.Find ("EnemySpawner");
		spawnEnemies = enemySpawner.GetComponent<SpawnEnemies> ();*/

	}

	public void Play (){
		SceneManager.LoadScene (gameScene);
		StartCoroutine( InitLevel());
	}

	/*public  void LevelWon(){

		//SavePlayerStats ();
		//SceneManager.LoadScene (menu);
		//GameObject playButton = GameObject.Find ("PlayButton");
		//NextLvl();

	}*/

	public void NextLvl(){
		StopAllCoroutines ();
		currentLevel++;
		//spawnEnemies.OnClear.RemoveAllListeners ();
		print ("lvl won");
		SavePlayerStats ();
		SaveHS ();
		//tell the player spawner not to use the default stats of the player but customs ones !
		/*GameObject spawn = GameObject.Find ("PlayerSpawner");
		SpawnPlayer spawnPlayer = spawn.GetComponent<SpawnPlayer> ();*/

		SceneManager.LoadScene (gameScene);
		StartCoroutine( InitLevel());
	}

	public void GameOver(){
		SaveHS ();
		SceneManager.LoadScene (gameOverMenu);
		print ("GameOver - YOU DIE");
		Destroy(this);

	}

	/*public void Quit(){
		Application.Quit ();s
	}*/

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

	void OnDestroy(){
		SaveHS ();
		print ("was destroyed");
	}

	IEnumerator Find(){
		yield return new WaitForSeconds (1f);
		print ("I have been cleared");
	}

	void SaveHS(){
		Player player = GameObject.Find ("Player").GetComponent<Player> ();
		PlayerPrefs.SetInt ("highScore", player.highScore);
		PlayerPrefs.Save ();
	}
}
