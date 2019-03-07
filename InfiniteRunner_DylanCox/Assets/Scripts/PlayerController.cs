using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Animator))]
public class PlayerController : Singleton<PlayerController> {

	//float h = 0f; 

	// lane variables
	int currentLane = 0; 
	int prevLane = 0;
	public float laneWidth;
	public float LaneWidth { get { return laneWidth; } }
	Coroutine currentLaneChange;
	int laneChangeStack = 0;

	[SerializeField]
	public int numLanes = 3;
	public int NumLanes { get { return numLanes; } }

	//Input variables 
	float hPrev = 0f; 
	int dirBuffer = 0; 

	[SerializeField]
	float strafeSpeed = 5f; // speed of lane changing

	// Jump parameters
	[SerializeField]
	float g = -9.81f;

	[SerializeField]
	float Vi = 5f; 

	// Animation 
	Animator anim; 
	int jumpParam; 
	int slideParam; 

	// Use this for initialization
	void Awake () {

		transform.position = Vector3.zero; //middle lane is always at origin 
		laneWidth = 7.5f / numLanes;
//		StartCoroutine(TestCoroutine());

		// Animation initialization 
		anim = GetComponent<Animator>();
		jumpParam = Animator.StringToHash ("Jump");
		slideParam = Animator.StringToHash ("Slide"); 

		//Debug.Break();
		
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
			StartCoroutine(Jump());
		}

		// Sliding 
		if ( Input.GetButtonDown(InputNames.slideButton)) {
			anim.SetTrigger (slideParam);
		} 

	}

	void MovePlayer(int dir) {

		if (currentLaneChange != null) {
			if (currentLane + dir != prevLane) {
				dirBuffer = dir;
				return;
			}

			StopCoroutine (currentLaneChange);
			dirBuffer = 0; 


		}
		prevLane = currentLane;
		currentLane = Mathf.Clamp (currentLane + dir, numLanes / -2, numLanes / 2); 

		currentLaneChange = StartCoroutine (LaneChange ());


	}

//	IEnumerator TestCoroutine() {
//		
//		Debug.Log("wait for 2 seconds");
//		yield return new WaitForSeconds (2);  //resume from this point in 2 seconds in real time
//			
//		Debug.Log("Thanks!  3 more seconds");
//		yield return new WaitForSeconds (3);  //resume from this point in 2 seconds in real time
//
//		Debug.Log("Have some more Factorials: ");
//		for (int i = 2; i < 10; i++) {
//			int factorial = i; 
//			for (int j = i; j > 1; j--) {// this for loop finishes all in one frame 
//				factorial *= j; 
//			}
//
//			int frameCount = 2 - i;
//			Debug.Log("Frame" + frameCount + ": Factorial of " + i + " is " + factorial); 
//			yield return null; // resume from this point next frame
//		}

//		Debug.Log("TestCoroutine is done now.  Bye!"); 
//	}

	// Strafe movement coroutine
	IEnumerator LaneChange(){
		Vector3 From = Vector3.right * prevLane * laneWidth;;
		Vector3 To = Vector3.right * currentLane * laneWidth;  

		float t = (laneWidth - Vector3.Distance (transform.position.x * Vector3.right, To)) / laneWidth;
		for (; t < 1f; t += strafeSpeed * Time.deltaTime / laneWidth) {
			transform.position = Vector3.Lerp (From + Vector3.up * transform.position.y, 
												To + Vector3.up * transform.position.y, t);
			yield return null; 
		}

		transform.position = To + Vector3.up * transform.position.y;
		currentLaneChange = null; 

		if (dirBuffer != 0 && ++laneChangeStack < 2) {
			MovePlayer (dirBuffer);
			dirBuffer = 0;
		}

		laneChangeStack = 0;

	}

	// Jumping coroutine
	IEnumerator Jump() { 
				// Jump Animation
		anim.SetBool(jumpParam, true);

		// Calculate total time of jump 
		float tFinal = (Vi * 2f) / -g; 

		// Calculate transition time 
		float tLand = tFinal - 0.125f;

		float t = Time.deltaTime;

		for (;t < tLand; t += Time.deltaTime) {
			float y = g * (t * t) / 2f + Vi * t;
			Helpers.SetPositionY (transform, y);

			yield return null;
		}

		Helpers.SetPositionY (transform, 0f);

		// Transition back to run
		anim.SetBool(jumpParam, false);

		for (; t < tFinal; t += Time.deltaTime) { 
			float y = g * (t * t) / 2f + Vi * t; 
			Helpers.SetPositionY (transform, y);


			yield return null;
		}
		Helpers.SetPositionY (transform, 0f);
	}


}
