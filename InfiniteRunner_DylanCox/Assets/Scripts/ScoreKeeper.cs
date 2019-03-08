using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(CollisionManager))]
public class ScoreKeeper : MonoBehaviour {

	double score; 

	// UI elements
	[SerializeField]
	Text scoreText; 

	[SerializeField]
	int scoreRate = 250; // Points per second




	// Use this for initialization
	void Start () {

		// Event initialization

		score = 0.0;

		// Subscribe game reset code to obstacle collision event 
		GetComponent<CollisionManager>().OnObstacleCollision += DisplayLeaderboard; 

		//double d = double.MaxValue;
		//Debug.Log("Double to int: " + (int)d);
		//Debug.Log ("Double to ulong " + (ulong)d);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		score += Time.deltaTime * scoreRate;

		scoreText.text = "Score:  " + (ulong)score; 
	
			
	}

	void DisplayLeaderboard(){

	}
}
