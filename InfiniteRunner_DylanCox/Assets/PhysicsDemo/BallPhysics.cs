using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour {

	public float speed = 20; 
	Rigidbody rb; 
	Transform tCam;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		tCam = GameObject.Find ("Main Camera").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		float boost = 1f; 
		Vector3 forward = tCam.forward;
		Vector3 right = tCam.right;

		if (Input.GetKey (KeyCode.Space)) {
			boost = 2f;
		}

		if (Input.GetKey (KeyCode.UpArrow)) {
			rb.AddForce (forward * speed * boost);
		} 
		if (Input.GetKey (KeyCode.DownArrow)) {
			rb.AddForce (-forward * speed * boost);
		} 
		if (Input.GetKey (KeyCode.LeftArrow)) {
			rb.AddForce (-right * speed * boost);
		} 
		if (Input.GetKey (KeyCode.RightArrow)) {
			rb.AddForce (right * speed * boost);
		} 
	}
}
