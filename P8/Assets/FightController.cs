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

	private List<GameObject> playerSpritesText;
	private List<GameObject> opponentSpritesText;
	private List<GameObject> playerSprites;
	private List<GameObject> opponentSprites;
	private bool updatingBattleState = false;
	private bool IsEnviormentBattle = false;
	private GameObject computerGameobject;
	private int playerMinionNum;
	private int turnCounter = -1;
	private GameObject battlePanel;
	private GameObject playerMinionOneText;
	private GameObject playerMinionTwoText;
	private GameObject playerMinionThreeText;
	private GameObject opponentMinionOneText;
	private GameObject opponentMinionTwoText;
	private GameObject opponentMinionThreeText;
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
		playerSpritesText = new List<GameObject> ();
		opponentSpritesText = new List<GameObject> ();
		playerSprites = new List<GameObject> ();
		opponentSprites = new List<GameObject> ();


		//--------------initialization of gameobjects--------------//
		battlePanel = gameObject.transform.Find ("Battle Panel").gameObject;
		playerMinionOneText = battlePanel.transform.Find ("Minion 1").gameObject;
		playerMinionTwoText = battlePanel.transform.Find ("Minion 2").gameObject;
		playerMinionThreeText = battlePanel.transform.Find ("Minion 3").gameObject;
		spriteCanvas = gameObject.transform.Find ("Minion Sprites").gameObject;
		opponentMinionOneText = battlePanel.transform.Find ("Opponent 1").gameObject;
		opponentMinionTwoText = battlePanel.transform.Find ("Opponent 2").gameObject;
		opponentMinionThreeText = battlePanel.transform.Find ("Opponent 3").gameObject;
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
			latIndex = (int)Math.Floor ((computerSquad.GetPos ().x) * 100f);
			lonIndex = (int)Math.Floor ((computerSquad.GetPos ().y) * 100f);
			zoneData.Add ("latIndex", latIndex);
			zoneData.Add ("lonIndex", lonIndex);
			requestData.Add ("code", Constants.RequestCodeSoloBattle);
			requestData.Add ("zone", zoneData);
			requestData.Add ("key", computerMinionKey);
			for (int i = 0; i < computerSquad.GetSize (); i++) {
				opponentMinions.Add(new Minion(computerSquad.GetKey(), 
					computerSquad.GetMaxHealth(), 
					computerSquad.GetLevel(), 
					computerSquad.GetName(), 
					computerSquad.GetPower(), 
					computerSquad.GetSpeed(), 
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
			computerMinionKey = "debug";
			computerSquad = new Squad (1, 1, "Spearman", 10, 1, 10, 57.003, 9.991, "debug");
			for (int i = 0; i < computerSquad.GetSize (); i++) {
				opponentMinions.Add(new Minion(computerSquad.GetKey(), 
					computerSquad.GetMaxHealth(), 
					computerSquad.GetLevel(), 
					computerSquad.GetName(), 
					computerSquad.GetPower(), 
					computerSquad.GetSpeed(), 
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
		stateRef.ValueChanged += HandleValueChanged;
	}

	/// <summary>
	/// Event handler for state changed.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="args">Arguments.</param>
	void HandleValueChanged(object sender, ValueChangedEventArgs args){
		DataSnapshot snapshot = args.Snapshot;
		bool battleOver;
		battleOver = (bool)snapshot.Child ("over")
			.GetValue (false);
		
		for (int i = 0; i < playerSpritesText.Count; i++) {
			UpdateMinionHealth (snapshot, i, true);
		}
			
		for (int i = 0; i < opponentSpritesText.Count; i++) {
			Debug.Log(opponentSpritesText [i].name);
			UpdateMinionHealth (snapshot, i, false);
		}

		if (battleOver) {
			stateRef.ValueChanged -= HandleValueChanged;
			GetReward ();
		} else {
			AdvanceTurn ();
		}
	}

	void AdvanceTurn(){
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
			minionKey = playerMinions [minionPos].GetKey ();
			teamString = "teamOne";
		} else {
			playerKey = computerMinionKey;
			minionKey = "minion-" + minionPos.ToString();
			teamString = "teamTwo";
		}

		maxHealth = (long)snapshot.Child (teamString)
			.Child (playerKey)
			.Child ("battleMinions")
			.Child (minionKey)
			.Child ("health")
			.GetValue (false);
		currentHealth = (long)snapshot.Child (teamString)
			.Child (playerKey)
			.Child ("battleMinions")
			.Child (minionKey)
			.Child ("battleStats")
			.Child ("currentHP")
			.GetValue (false);
		
		if (isPlayerMinion) {
			playerSpritesText [minionPos].transform.Find ("Health").gameObject.GetComponent<Text> ().text =
			currentHealth + " / " + maxHealth;
			if (currentHealth <= 0) {
				playerSpritesText [minionPos].SetActive (false);
				playerSprites [minionPos].SetActive (false);
			}
		} else {
			opponentSpritesText[minionPos].transform.Find ("Health").gameObject.GetComponent<Text> ().text =
				currentHealth + " / " + maxHealth;
			if (currentHealth <= 0) {
				opponentSpritesText [minionPos].SetActive (false);
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
					playerMinionOneText.SetActive (true);
					playerMinionOneText.transform.Find ("Health").gameObject.GetComponent<Text> ().text = 
						playerMinions [0].GetHealth () + " / " + playerMinions [0].GetHealth ();
					InsertSprite (playerMinions [0], 1, true);
					playerSpritesText.Add (playerMinionOneText);
					break;
				case 1:
					playerMinionTwoText.SetActive (true);
					playerMinionTwoText.transform.Find ("Health").gameObject.GetComponent<Text> ().text = 
						playerMinions [1].GetHealth () + " / " + playerMinions [1].GetHealth ();
					InsertSprite (playerMinions [1], 2, true);
					playerSpritesText.Add (playerMinionTwoText);
					break;
				case 2:
					playerMinionThreeText.SetActive (true);
					playerMinionThreeText.transform.Find ("Health").gameObject.GetComponent<Text> ().text = 
						playerMinions [2].GetHealth () + " / " + playerMinions [2].GetHealth ();
					InsertSprite (playerMinions [2], 3, true);
					playerSpritesText.Add (playerMinionThreeText);
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
			opponentMinionOneText.SetActive (true);
			opponentMinionOneText.transform.Find ("Health").gameObject.GetComponent<Text> ().text = 
				opponentMinions [0].GetHealth () + " / " + opponentMinions [0].GetHealth ();
			InsertSprite (opponentMinions [0], 1, false);
			opponentSpritesText.Add (opponentMinionOneText);

			break;
		case 2:
			opponentMinionOneText.SetActive (true);
			opponentMinionOneText.transform.Find ("Health").gameObject.GetComponent<Text> ().text = 
				opponentMinions [0].GetHealth () + " / " + opponentMinions [0].GetHealth ();
			InsertSprite (opponentMinions [0], 1, false);
			opponentSpritesText.Add (opponentMinionOneText);

			opponentMinionTwoText.SetActive (true);
			opponentMinionTwoText.transform.Find ("Health").gameObject.GetComponent<Text> ().text = 
				opponentMinions [1].GetHealth () + " / " + opponentMinions [1].GetHealth ();
			InsertSprite (opponentMinions [1], 2, false);
			opponentSpritesText.Add (opponentMinionTwoText);

			break;
		case 3:
			opponentMinionOneText.SetActive (true);
			opponentMinionOneText.transform.Find ("Health").gameObject.GetComponent<Text> ().text = 
				opponentMinions [0].GetHealth () + " / " + opponentMinions [0].GetHealth ();
			InsertSprite (opponentMinions [0], 1, false);
			opponentSpritesText.Add (opponentMinionOneText);

			opponentMinionTwoText.SetActive (true);
			opponentMinionTwoText.transform.Find ("Health").gameObject.GetComponent<Text> ().text = 
				opponentMinions [1].GetHealth () + " / " + opponentMinions [1].GetHealth ();
			InsertSprite (opponentMinions [1], 2, false);
			opponentSpritesText.Add (opponentMinionTwoText);

			opponentMinionThreeText.SetActive (true);
			opponentMinionThreeText.transform.Find ("Health").gameObject.GetComponent<Text> ().text = 
				opponentMinions [2].GetHealth () + " / " + opponentMinions [2].GetHealth ();
			InsertSprite (opponentMinions [2], 3, false);
			opponentSpritesText.Add (opponentMinionThreeText);

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
		sprite.GetComponent<Image> ().sprite = SelectSprite (minion.GetName ());
		if (isPlayerMinion) {
			playerSprites.Add (sprite);
		} else {
			opponentSprites.Add (sprite);
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

	void OnDestroy(){
		stateRef.ValueChanged -= HandleValueChanged;
	}
}
