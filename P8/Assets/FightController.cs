using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using UnityEngine.UI;

public class FightController : MonoBehaviour {
	public List<Minion> playerMinions;
	public List<Minion> opponentMinions;
	public Sprite swordman;
	public Sprite spearmaiden;
	public Sprite cleric;
	public Sprite placeholder;
	public DialogPanel dialogPanel;

	private bool updatingBattleState = false;
	private bool IsEnviormentBattle = false;
	private GameObject computerGameobject;
	private int playerMinionNum;
	private GameObject battlePanel;
	private GameObject playerMinionOne;
	private GameObject playerMinionTwo;
	private GameObject playerMinionThree;
	private GameObject opponentMinionOne;
	private GameObject opponentMinionTwo;
	private GameObject opponentMinionThree;
	private GameObject spriteCanvas;
	private GameObject progressIndicator;
	private GameObject pickMinions;
	private Request startBattle;
	private TaskIndicator taskIndicator;
	private Dictionary<string, object> requestData;
	private Dictionary<string, object> zoneData;
	private Dictionary<string, object> minionKeys;
	private Dictionary<string, object> minion1Moves;
	private Dictionary<string, object> minion2Moves;
	private Dictionary<string, object> minion3Moves;
	private string computerMinionKey = "";
	private Squad computerSquad;
	private int latIndex;
	private int lonIndex;
	private string battleKey;
	private DatabaseReference userRef; //Used first turn to add minions to player
	private DatabaseReference minion1Ref; //Used after first turn to ref minion 1
	private DatabaseReference minion2Ref; //Used after first turn to ref minion 2
	private DatabaseReference minion3Ref; //Used after first turn to ref minion 3
	private DatabaseReference stateRef; //Listen to state

	// Use this for initialization
	void Start () {
		//--------------initialization of Dictionaries for setting values--------------//
		minionKeys = new Dictionary<string, object>();
		minion1Moves = new Dictionary<string, object> ();
		minion2Moves = new Dictionary<string, object> ();
		minion3Moves = new Dictionary<string, object> ();


		//--------------initialization of minion lists--------------//
		playerMinions = new List<Minion> ();
		opponentMinions = new List<Minion> ();


		//--------------initialization of gameobjects--------------//
		battlePanel = gameObject.transform.Find ("Battle Panel").gameObject;
		playerMinionOne = battlePanel.transform.Find ("Minion 1").gameObject;
		playerMinionTwo = battlePanel.transform.Find ("Minion 2").gameObject;
		playerMinionThree = battlePanel.transform.Find ("Minion 3").gameObject;
		spriteCanvas = gameObject.transform.Find ("Minion Sprites").gameObject;


		//--------------related to request--------------//
		//initialization
		pickMinions = gameObject.transform.parent.gameObject
			.transform.Find ("Pick Minions").gameObject;
		pickMinions.SetActive (false);
		computerGameobject = GameObject.FindGameObjectWithTag ("Minion");
		requestData = new Dictionary<string, object> ();
		zoneData = new Dictionary<string, object> ();
		progressIndicator = GameObject.Find ("ProgressCircle");
		taskIndicator = new TaskIndicator (progressIndicator);

		//seting values for request
		if (computerGameobject != null) {
			IsEnviormentBattle = true;
			computerMinionKey = computerGameobject.name;
			computerSquad = computerGameobject.GetComponent<SpriteOnClick> ().squad;
			computerGameobject.SetActive (false);
			latIndex = (int)Math.Floor ((computerSquad.getPos ().x) * 100f);
			lonIndex = (int)Math.Floor ((computerSquad.getPos ().y) * 100f);
			zoneData.Add ("latIndex", latIndex);
			zoneData.Add ("lonIndex", lonIndex);
			requestData.Add ("code", Constants.RequestCodeSoloBattle);
			requestData.Add ("zone", zoneData);
			requestData.Add ("key", computerMinionKey);
		} else {
			Debug.Log ("WARNING DEBUG CODE BEING USED");
			IsEnviormentBattle = true;
			zoneData.Add ("latIndex", 5700);
			zoneData.Add ("lonIndex", 999);
			requestData.Add ("code", Constants.RequestCodeSoloBattle);
			requestData.Add ("zone", zoneData);
			requestData.Add ("key", "debug");
		}

		//starts the request
		startBattle = new Request(this, taskIndicator, dialogPanel, StartServerFight, requestData);
		startBattle.Start ();
	}

	/// <summary>
	/// Starts the server fight.
	/// </summary>
	/// <returns><c>true</c>, if server fight was started, <c>false</c> otherwise.</returns>
	/// <param name="snapshot">Snapshot.</param>
	bool StartServerFight(DataSnapshot snapshot){
		Debug.Log ("Response to request recived");
		long returnKey = (long)snapshot.Child ("code").GetValue(false);

		switch (returnKey) {
		case Constants.HttpOk:
			battleKey = (string)snapshot.Child ("data").GetValue (false);
			Debug.Log ("Started battle with key: " + battleKey);
			SetDatabaseRef ();
			pickMinions.SetActive (true);
			taskIndicator.OnEnd ();
			return true;
		case Constants.HttpNotFound:
			Debug.Log ("Minion not found");
			taskIndicator.OnEnd ();
			return true;
		default:
			Debug.Log ("Error in reponse");
			return false;
		}
	}

