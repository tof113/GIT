using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My.Events;

public class Player : Singleton<Player> {

	public int score;
	public float damage;
	public IntEvent OnScoreChange;

	void OnEnable(){
		SetScore (0);
	}

	public void SetScore(int newScore){
		score = newScore;
		OnScoreChange.Invoke (score);
	}

	public void AddScore (int additionalScore) {
		SetScore(score += additionalScore);
	}

}
