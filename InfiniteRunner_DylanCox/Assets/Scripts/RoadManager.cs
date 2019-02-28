using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour {
	GameObject[] LoadedPieces;
	// Use this for initialization
	void Start () {
		LoadedPieces = Resources.LoadAll<GameObject>("RoadPieces");

		for (int i = 0; i < LoadedPieces.Length; i++) {

			Instantiate (LoadedPieces [i]);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
