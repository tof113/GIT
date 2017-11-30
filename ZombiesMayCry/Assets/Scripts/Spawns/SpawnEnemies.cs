using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnEnemies : MonoBehaviour {

	private List<Room> rooms; // liste des toutes les pieces de la map ! 
	private int currentEnemies=0;
	public int maxEnemies;
	public Poolable enemyPrefab;
	private int width;
	private int height;
	private List<Coord> coordsOfEveryTiles;//toutes les coords de toutes les salles dans la meme liste
	private List<Coord> coordsOfEveryEdgeTiles;


	public float maxTimeBetweenSpawn = 0.5f;
	public float minTimeBetweenSpawn = 0.1f;
	public int minSpawnDistance;
	public int minSpawnDistanceToPlayer;
	public LayerMask layerMaskEnemy;
	public LayerMask layerMaskPlayer;

	void Start(){


		GameObject map = GameObject.Find ("MapGenerator");
		if (map) {

			MapGeneration mapGen = map.GetComponent<MapGeneration> ();
			if (mapGen) {
				width = mapGen.width;
				height = mapGen.height;
				rooms = mapGen.survivingRooms;

				print (rooms.ToArray ().Length);
			}
		}
		buildListWithAllCoords ();
		StartCoroutine (SpawnCoroutine ());
	}
	void OnEnable(){
		//StartCoroutine (SpawnCoroutine ());
	}	

	void OnDisable(){
		StopCoroutine(SpawnCoroutine ());
	}

	IEnumerator SpawnCoroutine(){
		while (currentEnemies < maxEnemies) {
			Spawn ();
			yield return new WaitForSeconds (Random.Range(minTimeBetweenSpawn,maxTimeBetweenSpawn));
		}
	}


	void Spawn(){
		

		Coord spawnCoord = findPosition (20);
		if (spawnCoord != null) {
			Coord test = new Coord (spawnCoord.tileX + 1, (spawnCoord.tileY + 1));

			GameObject obj = enemyPrefab.GetInstance ();
			obj.transform.position = CoordToWorldPoint (spawnCoord);
			obj.transform.rotation = transform.rotation;
			Debug.DrawLine (CoordToWorldPoint (spawnCoord), CoordToWorldPoint (test), Color.red, 100);
			currentEnemies++;
		} else {	
			print ("no suitable place found");
		}

	}
		

	Coord findPosition(int maxAttemps){
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

	void buildListWithAllCoords(){
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
}

