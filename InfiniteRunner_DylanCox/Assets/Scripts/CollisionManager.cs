using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour {

	GameObject Player;
	int playerMask;

	SkinnedMeshRenderer rend; 
	Animator PlayerAnim; 
	int SlideParam; 

	CollisionSphere[] collisionSpheres;
	CollisionSphere feet;
	CollisionSphere head;

	Vector3[] collisionSphereSlidePositions;

	bool invincible = false; 

	// Inspector parameters 
	[SerializeField]
	float blinkRate = 0.2f;

	[SerializeField]
	float blinkTime = 3f; 

	[SerializeField]
	bool debugSpheres = false; 

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

		rend = Player.GetComponentInChildren<SkinnedMeshRenderer> ();
		if (!Player) {
			Debug.Log ("Could not find Player");
			Destroy (this);
		} 

		PlayerAnim = Player.GetComponent<Animator> ();
		if (!PlayerAnim) {
			Debug.LogError ("Animator Component not found on Player!");  
			Destroy (this);
		}

		SlideParam = Animator.StringToHash ("SlideCurve");

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

		// Positions of CollisionSpheres mid-slide 
		collisionSphereSlidePositions = new Vector3[4];
		collisionSphereSlidePositions [0] = new Vector3 (0f, 0f, 0.75f);
		collisionSphereSlidePositions [1] = new Vector3 (0f, 0.25f, 0.25f);
		collisionSphereSlidePositions [2] = new Vector3 (0f, 0.55f, -0.15f);
		collisionSphereSlidePositions [3] = new Vector3 (0.4f, 0.73f, 0.2f);

		
	}
	
	// Update is called once per frame
	void LateUpdate()
	{
		List<Collider> collisions = new List<Collider>();

		for (int i = 0; i < collisionSpheres.Length; i++)
		{
			// Get vector that moves CollisionSphere to its final slide position
			Vector3 SlideDisplacement = collisionSphereSlidePositions[i] - collisionSpheres[i].offset;

			// Scale displacement by animation curve
			SlideDisplacement *= PlayerAnim.GetFloat(SlideParam);

			// Apply slide displacement to CollisionSphere's offset
			Vector3 offset = collisionSpheres[i].offset + SlideDisplacement;

			foreach (Collider c in Physics.OverlapSphere(Player.transform.position + offset,
				collisionSpheres[i].radius, playerMask))
			{
				collisions.Add(c);
			}
		}

		if (collisions.Count > 0)
			ObstacleCollision();
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

	void ObstacleCollision() {
		if (!invincible) {
			invincible = true;
			StartCoroutine (BlinkPlayer ());
		}
	}

	IEnumerator BlinkPlayer() {

		float startTime = Time.time;

		while (true) {
			// Toggle visbility by disabling mesh 
			rend.enabled = !rend.enabled;

			// Check if blink period has expired
			if (Time.time >= startTime + blinkTime) {
				rend.enabled = true;
				invincible = false;
				break; 
			}
			yield return new WaitForSeconds (blinkRate);
		}

	}

	void OnDrawGizmos()
	{
		if (!Application.isPlaying)
			return;

		if (!debugSpheres)
			return;

		for (int i = 0; i < collisionSpheres.Length; i++)
		{
			// Get vector that moves CollisionSphere to its final slide position
			Vector3 SlideDisplacement = collisionSphereSlidePositions[i] - collisionSpheres[i].offset;

			// Scale displacement by animation curve
			SlideDisplacement *= PlayerAnim.GetFloat(SlideParam);

			// Apply slide displacement to CollisionSphere's offset
			Vector3 offset = collisionSpheres[i].offset + SlideDisplacement;

			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(Player.transform.position + offset, collisionSpheres[i].radius);
		}
	}



}


//SlideParam
