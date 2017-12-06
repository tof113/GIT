using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {


	public string mainMenuScene;
	public string gameOverScene;
	public string newGameScene;



	public void GameOver(){
		SceneManager.LoadScene (gameOverScene);
	}
	public void MainMenu(){
		SceneManager.LoadScene (mainMenuScene);
	}
	public void NewGame(){
		SceneManager.LoadScene (newGameScene);
	}

	public void QuitGame(){
		Application.Quit();
	}
}
