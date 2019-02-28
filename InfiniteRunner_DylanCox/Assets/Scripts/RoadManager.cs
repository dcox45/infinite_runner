using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoadManager : MonoBehaviour {
	
	GameObject[] LoadedPieces;
	List<GameObject> RoadPieces;

	[SerializeField]
	int numPieces = 10; 

	[SerializeField]
	string firstPieceFilename = "Straight60m";

	// Use this for initialization
	void Start () {
		LoadedPieces = Resources.LoadAll<GameObject>("RoadPieces");
		RoadPieces = new List<GameObject> ();

		//Hard-code first two road pieces for consistent start of road 
		RoadPieces.Add(Instantiate(Resources.Load("RoadPieces/" + firstPieceFilename)) as GameObject);
		RoadPieces.Add(Instantiate(Resources.Load("RoadPieces/" + firstPieceFilename)) as GameObject);
				

		for (int i = 2; i < numPieces; i++) {
			int randomIndex = Random.Range (0, LoadedPieces.Length);

			RoadPieces.Add (Instantiate (LoadedPieces [randomIndex],
						RoadPieces[RoadPieces.Count - 1].transform.position,
						RoadPieces[RoadPieces.Count - 1].transform.rotation));

			// Get references to the two pieces we are processing with vector subtraction 
			Transform NewPiece = RoadPieces [RoadPieces.Count - 1].transform; 
			Transform PrevPiece = RoadPieces[RoadPieces.Count - 2].transform; 

			// Get Positions of four corner GameObjects 
			Transform BeginLeft = NewPiece.FindChild("BeginLeft");  
			Transform EndLeft = PrevPiece.FindChild ("EndLeft");
			Transform BeginRight = NewPiece.FindChild ("BeginRight"); 
			Transform EndRight = PrevPiece.FindChild ("EndRight");

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


			//Instantiate (LoadedPieces [i]);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
