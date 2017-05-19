﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnClickSkip : MonoBehaviour {
	public Button TheButton;
	public Minion minion;
	public GameObject minionSpriteCalling;
	public string battleKey;

	void Start(){
		TheButton = gameObject.GetComponent<Button> ();
		TheButton.onClick.AddListener (setFirebaseSkip);
	}

	void setFirebaseSkip(){
		new FirebaseMove (battleKey, minion).Start();
		minionSpriteCalling
			.GetComponent<OnClickPlayerBattleMinion> ().SetPassive(false);
		gameObject.SetActive (false);
	}
}