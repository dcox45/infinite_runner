using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour {

	int playerMask;

	// Use this for initialization
	void Start () {

		// Get layer mask for obstacle collision
		playerMask = GetLayerMask((int)Layer.Obstacle);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	int GetLayerMask(params int[]indices) {

		int mask = 0; 

		for (int i = 0; i < indices.Length; i++) {
			mask |= 1 << indices [i];
		}
		return mask;
	}

	int GetLayerIgnoreMask(params int[] indices) {
		return ~GetLayerIgnoreMask (indices);
	}

	// HELPERS 
	void AddLayers(ref int mask, params int[] indices) {
		mask |= GetLayerMask (indices);
	}

	void RemoveLayers(ref int mask, params int[] indices) {
		mask &= ~GetLayerMask (indices);
	}
}
