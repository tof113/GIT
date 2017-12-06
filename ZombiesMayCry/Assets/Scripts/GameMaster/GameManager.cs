using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

	public int currentLevel;
	private PowerLeveling powerLevelinglScript;

	// Use this for initialization
	void Awake() {

		powerLevelinglScript = GetComponent<PowerLeveling> ();
		InitLevel (currentLevel);
	}

	void InitLevel(int level){
		//powerLevelinglScript.SetUpLevel (level);
	}
	// Update is called once per frame
	/*void Update () {
		if(nomoreenemies!){
			InitLevel(currentLevel);
		}
	}*/
}
