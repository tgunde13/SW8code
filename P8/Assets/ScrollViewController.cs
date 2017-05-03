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
		//PopulatePanel ();
	}

	void PopulateMinionList(object sender, ChildChangedEventArgs args){
		string key = (string)args.Snapshot.Key;
		long health = (long)args.Snapshot.Child ("health").GetValue (false);
		long level = (long)args.Snapshot.Child ("level").GetValue (false);
		string name = (string)args.Snapshot.Child ("name").GetValue (false);
		long power = (long)args.Snapshot.Child ("power").GetValue (false);
		long speed = (long)args.Snapshot.Child ("speed").GetValue (false);
		string type = (string)args.Snapshot.Child ("type").GetValue (false);
		long xp = (long)args.Snapshot.Child ("xp").GetValue (false);

		Debug.Log ("Found minion: " + name);
		Minion m = new Minion (key, (int)health, (int)level, name, (int)power, (int)speed, type, (int)xp);
		userMinions.Add (m);
		numMinions++;
	}

	public void GetMinions(){
		Debug.Log ("get minions reached");
		userMinionsRef.GetValueAsync ().ContinueWith(task => {
			Debug.Log("Started to get data");
			if (task.IsFaulted) {
				Debug.Log("Failed to get Minions from User ID: " + userKey);
			}
			else if (task.IsCompleted) {
				DataSnapshot snapshot = task.Result;
				Debug.Log("Snapshot has: " + snapshot.ChildrenCount + " Children");
			}
		});
	}

	void PopulatePanel(){ //Firebase is too slow
		Debug.Log ("Number minons: " + numMinions);
		if (numMinions > 9) {
			pages = numMinions / 9;
			next.gameObject.SetActive (true);
			pageNum.text = "Page 1/" + pages;
		} else {
			for (int i = 1; i <= numMinions; i++) {
				Debug.Log (gameObject.transform.GetChild (2 + i).gameObject.name);
				gameObject.transform.GetChild (2 + i).gameObject.SetActive(true);
			}
		}
		AddUserMinions ();
	}

	void AddUserMinions(){
		for (int i = 0; i < numMinions; i++) {
			 
		}
	}

}
