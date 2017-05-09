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

	private bool updatingBattleState = false;
	private int playerMinionNum;
	private GameObject battlePanel;
	private GameObject playerMinionOne;
	private GameObject playerMinionTwo;
	private GameObject playerMinionThree;
	private GameObject opponentMinionOne;
	private GameObject opponentMinionTwo;
	private GameObject opponentMinionThree;
	private GameObject spriteCanvas;

	// Use this for initialization
	void Start () {
		playerMinions = new List<Minion> ();
		opponentMinions = new List<Minion> ();
		battlePanel = gameObject.transform.Find ("Battle Panel").gameObject;
		playerMinionOne = battlePanel.transform.Find ("Minion 1").gameObject;
		playerMinionTwo = battlePanel.transform.Find ("Minion 2").gameObject;
		playerMinionThree = battlePanel.transform.Find ("Minion 3").gameObject;
		spriteCanvas = gameObject.transform.Find ("Minion Sprites").gameObject;
	}

	public void StartFight(){
		playerMinionNum = playerMinions.Count;
		string minionName = "";

		if (playerMinionNum > 0) {
			playerMinionOne.SetActive (true);
			playerMinionOne.transform.Find ("Health").gameObject.GetComponent<Text> ().text = 
				playerMinions [0].getHealth () + " / " + playerMinions [0].getHealth ();
			InsertSprite (playerMinions [0], 1, true);
			if (playerMinionNum > 1) {
				playerMinionTwo.SetActive (true);
				playerMinionTwo.transform.Find ("Health").gameObject.GetComponent<Text> ().text = 
					playerMinions [1].getHealth () + " / " + playerMinions [1].getHealth ();
				InsertSprite (playerMinions [1], 2, true);
				if (playerMinionNum > 2) {
					playerMinionThree.SetActive (true);
					playerMinionThree.transform.Find ("Health").gameObject.GetComponent<Text> ().text = 
						playerMinions [2].getHealth () + " / " + playerMinions [2].getHealth ();
					InsertSprite (playerMinions [2], 3, true);
				}
			}
		} else {
			Debug.Log ("Did not find any user minions");
		}
	}

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
