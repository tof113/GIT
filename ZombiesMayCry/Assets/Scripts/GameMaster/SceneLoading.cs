using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour {

	public string gameScene;
	public string menu;
	public string startMenu;
	public string gameOverMenu;

	public void Play (){
		SceneManager.LoadScene (startMenu);
	}

	public void GameOver(){

		SceneManager.LoadScene (gameOverMenu);
	}

	public void Quit(){
		print ("You quit the game loser");
		Application.Quit ();
	}
}
