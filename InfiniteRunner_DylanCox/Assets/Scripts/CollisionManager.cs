using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour {

	GameObject Player;
	int playerMask;

	CollisionSphere[] collisionSpheres;
	CollisionSphere feet;
	CollisionSphere head;

	class CollisionSphere {

		public Vector3 offset;
		public float radius;

		public CollisionSphere( Vector3 offset, float radius){
			this.offset = offset;
			this.radius = radius;
		} 

		public static bool operator >(CollisionSphere lhs, CollisionSphere rhs) {
			return lhs.offset.y > rhs.offset.y;
		}

		public static bool operator <(CollisionSphere lhs, CollisionSphere rhs) {
			return lhs.offset.y > rhs.offset.y;
		}
	}

	class CollisionSphereComparer : IComparer {
		public int Compare(object a, object b) {
			if (!(a is CollisionSphere) || !(b is CollisionSphere)) { 
				Debug.LogError(Environment.StackTrace);
				throw new ArgumentException ("Cannot compare CollisionSpheres to non-CollisionSpheres");

			}

			CollisionSphere lhs = (CollisionSphere)a; 
			CollisionSphere rhs = (CollisionSphere)b; 

			if (lhs.offset.y < rhs.offset.y)
				return -1;
			else if (lhs.offset.y > rhs.offset.y)
				return 1;
			else
				return 0; 
		}
	}

	

	// Use this for initialization
	void Start () {

		// Get Player reference 
		Player = GameObject.Find("Robot");
		if (!Player) {
			Debug.Log ("Could not find Player");
			Destroy (this);
		} 

		// Get layer mask for obstacle collision
		playerMask = GetLayerMask((int)Layer.Obstacle);

		// Import SphereCollider components into CollisionSphere objects
		SphereCollider[] colliders = Player.GetComponents<SphereCollider>();
		collisionSpheres = new CollisionSphere[colliders.Length]; 

		for (int i = 0; i < colliders.Length; i++) {
			collisionSpheres [i] = new CollisionSphere (colliders[i].center, colliders[i].radius);
		}

		Array.Sort (collisionSpheres, new CollisionSphereComparer ());

		feet = collisionSpheres [0];
		head = collisionSpheres [collisionSpheres.Length - 1]; 
		
	}
	
	// Update is called once per frame
	void LateUpdate () {

		List<Collider> collisions = new List<Collider> (); 

		foreach (CollisionSphere s in collisionSpheres) {
			foreach (Collider c in Physics.OverlapSphere(Player.transform.position 
													+ s.offset, s.radius, playerMask)) {
				collisions.Add (c);
			}
		}

		if(collisions.Count > 0) 
			Debug.Log ("Collision!   GameObject: " + collisions [0].gameObject.name);
		
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
