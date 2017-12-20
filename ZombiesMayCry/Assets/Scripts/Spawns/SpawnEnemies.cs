using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SpawnEnemies : MonoBehaviour {

	private List<Room> rooms; // liste des toutes les pieces de la map ! 
	public int enemiesSpawned;

	public int maxEnemies;
	public int enemiesStillAlive;
	public Poolable enemyPrefab;
	public Poolable enemyHardPrefab;
	public Poolable enemyBossPrefab;
	[Range (0f,100f)]
	public float difficultyEnemy;// % de wall
	private int width;
	private int height;
	private List<Coord> coordsOfEveryTiles;//toutes les coords de toutes les salles dans la meme liste
	private List<Coord> coordsOfEveryEdgeTiles;

	private const int BOSS = 3;
	private const int HARD = 2;
	private const int NORMAL = 1;

	public float normalEnemyLife =1f;
	public float hardEnemyLife=2f;
	public float bossEnemyLife=5f;



	public float difficulty;
	public float maxTimeBetweenSpawn = 0.5f;
	public float minTimeBetweenSpawn = 0.1f;
	public int minSpawnDistance;
	public int minSpawnDistanceToPlayer;
	public LayerMask layerMaskEnemy;
	public LayerMask layerMaskPlayer;

	public EmptyEvent OnClear;

	void OnDisable(){
		//SceneManager.sceneLoaded -= OnSceneLoaded;
		StopCoroutine(SpawnCoroutine ());
	}

	void Start(){
			
			GameObject map = GameObject.Find ("MapGenerator");
			if (map) {

				MapGeneration mapGen = map.GetComponent<MapGeneration> ();
				if (mapGen) {
					width = mapGen.width;
					height = mapGen.height;
					rooms = mapGen.survivingRooms;
					BuildListWithAllCoords ();
					//DisplayMap ();
					//DisplayEdges ();
					StartCoroutine (SpawnCoroutine ());
				}
			} else {
				print ("map not found");
			}

	}
		

	IEnumerator SpawnCoroutine(){
		while (enemiesSpawned < maxEnemies) {
			Spawn ();
			yield return new WaitForSeconds (Random.Range(minTimeBetweenSpawn,maxTimeBetweenSpawn));
		}
	}


	void Spawn(){
		int enemyType = NORMAL;
		Coord spawnCoord = FindPosition (20);
		if (spawnCoord != null) {
			
			//Coord test = new Coord (spawnCoord.tileX + 1, (spawnCoord.tileY + 1));
			float whichEnemy = Random.Range (0f, 100f);

			GameObject obj = null;
			if (enemiesSpawned == maxEnemies - 1) {
				print ("I spawn a MONSTER");
				obj = enemyBossPrefab.GetInstance ();
				enemyType = BOSS;

			}else if (whichEnemy < difficultyEnemy) {
				obj = enemyHardPrefab.GetInstance ();
				enemyType = HARD;
			} else {	
				obj = enemyPrefab.GetInstance ();

			}

			Health objHealth = obj.GetComponent<Health> ();

			switch (enemyType) {
			case NORMAL:
				objHealth.initHealth = normalEnemyLife * difficulty;
				objHealth.currentHealth = normalEnemyLife * difficulty;

				break;
			case HARD:
				objHealth.initHealth = hardEnemyLife * difficulty;
				objHealth.currentHealth = hardEnemyLife * difficulty;

				break;
			case BOSS:
				objHealth.initHealth = bossEnemyLife * difficulty;
				objHealth.currentHealth = bossEnemyLife * difficulty;

				break;

			}
			//add de l'event au prefab permettant le comptage
			objHealth.OnDie.AddListener (LvlCleared);

			obj.transform.position = CoordToWorldPoint (spawnCoord);
			//Debug.DrawLine (CoordToWorldPoint (spawnCoord), CoordToWorldPoint (test), Color.red, 20)
			enemiesSpawned++;
			ChangeTextZombies ("" + enemiesStillAlive);
		} else {	
			print ("no suitable place found");
		}

	}
		

	Coord FindPosition(int maxAttemps){
		for (int i = 0; i < maxAttemps; i++) {
			int random = Random.Range (0, coordsOfEveryTiles.Count);
			Coord tile = coordsOfEveryTiles [random];

			if (!coordsOfEveryEdgeTiles.Contains(tile) && Physics.OverlapSphere (CoordToWorldPoint(tile), minSpawnDistance, layerMaskEnemy).Length ==0 
				&& Physics.OverlapSphere (CoordToWorldPoint(tile), minSpawnDistanceToPlayer, layerMaskPlayer).Length ==0) {//pas le long du mur
				return tile;
			}
			print ("zombi at this location ^^");
		}
		print ("too much zombies already ^^");
		return null;
	}

	Vector3 CoordToWorldPoint(Coord tile){
		return new Vector3 (-width / 2 + 0.5f + tile.tileX, -6, -height / 2 + 0.5f + tile.tileY);
	}

	void BuildListWithAllCoords(){
		coordsOfEveryTiles = new List<Coord> ();
		coordsOfEveryEdgeTiles = new List<Coord> ();
		foreach (Room room in rooms) {
			foreach (Coord coord in room.tiles) {
				coordsOfEveryTiles.Add (coord);
			}
			foreach (Coord edgeCoord in room.edgeTiles) {
				coordsOfEveryEdgeTiles.Add (edgeCoord);
			}
		}
	}

	public void LvlCleared(){
		enemiesStillAlive--;
		ChangeTextZombies ("" + enemiesStillAlive);

			
		if(enemiesStillAlive <= 0 && enemiesSpawned == maxEnemies){	
			enemiesSpawned = 0;
			StartCoroutine( Wait ());

		}
	}
	IEnumerator Wait(){
		yield return new WaitForSeconds (3f);
		print ("I have been cleared");
		OnClear.Invoke ();
	}

	void DisplayMap(){
		foreach (Coord c in coordsOfEveryTiles) {
			Coord test = new Coord (c.tileX + 1, (c.tileY + 1));
			Debug.DrawLine (CoordToWorldPoint (c), CoordToWorldPoint (test), Color.cyan, 10);
		}
	}

	void DisplayEdges(){
		foreach (Coord c in coordsOfEveryEdgeTiles) {
			Coord test = new Coord (c.tileX + 1, (c.tileY + 1));
			Debug.DrawLine (CoordToWorldPoint (c), CoordToWorldPoint (test), Color.gray, 10);
		}
	}

	public void ChangeTextZombies(string newText){
		GameObject.Find ("Main Camera").transform.GetChild (0).GetChild (7).gameObject.GetComponent<Text> ().text = newText;
	}


}

