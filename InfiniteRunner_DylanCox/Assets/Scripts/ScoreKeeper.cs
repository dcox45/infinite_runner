using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

[RequireComponent (typeof(CollisionManager))]
public class ScoreKeeper : MonoBehaviour {

	class Leaderboard {

		public const int numSlots = 8; 
		public LeaderboardEntry[] entries; 

		string[] defaultLeaderboard = {
			"1:CAL:150000",
			"2:VAC:120000",
			"3:SUP:120000",
			"4:NUB:120000",
			"5:LEL:120000",
			"6:BAD:120000",
			"7:PLS:120000",
			"8:NOP:120000",

		};

		public Leaderboard(){
			entries = new LeaderboardEntry[numSlots];
			try {
				Load();
			}
			catch(FileNotFoundException e) {
				Debug.LogError("Leaderboard data not found.  Reverting to default"); 
				Debug.LogException(e); 
				Wipe(); 

			}
			catch(System.IndexOutOfRangeException e) {
				Debug.LogError("Leaderboard data is corrupted.  Reverting to default"); 
				Debug.LogException(e); 
				Wipe(); 
			}
			catch(System.FormatException e) {
				Debug.LogError("Leaderboard data is corrupted.  Reverting to default"); 
				Debug.LogException(e); 
				Wipe(); 
			}
			catch(System.ArgumentException e) {
				Debug.LogError("Leaderboard data is corrupted.  Reverting to default"); 
				Debug.LogException(e); 
				Wipe(); 
			}
		}

		public struct LeaderboardEntry {
			public string initials;
			public ulong score; 

			public LeaderboardEntry(string initials, ulong score){
				this.initials = initials;
				this.score = score; 
			}
		}

	}

	Leaderboard leaderboard;

	double score; 
	bool pauseScore = false;

	// UI elements
	[SerializeField]
	Text scoreText; 

	[SerializeField]
	int scoreRate = 250; // Points per second




	// Use this for initialization
	void Start () {

		// Event initialization

		score = 0.0;

		// Subscribe game reset code to obstacle collision event 
		GetComponent<CollisionManager>().OnObstacleCollision += DisplayLeaderboard; 

		//double d = double.MaxValue;
		//Debug.Log("Double to int: " + (int)d);
		//Debug.Log ("Double to ulong " + (ulong)d);

		// Leaderboard initialization
		leaderboard = new Leaderboard(); 
	}
	
	// Update is called once per frame
	void LateUpdate () {
		score += Time.deltaTime * scoreRate;

		scoreText.text = "Score:  " + (ulong)score; 
	
			
	}

	void DisplayLeaderboard(){
		// Check leaderboard for high-score
		if (score > leaderboard.entries [Leaderboard.numSlots - 1].score) {
			int curr = Leaderboard.numSlots - 1;
			while (--curr >= 0) {
				if (score <= leaderboard.entries [curr].score) {
					break;
				}
			}
			++curr;

			// Shift leaderboard down for new entry
		}
	}

	public void Load() { 
		//load text save file
		string[] loadedEntries = File.ReadAllLines(Application.dataPath + "/_data/leaderboard_data.sav");

		for (int i = 0; i < loadedEntries.Length; i++) {
			// Components of an entry are separated by ':' 
			string[] loadedItemProperties = loadedEntries[i].Split(':');

			// ==========================================
			//LOADED ITEM PROPERTIES:
			// Place:Initials:Score
			//[0] = Place, [1] = Inititals, [2] = score
			// ==========================================

			int place = int.Parse (loadedItemProperties [0]);
			string initials = loadedItemProperties [1];
			ulong score = ulong.Parse (loadedItemProperties [2]);

			//entries [place - 1] = new LeaderboardEntry (initials, score);  
		}
	}

	void Wipe() {
		File.WriteAllLines (Application.dataPath + "/_data/leaderboard_data.sav", defaultLeaderboard); 

		try {
			Load();
		}
		catch (System.Exception e) {
			Debug.LogError("Could not revert to default leaderboard data.  UH OHHHHH"); 
			Debug.LogException(e); 
		}
	}

	public void Save() {
		string[] data = new string[leaderboard.entries.Length];
		for (int i = 0; i < leaderboard.entries.Length; i++) {
			data [i] = "" + (i + 1) + ":" + leaderboard.entries [i].initials + ":" + leaderboard.entries [i].score;
		}
		File.WriteAllLines (Application.dataPath + "/_data/leaderboard_data.sav", data);
	}
}
