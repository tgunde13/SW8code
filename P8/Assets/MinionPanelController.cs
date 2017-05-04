using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using UnityEngine.UI;

public class MinionPanelController : MonoBehaviour {
	private DatabaseReference userMinionsRef;
	private List<Minion> userMinions;
	private List<Minion> pickedMinions;
	public int minionsPerPage = 9;
	private int currentPage = 1;
	string userKey;
	private int pages = 1;
	private GameObject next;
	private GameObject prev;
	private GameObject pageNum;

	// Use this for initialization
	void Start () {
		userKey = FirebaseAuthHandler.getUserId ();
		if (userKey == null) {
			Debug.Log ("No User ID found");
			userKey = "T8kaWa5TaATk4FBosabRPhmUZz13";
			Debug.Log ("Using User Key: " + userKey);
		}
		userMinionsRef = FirebaseDatabase.DefaultInstance.GetReference ("players").Child (userKey).Child ("minions");
		userMinions = new List<Minion> ();
		pickedMinions = new List<Minion> ();
		next = gameObject.transform.Find ("Next").gameObject;
		prev = gameObject.transform.Find ("Prev").gameObject;
		pageNum = gameObject.transform.Find ("Page").gameObject;
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
		//Debug.Log ("Added minion: " + m.ToString ());
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
				Debug.Log("Done getting data");
				DataSnapshot snapshot = task.Result;
				Dictionary<string, object> minions = new Dictionary<string, object>();
				minions = (Dictionary<string, object>)snapshot.GetValue(false);

				foreach(KeyValuePair<string, object> entry in minions){
					//Debug.Log("Found minion with key: " + entry.Key);
					Dictionary<string, object> value = (Dictionary<string, object>)entry.Value;
					PopulateMinionList(entry.Key, value);
				}
				AddUserMinions();
			}
		});
	}

	/// <summary>
	/// Adds the user minions to the Minion Panel.
	/// </summary>
	void AddUserMinions(){
		string minionGameObjectName = "";
		GameObject minion;
		int numMinions = userMinions.Count;
		for (int i = 1; i <= numMinions; i++) {
			minionGameObjectName = "Minion " + i;
			minion = gameObject.transform.Find (minionGameObjectName).gameObject;
			minion.SetActive (true);
			minion.GetComponent<OnClickMinionPicker> ().minion = userMinions [i];
			if (i == minionsPerPage) {
				break;
			}
		}

		if (numMinions > minionsPerPage) {
			if (numMinions % minionsPerPage != 0) {
				pages = (numMinions / minionsPerPage) + 1;
			} else {
				pages = numMinions / minionsPerPage;
			}
			next.SetActive (true);
			pageNum.GetComponent<Text> ().text = "Page 1/" + pages;
		}
	}

	public void NextPressed(){
		Debug.Log ("Not yet implemented");
	}

	public void PrevPressed(){
		Debug.Log ("Not yet implemented");
	}

	private void AddMinionSelection(Minion m){
		if (pickedMinions.Count < 3) {
			pickedMinions.Add (m);
			Debug.Log ("Added minion: " + m.ToString ());
		}
	}

	public void RemoveMinionSelection(Minion m){
		if (pickedMinions.Contains (m)) {
			pickedMinions.Remove (m);
			Debug.Log ("Removed minion: " + m.ToString ());
		} else {
			AddMinionSelection (m);
		}
	}
}
