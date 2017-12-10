using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPlayer : Singleton<SpawnPlayer> {

	private List<Room> rooms; // liste des toutes les pieces de la map ! 

	public GameObject player;
	private int width;
	private int height;
	public bool firstTime = true;

	void OnEnable(){
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode){
		GameObject map = GameObject.Find ("MapGenerator");
		if (map) {
			
			MapGeneration mapGen = map.GetComponent<MapGeneration> ();
			if (mapGen) {
				width = mapGen.width;
				height = mapGen.height;
				rooms = mapGen.survivingRooms;

			}
		}
		Spawn ();
	}

	void OnDisable(){
		//SceneManager.sceneLoaded -= OnSceneLoaded;
	}


	public void Spawn(){
		
		Coord spawnCoord = findPosition ();
		Coord test = new Coord(spawnCoord.tileX +1,(spawnCoord.tileY +1));
		GameObject obj = Instantiate (player);;

		//change attribute according to game manager save status !
		if (!firstTime) {
			print ("changing stats");
			//current life
			GameObject gameMaster = GameObject.Find("GameMaster");
			Manager manager = gameMaster.GetComponent<Manager> ();
			obj.GetComponent<Health> ().initHealth = manager.lifeLeft;
			//current ammo
			GameObject machineGun = player.transform.GetChild (1).gameObject;
			if (!machineGun) {
				print ("what ?");
			}
			Gun playerGun = machineGun.GetComponent<Gun> ();
			playerGun.maxBullets = manager.ammoLeft;

		} 
		obj.name = "Player";
		//add listner to prefab

		Health objHealth = obj.GetComponent<Health> ();
		objHealth.OnDie.AddListener (Manager.Instance.GameOver);

		obj.transform.position = CoordToWorldPoint (spawnCoord);
		obj.transform.rotation = transform.rotation;
		print ("i spawn player");
		Debug.DrawLine (CoordToWorldPoint (spawnCoord), CoordToWorldPoint (test), Color.blue, 100);
	}
		

	Coord findPosition(){
		
		Room spawnRoom = rooms[0];// rooms[0];//sorted by size desc so main room

		if (spawnRoom != null) {
			while(true){
				int random = Random.Range (0, spawnRoom.tiles.Count);
				Coord tile = spawnRoom.tiles [random];
				if (!spawnRoom.edgeTiles.Contains (tile)) {//pas le long du mur
					return tile;
				}
			}
		} else {
			print ("no room?");
		}
		print ("buuuug");
		return null;
	}

	Vector3 CoordToWorldPoint(Coord tile){
		return new Vector3 (-width / 2 + 0.5f + tile.tileX, -8, -height / 2 + 0.5f + tile.tileY);
	}
}

