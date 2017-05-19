using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnClickOpponentBattleMinion : MonoBehaviour, IPointerClickHandler {
	public Minion playerMinion;
	public GameObject minionSpriteCalling;
	public string battleKey;

	private Minion minion;
	private int minionIndexNumber;
	private GameObject fightController;

	void Start () {
		//Index at which minion is
		minionIndexNumber = Int32.Parse (gameObject.name.Substring (23)) - 1;

		//Fight Controller
		fightController = gameObject.transform.parent.gameObject
		.transform.parent.gameObject;

		//set minion
		minion = fightController.GetComponent<FightController>()
			.opponentMinions[minionIndexNumber];
	}

	public void OnPointerClick(PointerEventData eventData){
		new FirebaseMove (battleKey, playerMinion, minion, minionIndexNumber).Start();
	}
}
