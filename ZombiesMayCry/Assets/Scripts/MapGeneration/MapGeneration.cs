using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class MapGeneration : MonoBehaviour {

	[Range (0,100)]
	public int randomFillPercent;// % de wall

	public int width;
	public int height;
	public int smoothing = 4; // nombre de fois que le smoothing est appliqué
	public int border = 5;
	public int wallThresholdSize = 50;
	public int roomThresholdSize = 50;
	public int tunnelSize = 1;

	public string seed; // creer une map identique
	public bool UseRandomSeed; // utilsie la seed ?

	public List<Room> survivingRooms;

	public List<Room> SurvivingRoomsGet {
		get {
			return survivingRooms;
		}
	}
		


	int [,] map; //0 empty, 1 wall




	// Use this for initialization
	void OnEnable () {
		GenerateMap ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(1)){
			GenerateMap();
		}
	}

	void GenerateMap(){
		map = new int[width, height];
		RandomFillMap ();

		for (int i = 0; i < 5; i++) {
			SmoothMap ();
		}

		ProcessMap ();

		//ajoute une bordure a la map
		int borderSize = border;
		int[,] borderMap = new int[width  + borderSize *2, height + borderSize *2];

		for (int x = 0; x < borderMap.GetLength(0); x++) {
			for (int y = 0; y < borderMap.GetLength(1); y++) {
				if (x >= borderSize && x < width + borderSize && y >= borderSize && y < height + borderSize) {
					borderMap [x, y] = map [x - borderSize, y - borderSize];
				} else {
					borderMap [x, y] = 1;
				}
					
			}
		}

		//Generation des Mesh
		MeshGenerator meshGen = GetComponent<MeshGenerator>();
		meshGen.GenerateMesh (borderMap, 1);//default 1
	}
	/*
	 * rempli la map de 0/1 aléatoirement selon le fill pourcentage + construit les murs
	 */ 
	void RandomFillMap(){
		if (UseRandomSeed) {
			seed = Time.time.ToString();// creer une seed random
		}

		System.Random pseudoRandom = new System.Random (seed.GetHashCode());

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (x == 0 || x == width - 1 || y == 0 || y == height - 1) {
					map [x, y] = 1;//mets des mur sur les bords
				} else {
					map [x, y] = (pseudoRandom.Next (0, 100) < randomFillPercent) ? 1: 0;//rempli la map de 0/1 selon le fill pourcentage
				}
			}
		}
	}
	/*
	 * lisse la map selon le nombre de voisins du meme type
	 */ 
	void SmoothMap(){
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				int neighbourWallTiles = GetSurroundingWallCount (x, y);

				if (neighbourWallTiles > smoothing) {
					map [x, y] = 1;
				} else if (neighbourWallTiles < smoothing) {
					map [x, y] = 0;
				}
			}
		}
	}

	/*
	 * return le nombre de voisins ui sont des mur
	 */
	int GetSurroundingWallCount(int gridX, int gridY){
		int wallCount = 0;
		for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) {
			for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
				if (IsInMapRange(neighbourX, neighbourY)) { // pas prendre en dehors de la carte
					if (neighbourX != gridX || neighbourY != gridY) {//pas prendre la cellule meme
						wallCount += map [neighbourX, neighbourY];
					}
				} else {
					wallCount++;
				}
			}
		}
		return wallCount;
	}
	/*
	 * Removes small regionsof walls or rooms
	 * Deals with connecting the remaining rooms
	 */ 
	void ProcessMap() {
		//remove small wall regions
		List<List<Coord>> wallRegions = GetRegions (1);

		foreach (List<Coord> wallRegion in wallRegions) {
			if (wallRegion.Count < wallThresholdSize) {
				foreach (Coord tile in wallRegion) {
					map[tile.tileX,tile.tileY] = 0;
				}
			}
		}
		//remove small rooms
		List<List<Coord>> roomRegions = GetRegions (0);

		survivingRooms = new List<Room>();
		foreach (List<Coord> roomRegion in roomRegions) {
			if (roomRegion.Count < roomThresholdSize) {
				foreach (Coord tile in roomRegion) {
					map [tile.tileX, tile.tileY] = 1;
				}
			} else {
				survivingRooms.Add (new Room (roomRegion, map));
			}
		}
		survivingRooms.Sort ();
		survivingRooms [0].isMainRoom = true;
		survivingRooms [0].isAccessibleFromMainRoom = true;
		ConnectClosestRooms (survivingRooms);
	}
	//return all the walls (1) or rooms (0)
	List<List<Coord>> GetRegions(int tileType) {
		List<List<Coord>> regions = new List<List<Coord>> ();
		int[,] mapFlags = new int[width,height];

		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				if (mapFlags[x,y] == 0 && map[x,y] == tileType) {
					List<Coord> newRegion = GetRegionTiles(x,y);
					regions.Add(newRegion);

					foreach (Coord tile in newRegion) {
						mapFlags[tile.tileX, tile.tileY] = 1;
					}
				}
			}
		}

		return regions;
	}
	//return the region of the same type as the selected tile =>Floodfull algorithm
	List<Coord> GetRegionTiles(int startX, int startY) {
		List<Coord> tiles = new List<Coord> ();
		int[,] mapFlags = new int[width,height];// 1 == already checked
		int tileType = map [startX, startY];

		Queue<Coord> queue = new Queue<Coord> ();
		queue.Enqueue (new Coord (startX, startY));
		mapFlags [startX, startY] = 1;

		while (queue.Count > 0) {
			Coord tile = queue.Dequeue();
			tiles.Add(tile);
			//regarder toutes la cases adjacente
			for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++) {
				for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++) {
					if (IsInMapRange(x,y) && (y == tile.tileY || x == tile.tileX)) {//dans la map et pas une diagonale
						if (mapFlags[x,y] == 0 && map[x,y] == tileType) {
							mapFlags[x,y] = 1;
							queue.Enqueue(new Coord(x,y));
						}
					}
				}
			}
		}
		return tiles;
	}
	//est dans la map
	bool IsInMapRange(int x, int y) {
		return x >= 0 && x < width && y >= 0 && y < height;
	}

	/**
	 * if false -> connect closes rooms together
	 * if true -> connect rooms to the closest connected room to the mainRoom possible !
	 */
	void ConnectClosestRooms (List<Room> allRooms, bool forceAccessibilityFromMainRoom = false){

		List<Room> roomListA = new List<Room> ();//not connected to the mainRoom
		List<Room> roomListB = new List<Room> ();//Connected to the mainRoom

		if (forceAccessibilityFromMainRoom) {
			foreach (Room room in allRooms) {
				if (room.isAccessibleFromMainRoom) {
					roomListB.Add (room);
				} else {
					roomListA.Add (room);
				}
			}
		} else {
			roomListA = allRooms;
			roomListB = allRooms;
		}


		int bestDistance = 0;
		Coord bestTileA = new Coord ();
		Coord bestTileB = new Coord ();
		Room bestRoomA = new Room ();
		Room bestRoomB = new Room ();
		bool possibleConnectionFound = false;

		foreach (Room roomA in roomListA) {
			if (!forceAccessibilityFromMainRoom) { 
				possibleConnectionFound = false;
				if (roomA.connectedRooms.Count > 0) {
					continue;
				}
			}
			foreach(Room roomB in roomListB){
				if (roomA == roomB || roomA.IsConnected(roomB)) {
					continue;
				}
				for(int tileIndexA =0; tileIndexA < roomA.edgeTiles.Count; tileIndexA ++){
					for(int tileIndexB =0; tileIndexB < roomB.edgeTiles.Count; tileIndexB ++){
						Coord tileA = roomA.edgeTiles [tileIndexA];
						Coord tileB = roomB.edgeTiles [tileIndexB];
						int distanceBetweenRooms = (int)(Math.Pow (tileA.tileX - tileB.tileX, 2) + Math.Pow (tileA.tileY - tileB.tileY, 2));
						if (distanceBetweenRooms < bestDistance || !possibleConnectionFound) {
							bestDistance = distanceBetweenRooms;
							possibleConnectionFound = true;
							bestTileA = tileA;
							bestTileB = tileB;
							bestRoomA = roomA;
							bestRoomB = roomB;

						}
					}
				}
			}

			if (possibleConnectionFound && !forceAccessibilityFromMainRoom) {
				CreatePassage (bestRoomA, bestRoomB, bestTileA, bestTileB);
			}
		}
		if (possibleConnectionFound && forceAccessibilityFromMainRoom) {
			CreatePassage (bestRoomA, bestRoomB, bestTileA, bestTileB);
			ConnectClosestRooms (allRooms, true);
		}

		if (!forceAccessibilityFromMainRoom) {//Once connected to another, connect to the mainRoom !
			ConnectClosestRooms (allRooms, true);
		}
	}

	/*crée une ligne entre deux pieces / cases d'une piece
	 */ 
	void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB){
		Room.ConnectRooms (roomA, roomB);
		Debug.DrawLine (CoordToWorldPoint (tileA), CoordToWorldPoint (tileB), Color.green, 100);

		List<Coord> line = GetLine (tileA, tileB);
		foreach (Coord c in line) {
			DrawCircle (c, tunnelSize);
		}
	}
	/*
	 * remplace un mur par un vide d'une case
	 */ 
	void DrawCircle(Coord c, int r){
		for (int x = -r; x <= r; x++) {
			for (int y = -r; y <= r; y++) {
				if (x * x + y * y <= r * r) {
					int drawX = c.tileX + x;
					int drawY = c.tileY + y;
					if (IsInMapRange (drawX, drawY)) {
						map [drawX, drawY] = 0;
					}
				}
			}
		}
	}

	/*
	 * algo permettant de trouver les coordonées des carrés en dessous d'une ligne entre 2 cases
	 */ 
	List<Coord> GetLine(Coord from, Coord to){
		List<Coord> line = new List<Coord> ();

		int x = from.tileX;
		int y = from.tileY;

		int dx = to.tileX - from.tileX;
		int dy = to.tileY - from.tileY;

		bool inverted = false;
		int step = Math.Sign (dx);
		int gradienStep = Math.Sign (dy);

		int longuest = Mathf.Abs (dx);
		int shortest = Mathf.Abs (dy);

		if (longuest < shortest) {//inverse si la pente est negative !!!
			inverted = true;
			longuest = Mathf.Abs (dy);
			shortest = Mathf.Abs (dx);
			step = Math.Sign (dy);
			gradienStep = Math.Sign (dx);

		}

		int gradienAccumulation = longuest / 2;
		for (int i = 0; i < longuest; i++) {
			line.Add (new Coord (x, y));
			if (inverted) {
				y += step;
			} else {
				x += step;
			}

			gradienAccumulation += shortest;
			if (gradienAccumulation >= longuest) {
				if (inverted) {
					x += gradienStep;
				} else {
					y += gradienStep;
				}
				gradienAccumulation -= longuest;
			}
		}

		return line;


	}

	Vector3 CoordToWorldPoint(Coord tile){
		return new Vector3 (-width / 2 + 0.5f + tile.tileX, 2, -height / 2 + 0.5f + tile.tileY);
	}


	/*
	void OnDrawGizmos(){ //Affichage
		if (map != null) {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					Gizmos.color = (map [x, y] == 1) ? Color.black : Color.white;
					Vector3 position = new Vector3 (-width / 2 + x + 0.5f, 0, -height / 2 + y + 0.5f);
					Gizmos.DrawCube (position, Vector3.one);
				}
			}
		}
	}*/
}
