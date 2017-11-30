using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
	/*
	 * informations sur les pieces
	 */ 

	
public class Room : IComparable<Room>{



	public List<Coord> tiles;
	public List<Coord> edgeTiles;
	public List<Room> connectedRooms;
	public int roomSize;
	public bool isAccessibleFromMainRoom;
	public bool isMainRoom;

	public Room(){

	}
	public Room(List<Coord> roomTiles, int [,] map){
		tiles = roomTiles;
		roomSize = tiles.Count;
		connectedRooms = new List<Room>();

		edgeTiles = new List<Coord>();
		foreach(Coord tile in tiles){
			for(int x = tile.tileX-1; x <= tile.tileX+1;x++){
				for(int y = tile.tileY-1; y <= tile.tileY+1;y++){
					if(x == tile.tileX || y == tile.tileY){
						if(map[x,y] == 1){
							edgeTiles.Add(tile);
						}
					}
				}
			}
		}
	}

	public void SetAccessibleFrommainRoom(){
		if (!isAccessibleFromMainRoom) {
			isAccessibleFromMainRoom = true;
			foreach (Room room in connectedRooms) {
				room.isAccessibleFromMainRoom =true;
			}
		}
	}

	public static void ConnectRooms(Room roomA, Room roomB){
		if (roomA.isAccessibleFromMainRoom) {
			roomB.isAccessibleFromMainRoom = true;
		} else if (roomB.isAccessibleFromMainRoom) {
			roomA.isAccessibleFromMainRoom = true;
		}
		roomA.connectedRooms.Add (roomB);
		roomB.connectedRooms.Add (roomA);

	}

	public bool IsConnected(Room otherRoom){
		return connectedRooms.Contains (otherRoom);
	}

	public int CompareTo(Room otherRoom){
		return otherRoom.roomSize.CompareTo (roomSize);
	}



}
