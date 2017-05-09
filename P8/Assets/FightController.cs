using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using UnityEngine.UI;

public class FightController : MonoBehaviour {
	public List<Minion> playerMinions;
	public List<Minion> opponentMinions;

	private bool updatingBattleState = false;

	// Use this for initialization
	void Start () {
		playerMinions = new List<Minion> ();
		opponentMinions = new List<Minion> ();
	}

	public void StartFight(){
		Debug.Log ("Found " + playerMinions.Count + " minions");
	}
}