	/// <summary>
	/// Sets the database references, that can be set before minions are chosen.
	/// </summary>
	void SetDatabaseRef (){
		userRef = FirebaseDatabase.DefaultInstance.GetReference ("battles")
			.Child (battleKey)
			.Child("chosenMoves")
			.Child("moves")
			.Child(FirebaseAuthHandler.getUserId());
		stateRef = FirebaseDatabase.DefaultInstance.GetReference ("battles")
			.Child (battleKey)
			.Child ("state");
		stateRef.ChildChanged += handleChildChanged;
	}

	/// <summary>
	/// Event handler for state changed.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="args">Arguments.</param>
	void handleChildChanged(object sender, ChildChangedEventArgs args){
		
	}

	/// <summary>
	/// Sets the minion database references.
	/// </summary>
	void SetMinionDatabaseRefs(){
		switch(playerMinions.Count){
		case 1: 
			minion1Ref = userRef.Child(playerMinions[0].getName());
			break;
		case 2: 
			minion1Ref = userRef.Child(playerMinions[0].getName());
			minion2Ref = userRef.Child(playerMinions[1].getName());
			break;
		case 3:
			minion1Ref = userRef.Child(playerMinions[0].getName());
			minion2Ref = userRef.Child(playerMinions[1].getName());
			minion3Ref = userRef.Child(playerMinions[2].getName());
			break;
		default:
			Debug.Log ("Found no or more than 3 minions");
			break;
		}
	}
		
	/// <summary>
	/// Sets the player minions, and processes the first turn.
	/// </summary>
	public void SetPlayerMinions(){
		playerMinionNum = playerMinions.Count;
		string minionName = "";

		if (playerMinionNum > 0) {
			playerMinionOne.SetActive (true);
			playerMinionOne.transform.Find ("Health").gameObject.GetComponent<Text> ().text = 
				playerMinions [0].getHealth () + " / " + playerMinions [0].getHealth ();
			minion1Moves.Add ("minionKey", playerMinions [0].getKey ());
			minion1Moves.Add ("avatarKey", FirebaseAuthHandler.getUserId ());
			InsertSprite (playerMinions [0], 1, true);
			userRef.Child("minion-0").SetValueAsync (minion1Moves).ContinueWith(task => {
				Debug.Log("Started to set data");
				if (task.IsFaulted) {
					Debug.Log("Failed to set minion 1 in battle");
				}
				else if (task.IsCompleted) {
					Debug.Log("Minion 1 set correctly");
				}
			});
			if (playerMinionNum > 1) {
				playerMinionTwo.SetActive (true);
				playerMinionTwo.transform.Find ("Health").gameObject.GetComponent<Text> ().text = 
					playerMinions [1].getHealth () + " / " + playerMinions [1].getHealth ();
				minion2Moves.Add ("minionKey", playerMinions [1].getKey ());
				minion2Moves.Add ("avatarKey", FirebaseAuthHandler.getUserId ());
				InsertSprite (playerMinions [1], 2, true);
				userRef.Child("minion-1").SetValueAsync (minion2Moves).ContinueWith(task => {
					Debug.Log("Started to set data");
					if (task.IsFaulted) {
						Debug.Log("Failed to set minion 2 in battle");
					}
					else if (task.IsCompleted) {
						Debug.Log("Minion 2 set correctly");
					}
				});
				if (playerMinionNum > 2) {
					playerMinionThree.SetActive (true);
					playerMinionThree.transform.Find ("Health").gameObject.GetComponent<Text> ().text = 
						playerMinions [2].getHealth () + " / " + playerMinions [2].getHealth ();
					minion3Moves.Add ("minionKey", playerMinions [2].getKey ());
					minion3Moves.Add ("avatarKey", FirebaseAuthHandler.getUserId ());
					InsertSprite (playerMinions [2], 3, true);
					userRef.Child("minion-2").SetValueAsync (minion3Moves).ContinueWith(task => {
						Debug.Log("Started to set data");
						if (task.IsFaulted) {
							Debug.Log("Failed to set minion 3 in battle");
						}
						else if (task.IsCompleted) {
							Debug.Log("Minion 3 set correctly");
						}
					});
				}
			}
		} else {
			Debug.Log ("Did not find any user minions");
		}
		SetMinionDatabaseRefs ();
	}

	/// <summary>
	/// Inserts a sprite based on the parameters it was called with.
	/// </summary>
	/// <param name="minion">The minion to insert.</param>
	/// <param name="minionPos">Minion position 1-3 is allowed, where 1 is the top.</param>
	/// <param name="isPlayerMinion">If set to <c>true</c> is player minion, else it is an opponent minion.</param>
	void InsertSprite(Minion minion, int minionPos, bool isPlayerMinion){
		string minionSpriteName = "";
		GameObject sprite;

		if (isPlayerMinion) {
			minionSpriteName += "Player";
		} else {
			minionSpriteName += "Opponent";
		}
		minionSpriteName += " Minion Sprite " + minionPos;
		sprite = spriteCanvas.transform.Find (minionSpriteName).gameObject;
		sprite.SetActive (true);
		sprite.GetComponent<Image> ().sprite = SelectSprite (minion.getName ());
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
}
