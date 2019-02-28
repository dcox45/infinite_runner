using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoadManager : MonoBehaviour {

	GameObject[] LoadedPieces;
	List<GameObject> RoadPieces;

	Transform BeginLeft, BeginRight, EndLeft, EndRight;

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


		for (int i = 2; i < numPieces; i++) {
			AddPiece ();
		}

		RoadPieces [0].transform.parent = RoadPieces [1].transform;

		// Move road to pass first piece 
		float halfLength = (RoadPieces [0].transform.FindChild ("BeginLeft").position -
		                   RoadPieces [0].transform.FindChild ("EndLeft").position).magnitude / 2;
		RoadPieces[1].transform.Translate (0f, 0f, -halfLength, Space.World); 

		// Get corner markers of current piece 
		BeginLeft = RoadPieces[1].transform.FindChild("BeginLeft");
		BeginRight = RoadPieces[1].transform.FindChild("BeginRight");
		EndLeft = RoadPieces[1].transform.FindChild("EndLeft");
		EndRight = RoadPieces[1].transform.FindChild("EndRight");


	}

	// Update is called once per frame
	void Update () {
		RoadPieces [1].transform.Translate (0f, 0f, -speed * Time.deltaTime, Space.World);
		//RoadPieces [1].transform.Rotate (0f, 20f * Time.deltaTime, 0f, Space.World);
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
}