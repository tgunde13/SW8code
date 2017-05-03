using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using UnityEngine.UI;

public class ScrollViewController : MonoBehaviour {
	private DatabaseReference userMinionsRef;
	public List<Minion> userMinions;
	private int numMinions = 0;
	string userKey;
	private int pages = 1;
	public Button next;
	public Button prev;
	public Text pageNum;

	// Use this for initialization
	void Start () {
		userKey = FirebaseAuthHandler.getUserId ();
		if (userKey == null) {
			Debug.Log ("No User ID found");
			userKey = "T8kaWa5TaATk4FBosabRPhmUZz13";
		}
		userMinionsRef = FirebaseDatabase.DefaultInstance.GetReference ("players").Child (userKey).Child ("minions");
		userMinions = new List<Minion> ();
		GetMinions ();
	}

	/// <summary>
	/// Populates the minion list.
	/// </summary>
	/// <param name="minionKey">Minion key.</param>
	/// <param name="minion">Minion as a Dictionary containing all fields.</param>
	void PopulateMinionList(string minionKey, Dictionary<string, object> minion){
		object obj = 0;
		minion.TryGetValue("health", out obj);
		int health = Int32.Parse(obj.ToString());

		minion.TryGetValue ("level", out obj);
		int level = Int32.Parse (obj.ToString ());

		minion.TryGetValue ("name", out obj);
		string name = obj.ToString ();

		minion.TryGetValue ("power", out obj);
		int power = Int32.Parse (obj.ToString ());

		minion.TryGetValue ("speed", out obj);
		int speed = Int32.Parse (obj.ToString ());

		minion.TryGetValue ("type", out obj);
		string type = obj.ToString ();

		minion.TryGetValue ("xp", out obj);
		int xp = Int32.Parse (obj.ToString ());

		//Debug.Log ("Found minion: " + name);
		Minion m = new Minion (minionKey, health, level, name, power, speed, type, xp);
		userMinions.Add (m);
		numMinions++;
		Debug.Log ("Added minion: " + m.ToString ());
	}

	/// <summary>
	/// Retrives the minions of the current user, then calls PopulateMinionList on each minion
	/// </summary>
	public void GetMinions(){
		Debug.Log ("get minions reached");
		userMinionsRef.GetValueAsync ().ContinueWith(task => {
			Debug.Log("Started to get data");
			if (task.IsFaulted) {
				Debug.Log("Failed to get Minions from User ID: " + userKey);
			}
			else if (task.IsCompleted) {
				//Objects from the list is Dictionaries, but must be cast from objects
				Debug.Log("Done getting data");
				DataSnapshot snapshot = task.Result;
				Dictionary<string, object> minions = new Dictionary<string, object>();
				minions = (Dictionary<string, object>)snapshot.GetValue(false);
				foreach(KeyValuePair<string, object> entry in minions){
					Debug.Log("Found minion with key: " + entry.Key);
					Dictionary<string, object> value = (Dictionary<string, object>)entry.Value;
					PopulateMinionList(entry.Key, value);
				}
			}
		});
	}

	void AddUserMinions(){
		for (int i = 0; i < numMinions; i++) {
			 
		}
	}

}
