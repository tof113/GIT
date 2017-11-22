using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KeyboardMovment : MonoBehaviour {

	[SerializeField]
	float speed = 1f;

	public Rect boundries;
	// Use this for initialization
	void Start () {
		
	}
	void Update(){
		KinematicMove();
	}

	void Awake(){
		boundries = ScreenToRect (0.5f);
	}

		
	//depacelemnt par frame
	void KinematicMove(){
		
		Vector3 velocity = GetAxisVector ();
		velocity *= speed* Time.deltaTime;

		transform.position += velocity;

		//ClampPosition ();
	}

	Vector3 GetAxisVector(){
		float hDirection = Input.GetAxisRaw ("Horizontal");
		float vDirection = Input.GetAxisRaw ("Vertical");
		return new Vector3 (hDirection, vDirection, 0);
	}

	void ClampPosition(){
		Vector3 newPos = transform.position;
		newPos.x = Mathf.Clamp (transform.position.x, boundries.xMin, boundries.xMax);
		newPos.y = Mathf.Clamp (transform.position.y, boundries.yMin, boundries.yMax);

		transform.position = newPos;
	}
	/*
	void OnDrawGizmos(){
		GizmosUtils.DrawRectangle (boundries, Color.green);
	}
*/
	Rect ScreenToRect(float margin){
		Camera cam = Camera.main;
		Vector3 bottomLeft = cam.ScreenToWorldPoint (Vector3.zero);
		Vector3 topRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,0)); 
		Rect rect = Rect.MinMaxRect (bottomLeft.x, bottomLeft.y, topRight.x, topRight.y);
		rect.x += margin;
		rect.y += margin;
		rect.width -= margin * 2;
		rect.height -= margin * 2;
		return rect;
	}
}
