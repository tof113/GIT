using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Behaviours/ScoreBehaviour")]
public class ScoreBehaviour : ObjectBehaviour {

	public int score;

	#region implemented abstract members of ObjectBehaviour
	public override void Execute (GameObject obj)
	{
		if (obj) {
			Player player = obj.GetComponent<Player> ();
			if (player) {
				player.AddScore (score);
			}
		}
	}
	#endregion


}
