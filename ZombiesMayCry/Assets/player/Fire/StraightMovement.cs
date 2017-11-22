using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightMovement : MonoBehaviour {


	public int speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 velocity = Vector3.right * speed * Time.deltaTime;
		transform.Translate (velocity, Space.Self);
	}
}
