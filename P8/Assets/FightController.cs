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
	public string battleKey;

	private List<GameObject> playerSprites;
	private List<GameObject> opponentSprites;
	private bool updatingBattleState = false;
	private bool IsEnviormentBattle = false;
	private GameObject computerGameobject;
	private int playerMinionNum;
	private int turnCounter = -1;
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
	private GameObject turnIndicator;
	private Request startBattle;
	private TaskIndicator taskIndicator;
	private Dictionary<string, object> requestData;
	private Dictionary<string, object> zoneData;
	private string computerMinionKey = "";
	private Squad computerSquad;
	private int latIndex;
	private int lonIndex;
	private DatabaseReference stateRef; //Listen to state

	// Use this for initialization
	void Start () {
		//--------------initialization of lists--------------//
		playerMinions = new List<Minion> ();
		opponentMinions = new List<Minion> ();
		playerSprites = new List<GameObject> ();
		opponentSprites = new List<GameObject> ();


		//--------------initialization of gameobjects--------------//
		battlePanel = gameObject.transform.Find ("Battle Panel").gameObject;
		playerMinionOne = battlePanel.transform.Find ("Minion 1").gameObject;
		playerMinionTwo = battlePanel.transform.Find ("Minion 2").gameObject;
		playerMinionThree = battlePanel.transform.Find ("Minion 3").gameObject;
		spriteCanvas = gameObject.transform.Find ("Minion Sprites").gameObject;
		opponentMinionOne = battlePanel.transform.Find ("Opponent 1").gameObject;
		opponentMinionTwo = battlePanel.transform.Find ("Opponent 2").gameObject;
		opponentMinionThree = battlePanel.transform.Find ("Opponent 3").gameObject;
		turnIndicator = gameObject.transform.Find("Turn Counter").gameObject
			.transform.Find("Turn").gameObject;


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
			for (int i = 0; i < computerSquad.getSize (); i++) {
				opponentMinions.Add(new Minion(computerSquad.getKey(), 
					computerSquad.getMaxHealth(), 
					computerSquad.getLevel(), 
					computerSquad.getName(), 
					computerSquad.getPower(), 
					computerSquad.getSpeed(), 
					"Melee", 
					0));
			}
		} else {
			Debug.Log ("WARNING DEBUG CODE BEING USED");
			IsEnviormentBattle = true;
			zoneData.Add ("latIndex", 5700);
			zoneData.Add ("lonIndex", 999);
			requestData.Add ("code", Constants.RequestCodeSoloBattle);
			requestData.Add ("zone", zoneData);
			requestData.Add ("key", "debug");
			computerSquad = new Squad (1, 1, "Spearman", 10, 1, 10, 57.003, 9.991, "debug");
			for (int i = 0; i < computerSquad.getSize (); i++) {
				opponentMinions.Add(new Minion(computerSquad.getKey(), 
					computerSquad.getMaxHealth(), 
					computerSquad.getLevel(), 
					computerSquad.getName(), 
					computerSquad.getPower(), 
					computerSquad.getSpeed(), 
					"Melee", 
					0));
			}
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
		stateRef = FirebaseDatabase.DefaultInstance.GetReference ("battles")
			.Child (battleKey)
			.Child ("state");
		stateRef.ValueChanged += handleValueChanged;
	}

	/// <summary>
	/// Event handler for state changed.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="args">Arguments.</param>
	void handleValueChanged(object sender, ValueChangedEventArgs args){
		DataSnapshot snapshot = args.Snapshot;
		bool battleOver;
		battleOver = (bool)snapshot.Child ("over")
			.GetValue (false);
		
		for (int i = 0; i < playerSprites.Count; i++) {
			UpdateMinionHealth (snapshot, i, true);
		}

		for (int i = 0; i < opponentSprites.Count; i++) {
			UpdateMinionHealth (snapshot, i, false);
		}

		if (battleOver) {
			stateRef.ValueChanged -= handleValueChanged;
			GetReward ();
		} else {
			AdvanceTurn ();
		}
	}

	void AdvanceTurn(){
		Debug.Log ("AdvanceTurn reached");
		turnCounter++;
		turnIndicator.GetComponent<Text> ().text = turnCounter.ToString();

		OnClickPlayerBattleMinion oc = spriteCanvas
			.transform.Find ("Player Minion Sprite 1").gameObject
			.AddComponent<OnClickPlayerBattleMinion> () as OnClickPlayerBattleMinion;
		OnClickPlayerBattleMinion oc2 = spriteCanvas
			.transform.Find ("Player Minion Sprite 2").gameObject
			.AddComponent<OnClickPlayerBattleMinion> () as OnClickPlayerBattleMinion;
		OnClickPlayerBattleMinion oc3 = spriteCanvas
			.transform.Find ("Player Minion Sprite 3").gameObject
			.AddComponent<OnClickPlayerBattleMinion> () as OnClickPlayerBattleMinion;
	}

	void GetReward(){
		Debug.Log ("Battle over getting rewarded");
		gameObject.transform.parent.gameObject.GetComponent<RewardScreenController> ().AwardReward (battleKey, playerMinions, opponentMinions[0]);
		Destroy (gameObject);
	}

	void UpdateMinionHealth(DataSnapshot snapshot, int minionPos, bool isPlayerMinion){
		string minionKey = "";
		string playerKey = "";
		string teamString = "";
		long maxHealth = 0;
		long currentHealth = 0;

		if (isPlayerMinion) {
			playerKey = FirebaseAuthHandler.getUserId ();
			minionKey = playerMinions [minionPos].getKey ();
			teamString = "teamOne";
		} else {
			playerKey = computerMinionKey;
			minionKey = "minion-" + minionPos;
			teamString = "teamTwo";
		}
		Debug.Log (snapshot.Child(teamString).GetValue(false).GetType());
		maxHealth = (long)snapshot.Child (teamString)
			.Child (playerKey)
			.Child ("battleMinions")
			.Child (minionKey)
			.Child ("health")
			.GetValue (false);
		Debug.Log (maxHealth);
		currentHealth = (long)snapshot.Child (teamString)
			.Child (playerKey)
			.Child ("battleMinions")
			.Child (minionKey)
			.Child ("battleStats")
			.Child ("currentHP")
			.GetValue (false);
		Debug.Log (currentHealth);
		
		if (isPlayerMinion) {
			playerSprites [minionPos].transform.Find ("Health").gameObject.GetComponent<Text> ().text =
			currentHealth + " / " + maxHealth;
			if (currentHealth <= 0) {
				playerSprites [minionPos].SetActive (false);
			}
		} else {
			opponentSprites[minionPos].transform.Find ("Health").gameObject.GetComponent<Text> ().text =
				currentHealth + " / " + maxHealth;
			if (currentHealth <= 0) {
				opponentSprites [minionPos].SetActive (false);
			}
		}
	}
		
	/// <summary>
	/// Sets the player minions, and processes the first turn.
	/// </summary>
	public void SetPlayerMinions(){
		playerMinionNum = playerMinions.Count;
		bool minionPicked = false;

		for (int i = 0; i < 3; i++) {
			//Adds a sprite if a minion was picked
			if (i <= (playerMinionNum - 1)) {
				minionPicked = true;
				switch (i) {
				case 0:
					playerMinionOne.SetActive (true);
					playerMinionOne.transform.Find ("Health").gameObject.GetComponent<Text> ().text = 
						playerMinions [0].getHealth () + " / " + playerMinions [0].getHealth ();
					InsertSprite (playerMinions [0], 1, true);
					playerSprites.Add (playerMinionOne);
					break;
				case 1:
					playerMinionTwo.SetActive (true);
					playerMinionTwo.transform.Find ("Health").gameObject.GetComponent<Text> ().text = 
						playerMinions [1].getHealth () + " / " + playerMinions [1].getHealth ();
					InsertSprite (playerMinions [1], 2, true);
					playerSprites.Add (playerMinionTwo);
					break;
				case 2:
					playerMinionThree.SetActive (true);
					playerMinionThree.transform.Find ("Health").gameObject.GetComponent<Text> ().text = 
						playerMinions [2].getHealth () + " / " + playerMinions [2].getHealth ();
					InsertSprite (playerMinions [2], 3, true);
					playerSprites.Add (playerMinionThree);
					break;
				default:
					Debug.Log ("Picked too many minions");
					break;
				}
			}

			if (minionPicked) {
				new FirebaseMove (battleKey, playerMinions [i], i).Start ();
			} else {
				new FirebaseMove (battleKey, i).Start ();
			}

			minionPicked = false;
		}

		AddOpponentSprite ();
	}

	void AddOpponentSprite (){
		switch (opponentMinions.Count) {
		case 1:
			opponentMinionOne.SetActive (true);
			opponentMinionOne.transform.Find ("Health").gameObject.GetComponent<Text> ().text = 
				opponentMinions [0].getHealth () + " / " + opponentMinions [0].getHealth ();
			InsertSprite (opponentMinions [0], 1, false);

			break;
		case 2:
			opponentMinionOne.SetActive (true);
			opponentMinionOne.transform.Find ("Health").gameObject.GetComponent<Text> ().text = 
				opponentMinions [0].getHealth () + " / " + opponentMinions [0].getHealth ();
			InsertSprite (opponentMinions [0], 1, false);

			opponentMinionTwo.SetActive (true);
			opponentMinionTwo.transform.Find ("Health").gameObject.GetComponent<Text> ().text = 
				opponentMinions [1].getHealth () + " / " + opponentMinions [1].getHealth ();
			InsertSprite (opponentMinions [1], 2, false);

			break;
		case 3:
			opponentMinionOne.SetActive (true);
			opponentMinionOne.transform.Find ("Health").gameObject.GetComponent<Text> ().text = 
				opponentMinions [0].getHealth () + " / " + opponentMinions [0].getHealth ();
			InsertSprite (opponentMinions [0], 1, false);

			opponentMinionTwo.SetActive (true);
			opponentMinionTwo.transform.Find ("Health").gameObject.GetComponent<Text> ().text = 
				opponentMinions [1].getHealth () + " / " + opponentMinions [1].getHealth ();
			InsertSprite (opponentMinions [1], 2, false);

			opponentMinionThree.SetActive (true);
			opponentMinionThree.transform.Find ("Health").gameObject.GetComponent<Text> ().text = 
				opponentMinions [2].getHealth () + " / " + opponentMinions [2].getHealth ();
			InsertSprite (opponentMinions [2], 3, false);

			break;
		default:
			Debug.Log ("Opponent minions 0 or over 3");

			break;
		}
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

	void OnDestroy(){
		stateRef.ValueChanged -= handleValueChanged;
	}
}
