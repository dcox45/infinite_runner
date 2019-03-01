using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoadManager : MonoBehaviour {

	GameObject[] LoadedPieces;
	List<GameObject> RoadPieces;

	Transform BeginLeft, BeginRight, EndLeft, EndRight;

	Vector3 RotationPoint = Vector3.zero;

	[SerializeField]
	int numPieces = 10; 

	[SerializeField]
	string firstPieceFilename = "Straight60m";

	[SerializeField]
	float speed = 20f;

	// Use this for initialization
	void Start () {
		LoadedPieces = Resources.LoadAll<GameObject>("RoadPieces");
		RoadPieces = new List<GameObject> ();

		//Hard-code first two road pieces for consistent start of road 
		RoadPieces.Add(Instantiate(Resources.Load("RoadPieces/" + firstPieceFilename)) as GameObject);
		RoadPieces.Add(Instantiate(Resources.Load("RoadPieces/" + firstPieceFilename)) as GameObject);

		Vector3 Displacement = RoadPieces[0].transform.FindChild("EndLeft").position - RoadPieces[1].transform.FindChild("BeginLeft").position;
		RoadPieces[1].transform.Translate(Displacement, Space.World);


		for (int i = 2; i < numPieces; i++) {
			AddPiece ();
		}

		RoadPieces [0].transform.parent = RoadPieces [1].transform;

		// Move road to pass first piece 
		float halfLength = (RoadPieces [0].transform.FindChild ("BeginLeft").position -
			RoadPieces [0].transform.FindChild ("EndLeft").position).magnitude / 2;
		RoadPieces[1].transform.Translate (0f, 0f, -halfLength, Space.World); 

		SetCurrentPiece ();
		// Get corner markers of current piece 
		//BeginLeft = RoadPieces[1].transform.FindChild("BeginLeft");
	//	BeginRight = RoadPieces[1].transform.FindChild("BeginRight");
		//EndLeft = RoadPieces[1].transform.FindChild("EndLeft");
	//EndRight = RoadPieces[1].transform.FindChild("EndRight");
	}

	// Update is called once per frame
	void Update () {

		if (RoadPieces [1].tag == Tags.straightPiece) {
			RoadPieces [1].transform.Translate (0f, 0f, -speed * Time.deltaTime, Space.World);
		} else {
			float radius = Mathf.Abs(RotationPoint.x);
			float angle = ((speed * Time.deltaTime) / radius) * Mathf.Sign(RoadPieces[1].transform.localScale.x) * Mathf.Rad2Deg;
			RoadPieces[1].transform.RotateAround(RotationPoint, Vector3.up, angle);
			//RoadPieces [1].transform.RotateAround (RotationPoint, Vector3.up, -speed * Time.deltaTime);
		}
			

		//RoadPieces [1].transform.Translate (0f, 0f, -speed * Time.deltaTime, Space.World);
		//RoadPieces [1].transform.Rotate (0f, 20f * Time.deltaTime, 0f, Space.World);

		// Delete piece behind player and add to road; move Parents / Children up

		if (EndLeft.position.z < 0f || EndRight.position.z < 0f) {
			Destroy (RoadPieces [0]);
			RoadPieces.RemoveAt (0);
			AddPiece ();

			for (int i = RoadPieces.Count - 1; i >= 0; i--) {
				RoadPieces [i].transform.parent = null; 
				RoadPieces [i].transform.parent = RoadPieces[1].transform;
			}

			// Get new references for coners of current piece 
			SetCurrentPiece(); 
		}
	}

	void AddPiece() {
		int randomIndex = Random.Range (0, LoadedPieces.Length);

		RoadPieces.Add (Instantiate (LoadedPieces [randomIndex],
			RoadPieces[RoadPieces.Count - 1].transform.position,
			RoadPieces[RoadPieces.Count - 1].transform.rotation));

		// Get references to the two pieces we are processing with vector subtraction 
		Transform NewPiece = RoadPieces [RoadPieces.Count - 1].transform; 
		Transform PrevPiece = RoadPieces[RoadPieces.Count - 2].transform; 

		// Get Positions of four corner GameObjects 
		BeginLeft = NewPiece.FindChild("BeginLeft");  
		EndLeft = PrevPiece.FindChild ("EndLeft");
		BeginRight = NewPiece.FindChild ("BeginRight"); 
		EndRight = PrevPiece.FindChild ("EndRight");

		// Compute edges
		Vector3 BeginEdge = BeginRight.position - BeginLeft.position;
		Vector3 EndEdge = EndRight.position - EndLeft.position;

		//Compute angle between edges
		float angle = Vector3.Angle(BeginEdge, EndEdge) * Mathf.Sign(Vector3.Cross(BeginEdge, EndEdge).y); 

		//Rotate new piece to align with previous piece 
		NewPiece.Rotate(0f, angle, 0f, Space.World);

		// Move piece into position
		Vector3 Displacement = EndLeft.position - BeginLeft.position;
		NewPiece.Translate (Displacement, Space.World);

		NewPiece.parent = RoadPieces [1].transform; 
	}

	public Vector3 GetRotationPoint(Transform BeginLeft, Transform BeginRight, Transform EndLeft, Transform EndRight) {

		// Compute edges from corner positions
		Vector3 BeginEdge = BeginLeft.position - BeginRight.position; 
		Vector3 EndEdge = EndLeft.position - EndRight.position; 

		float a = Vector3.Dot (BeginEdge, BeginEdge); // square magnitude of begin edge 
		float b = Vector3.Dot(BeginEdge, EndEdge); // project BeginEdge onto EndEdge
		float e = Vector3.Dot(EndEdge,EndEdge); // square magnitude of end edge 

		float d = a * e - b * b;

		Vector3 r = BeginLeft.position - EndLeft.position; 
		float c = Vector3.Dot (BeginEdge, r);
		float f = Vector3.Dot (EndEdge, r); 

		float s = (b * f - c * e) / d;
		float t = (a * f - c * b) / d; 

		Vector3 RotationPointBegin = BeginLeft.position + BeginEdge * s;
		Vector3 RotationPointEnd = EndLeft.position + EndEdge * t; 

		//return midpoint between two closest points
		return (RotationPointBegin + RotationPointEnd) / 2f; 

		}

	void SetCurrentPiece() {
		BeginLeft = RoadPieces[1].transform.FindChild("BeginLeft");
		BeginRight = RoadPieces[1].transform.FindChild("BeginRight");
		EndLeft = RoadPieces[1].transform.FindChild("EndLeft");
		EndRight = RoadPieces[1].transform.FindChild("EndRight");

		RotationPoint = GetRotationPoint (BeginLeft, BeginRight, EndLeft, EndRight); 
	}
}