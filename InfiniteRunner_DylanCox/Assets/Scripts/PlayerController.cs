using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	float h = 0f; 

	// lane variables
	int currentLane = 0; 
	int numLanes = 3;

	//Input variables 
	float hPrev = 0f; 

	// Use this for initialization
	void Awake () {

		transform.position = Vector3.zero; //middle lane is always at origin 
		
	}
	
	// Update is called once per frame
	void Update () {

		float hNew = Input.GetAxisRaw (InputNames.horzontalAxis); // returns -1, 0, or 1 with no smoothing
		float hDelta = hNew - hPrev;
		
		if (Mathf.Abs (hDelta) > 0f && Mathf.Abs(hNew) > 0f) {
			//Debug.Log ("Horizontal axis:  " + Input.GetAxis (InputNames.horzontalAxis)); 

			MovePlayer ((int)hNew);
		}
		hPrev = hNew;

		// Jumping 
		if (Input.GetButtonDown (InputNames.jumpButton)) {
			Debug.Log ("Jump Button Pressed");
		}

		// Sliding 
		if ( Input.GetButtonDown(InputNames.slideButton)) {
			Debug.Log("Slide Button Pressed)");
		} 

	}

	void MovePlayer(int dir) {
		currentLane = Mathf.Clamp (currentLane + dir, numLanes / -2, numLanes / 2);; 

		transform.position = new Vector3(currentLane, 0f, 0f);
	}

}
