using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public class ScrollViewController : MonoBehaviour {
	private DatabaseReference userMinionsRef;
	public List<Minion> userMinions;

	// Use this for initialization
	IEnumerator Start () {
		string userKey = FirebaseAuthHandler.getUserId ();
		userMinionsRef = FirebaseDatabase.DefaultInstance.GetReference ("players").Child (userKey).Child ("minions");
		userMinionsRef.ChildAdded += PopulateMinionList;
		yield return new WaitForSeconds (1f);
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
		userMinions.Add (new Minion (key, (int)health, (int)level, name, (int)power, (int)speed, type, (int)xp));
	}

	void PopulateScrollView(){
		float scrollViewSize = Mathf.Floor(userMinions.Count / 6f) + 1f;
		Debug.Log (scrollViewSize);
		GetComponent<RectTransform> ().sizeDelta.y = 255f * scrollViewSize;
	}

}
