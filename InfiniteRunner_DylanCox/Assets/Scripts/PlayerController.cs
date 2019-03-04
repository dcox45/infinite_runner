using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	float h = 0f; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		float hNew = Input.GetAxisRaw (InputNames.horzontalAxis); // returns -1, 0, or 1 with no smoothing
		
		if (!Mathf.Approximately (h, hNew) && !Mathf.Approximately(hNew, 0f)) {
			//Debug.Log ("Horizontal axis:  " + Input.GetAxis (InputNames.horzontalAxis)); 
			if (hNew < 0f) {
				Debug.Log ("Left Input");
			} else if (hNew > 0f) {
				Debug.Log ("Right Input");
			}

			h = hNew;



		}

		// Jumping 
		if (Input.GetButtonDown (InputNames.jumpButton)) {
			Debug.Log ("Jump Button Pressed");
		}

		// Sliding 
		if ( Input.GetButtonDown(InputNames.slideButton)) {
			Debug.Log("Slide Button Pressed)");
		} 

	}

}
