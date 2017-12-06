using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnPlayer : MonoBehaviour {

	private List<Room> rooms; // liste des toutes les pieces de la map ! 

	public Poolable player;
	private int width;
	private int height;

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
		Spawn ();
	}


	void Spawn(){
		
		Coord spawnCoord = findPosition ();
		Coord test = new Coord(spawnCoord.tileX +1,(spawnCoord.tileY +1));

		GameObject obj = player.GetInstance();
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

