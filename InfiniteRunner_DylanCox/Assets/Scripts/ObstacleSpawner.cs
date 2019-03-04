using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour {
	GameObject [] LoadedObstacles;

	// Use this for initialization
	void Start () {

		//load obstacle prefabs
		LoadedObstacles = Resources.LoadAll<GameObject>("Obstacles");

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
