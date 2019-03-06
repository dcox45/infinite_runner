using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour {
	GameObject [] LoadedObstacles;


	// Use this for initialization
	void Awake () {

		//load obstacle prefabs
		LoadedObstacles = Resources.LoadAll<GameObject>("Obstacles");

	}
	
	// Update is called once per frame


	void PlaceObstacles(GameObject Piece) {
		Debug.Log ("New Piece added.  We should probably put some obstacles on it");
	}
}
