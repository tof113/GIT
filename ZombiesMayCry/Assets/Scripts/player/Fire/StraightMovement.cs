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
		Rigidbody bullet= GetComponent<Rigidbody> ();
		bullet.velocity = transform.right * speed;
	

	}
}
