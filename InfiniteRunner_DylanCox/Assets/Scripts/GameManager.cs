﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

	RoadManager rm; 
	CollisionManager cm;
	ScoreKeeper sk;
	PlayerController pc; 

	// Use this for initialization
	void Start () {

		rm = GetComponent<RoadManager> ();
		cm = GetComponent<CollisionManager> ();
		sk = GetComponent<ScoreKeeper> ();
		pc = PlayerController.Instance; 

		cm.OnObstacleCollision += PauseGamePlay;
		
	}

	public void ResetLevel() {

		sk.CloseLeaderboard ();
		rm.Reset ();
		pc.Reset ();
		cm.enabled = true; 

	}

	public void PauseGamePlay() {
		pc.gameObject.SetActive (false);
		rm.enabled = false;
		cm.enabled = false;  
	}
}
