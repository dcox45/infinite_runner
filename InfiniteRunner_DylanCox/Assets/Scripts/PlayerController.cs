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
		StartCoroutine(TestCoroutine()); 
		
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

	IEnumerator TestCoroutine() {
		
		Debug.Log("wait for 2 seconds");
		yield return new WaitForSeconds (2);  //resume from this point in 2 seconds in real time
			
		Debug.Log("Thanks!  3 more seconds");
		yield return new WaitForSeconds (3);  //resume from this point in 2 seconds in real time

		Debug.Log("Have some more Factorials: ");
		for (int i = 2; i < 10; i++) {
			int factorial = i; 
			for (int j = i; j > 1; j--) {// this for loop finishes all in one frame 
				factorial *= j; 
			}

			int frameCount = 2 - i;
			Debug.Log("Frame" + frameCount + ": Factorial of " + i + " is " + factorial); 
			yield return null; // resume from this point next frame
		}

		Debug.Log("TestCoroutine is done now.  Bye!"); 
	}


}
