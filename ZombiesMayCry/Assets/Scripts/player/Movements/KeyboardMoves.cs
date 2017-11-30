using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMoves : MonoBehaviour {

	public float speed=6f;

	Vector3 movement;
	Rigidbody playerRigidbody;
	int floorMask;
	float camRayLength = 100f;

	void Awake(){
		floorMask = LayerMask.GetMask ("Floor");
		playerRigidbody = GetComponent<Rigidbody> ();
	}

	void FixedUpdate(){
		// staticMove
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");
		Move (h, v);

		/* move d'après souris
		float h = Input.GetAxisRaw ("Horizontal") * Time.deltaTime * speed;
		float v = Input.GetAxisRaw ("Vertical") * Time.deltaTime * speed;
		transform.Translate(h, 0, v); */

		//KinecticMovement ();
		Turning ();
	}

	void Move (float h, float v){
		movement.Set (h, 0f, v);
		movement = movement.normalized * speed * Time.deltaTime;
		playerRigidbody.MovePosition (transform.position + movement);
	}

	void Turning(){

		//rayon entre curseur et vers la cam
		Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		//ce qui a ete tocuher par le rayon
		RaycastHit floorHit;
	
		if(Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)){
			Vector3 playerToMouse = floorHit.point - transform.position;
			//s'assurer que ce soit bien au sol
			playerToMouse.y = 0f;
			//la rotation
			Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
			//oriente l'objet
			playerRigidbody.MoveRotation (newRotation);
			//transform.rotation = newRotation;
		}
	}



	void KinecticMovement(){
		Vector3 velocity = GetAxisVector ();
		velocity *= speed * Time.deltaTime;
		transform.position += velocity;

		//ClampPosition ();
	}

	Vector3 GetAxisVector(){
		float hDirection = Input.GetAxisRaw ("Horizontal");
		float vDirection = Input.GetAxisRaw ("Vertical");
		return new Vector3 (hDirection, 0, vDirection);
	}
}
