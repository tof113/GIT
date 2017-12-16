using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My.Events;
using UnityEngine.UI;


public class Player : MonoBehaviour {

	public int score;
	public float damage;
	public int ammo;
	public IntEvent OnScoreChange;
	GameObject afficheScore;


	void OnEnable(){
		SetScore (0);
		ChangeText("" + score);
	}

	public void SetScore(int newScore){
		score = newScore;
		ChangeText("" + score);
		OnScoreChange.Invoke (score);
	}

	public void AddScore (int additionalScore) {
		SetScore(score += additionalScore);
	}

	public void ChangeText(string newText) {
		GameObject.Find ("Main Camera").transform.GetChild (0).GetChild (5).gameObject.GetComponent<Text>().text = newText;
	}

	public void ChangeAmmo(int currentAmmo){
		ammo = currentAmmo;
	}
		


}
