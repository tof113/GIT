using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonController : MonoBehaviour {

	public Gun canon;

	void OnEnable() {
		canon.SetShooting (false);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			canon.SetShooting(true);
		}
		if (Input.GetButtonUp("Fire1")) {
			canon.SetShooting (false);
		}
	}
}
