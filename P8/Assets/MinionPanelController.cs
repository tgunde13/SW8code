using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using UnityEngine.UI;

public class MinionPanelController : MonoBehaviour {
	public int minionsPerPage = 9;
	public Sprite swordman;
	public Sprite spearmaiden;
	public Sprite cleric;
	public Sprite placeholder;

	private DatabaseReference userMinionsRef;
	private List<Minion> userMinions;
	private List<Minion> pickedMinions;
	private int currentPage = 1;
	string userKey;
	private int pages = 1;
	private GameObject next;
	private GameObject prev;
	private GameObject done;
	private GameObject numPickedMinions;
	private GameObject pageNum;
	private GameObject battleCanvas;
	private GameObject battlePanel;
	private FightController fight;
	private bool updateMinionPanel = false;

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
		next = gameObject.transform.parent.gameObject
			.transform.Find ("Buttons").gameObject
			.transform.Find ("Next").gameObject;
		prev = gameObject.transform.parent.gameObject
			.transform.Find ("Buttons").gameObject
			.transform.Find("Prev").gameObject;
		pageNum = gameObject.transform.Find ("Page").gameObject;
		done = gameObject.transform.parent.gameObject
			.transform.Find ("Buttons").gameObject
			.transform.Find("Done").gameObject;
		numPickedMinions = gameObject.transform.Find ("Picked minions").gameObject;
		battleCanvas = gameObject.transform.parent.gameObject
			.transform.parent.gameObject
			.transform.Find ("Battle Minions").gameObject;
		battlePanel = battleCanvas.transform.Find ("Battle Panel").gameObject;
		fight = battleCanvas.GetComponent<FightController> ();
		GetMinions ();
	}

	void Update () {
		if (updateMinionPanel) {
			updateMinionPanel = false;
			RemoveMinionsFromPanel ();
			AddMinionsToPanel ();
			SetButtonStatus ();
			SetPageNum ();
		}
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

		Minion m = new Minion (minionKey, health, level, name, power, speed, type, xp);
		userMinions.Add (m);
	}

	/// <summary>
	/// Retrives the minions of the current user, then calls PopulateMinionList on each minion
	/// </summary>
	void GetMinions(){
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
		int firstPageMinions;

		if (numMinions > minionsPerPage) {
			firstPageMinions = minionsPerPage;
			if (numMinions % minionsPerPage != 0) {
				pages = (numMinions / minionsPerPage) + 1;
			} else {
				pages = numMinions / minionsPerPage;
			}
			next.SetActive (true);
			pageNum.GetComponent<Text> ().text = "Page 1/" + pages;
		} else {
			firstPageMinions = numMinions;
		}

		for (int i = 0; i < firstPageMinions; i++) {
			minionGameObjectName = "Minion " + (i + 1);
			minion = gameObject.transform.Find (minionGameObjectName).gameObject;
			minion.SetActive (true);
			minion.GetComponent<OnClickMinionPicker> ().minion = userMinions [i];
			minion.GetComponent<Image> ().sprite = SelectSprite(userMinions[i].getName());
		}
	}

	/// <summary>
	/// Selects the sprite, from the name of the minion.
	/// </summary>
	/// <returns>The sprite related to the Name.</returns>
	/// <param name="name">Name of the minion.</param>
	Sprite SelectSprite(string name){
		switch (name) {
		case "Swordman":
			return swordman;
		case "Spearman":
			return spearmaiden;
		case "Cleric":
			return cleric;
		default:
			return placeholder;
		}
	}

	public void NextPressed(){
		currentPage++;
		Debug.Log ("Next recived");
		updateMinionPanel = true;
	}

	public void PrevPressed(){
		currentPage--;
		updateMinionPanel = true;
	}

	public void DonePressed(){
		battlePanel.SetActive (true);
		foreach (Minion m in pickedMinions) {
			fight.playerMinions.Add (m);
		}
		fight.StartFight ();
		Destroy (gameObject.transform.parent.gameObject);
	}

	void RemoveMinionsFromPanel(){
		string minionGameObjectName = "";
		GameObject minion;
		GameObject border;
		for (int i = 1; i <= 9; i++) {
			Debug.Log ("RemoveMinionsFromPanel: " + i);
			minionGameObjectName = "Minion " + i;
			minion = gameObject.transform.Find (minionGameObjectName).gameObject;
			border = minion.transform.Find ("Border").gameObject;
			if (border.activeSelf) {
				border.SetActive (false);
			}
			minion.SetActive (false);
		}
	}

	void AddMinionsToPanel(){
		string minionGameObjectName = "";
		GameObject minion;
		GameObject border;
		int minionGameObjects = 1;
		int minionOffset = (currentPage - 1) * minionsPerPage;
		for (int i = minionOffset; i < minionsPerPage + minionOffset; i++) {
			if (userMinions.Count <= i) {
				break;
			}
			Debug.Log ("Minion num " + i + " being added");
			minionGameObjectName = "Minion " + minionGameObjects;
			minion = gameObject.transform.Find (minionGameObjectName).gameObject;
			border = minion.transform.Find ("Border").gameObject;
			minion.SetActive (true);
			minion.GetComponent<OnClickMinionPicker> ().minion = userMinions [i];
			minion.GetComponent<Image> ().sprite = SelectSprite(userMinions[i].getName());
			if (pickedMinions.Count != 0) {
				if (pickedMinions.Contains (userMinions [i])) {
					border.SetActive (true);
				}
			}
			minionGameObjects++;
		}
	}

	void SetButtonStatus(){
		if (currentPage == 1) {
			prev.SetActive (false);
		} else {
			prev.SetActive (true);
		}
			
		if (userMinions.Count > currentPage * minionsPerPage) {
			next.SetActive (true);
		} else {
			next.SetActive (false);
		}
	}

	void SetPageNum(){
		pageNum.GetComponent<Text> ().text = "Page " + currentPage + "/" + pages;
	}

	private bool AddMinionSelection(Minion m){
		if (pickedMinions.Count < 3) {
			pickedMinions.Add (m);
			Debug.Log ("Added minion: " + m.ToString ());
			if (!done.activeSelf) {
				done.SetActive (true);
			}
			numPickedMinions.GetComponent<Text> ().text = "Picked " + pickedMinions.Count + "/" + 3 + " Minions";
			return true;
		}
		return false;
	}

	public bool RemoveMinionSelection(Minion m){
		if (pickedMinions.Contains (m)) {
			pickedMinions.Remove (m);
			Debug.Log ("Removed minion: " + m.ToString ());
			if (pickedMinions.Count == 0) {
				done.SetActive (false);
			}
			numPickedMinions.GetComponent<Text> ().text = "Picked " + pickedMinions.Count + "/" + 3 + " Minions";
			return true;
		} else {
			return AddMinionSelection (m);
		}
	}
}
