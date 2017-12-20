using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My.Events;
using UnityEngine.UI;


public class Player : MonoBehaviour {

	public int highScore;
	public int score;
	public float damage;
	public int ammo;
	public IntEvent OnScoreChange;
	GameObject afficheScore;


	void OnEnable(){
		SetScore (0);
		highScore = PlayerPrefs.GetInt ("highScore");
		ChangeText("" + score,""+highScore);
	}

	public void SetScore(int newScore){
		score = newScore;
		if (score > highScore) {
			highScore = score;
		}
		ChangeText("" + score, "" +highScore);
		OnScoreChange.Invoke (score);
	}

	public void AddScore (int additionalScore) {
		SetScore(score += additionalScore);
	}

	public void ChangeText(string newText,string hs) {
		GameObject.Find ("Main Camera").transform.GetChild (0).GetChild (5).gameObject.GetComponent<Text>().text = newText;
		GameObject.Find ("Main Camera").transform.GetChild (0).GetChild (11).gameObject.GetComponent<Text>().text = hs;
	}

	public void ChangeAmmo(int currentAmmo){
		ammo = currentAmmo;
	}
		


}
