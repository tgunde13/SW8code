using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnClickPlayerBattleMinion : MonoBehaviour, IPointerClickHandler {
	public Minion minion;
	public GameObject opponentSpriteOne;
	public GameObject opponentSpriteTwo;
	public GameObject opponentSpriteThree;

	private GameObject attackButton;
	private GameObject skipButton;
	private GameObject minionOneText;
	private GameObject minionTwoText;
	private GameObject minionThreeText;
	private GameObject opponentOneText;
	private GameObject opponentTwoText;
	private GameObject opponentThreeText;
	private GameObject playerSpriteOne;
	private GameObject playerSpriteTwo;
	private GameObject playerSpriteThree;
	private GameObject fightController;
	private string battleKey;
	private int minionIndexNumber;
	private bool firstActive = true;


	public void Start () {
		//Index at which minion is
		minionIndexNumber = Int32.Parse(gameObject.name.Substring(21)) - 1;


		//Fight Controller
		fightController = gameObject.transform.parent.gameObject
			.transform.parent.gameObject;


		//battleKey
		battleKey = fightController.GetComponent<FightController>().battleKey;


		//Set minion
		minion = fightController.GetComponent<FightController>().playerMinions[minionIndexNumber];


		//Health idicators
		minionOneText = gameObject.transform.parent.gameObject
			.transform.parent.gameObject
			.transform.Find("Battle Panel").gameObject
			.transform.Find("Minion 1").gameObject;

		minionTwoText = gameObject.transform.parent.gameObject
			.transform.parent.gameObject
			.transform.Find("Battle Panel").gameObject
			.transform.Find("Minion 2").gameObject;

		minionThreeText = gameObject.transform.parent.gameObject
			.transform.parent.gameObject
			.transform.Find("Battle Panel").gameObject
			.transform.Find("Minion 3").gameObject;

		opponentOneText = gameObject.transform.parent.gameObject
			.transform.parent.gameObject
			.transform.Find("Battle Panel").gameObject
			.transform.Find("Opponent 1").gameObject;

		opponentTwoText = gameObject.transform.parent.gameObject
			.transform.parent.gameObject
			.transform.Find("Battle Panel").gameObject
			.transform.Find("Opponent 2").gameObject;

		opponentThreeText = gameObject.transform.parent.gameObject
			.transform.parent.gameObject
			.transform.Find("Battle Panel").gameObject
			.transform.Find("Opponent 3").gameObject;


		//Buttons
		attackButton = gameObject.transform.parent.gameObject
			.transform.parent.gameObject
			.transform.Find("Battle Panel").gameObject
			.transform.Find("Attack").gameObject;

		skipButton = gameObject.transform.parent.gameObject
			.transform.parent.gameObject
			.transform.Find("Battle Panel").gameObject
			.transform.Find("Skip").gameObject;


		//Sprites
		playerSpriteOne = gameObject.transform.parent.gameObject
			.transform.Find ("Player Minion Sprite 1").gameObject;

		playerSpriteTwo = gameObject.transform.parent.gameObject
			.transform.Find ("Player Minion Sprite 2").gameObject;

		playerSpriteThree = gameObject.transform.parent.gameObject
			.transform.Find ("Player Minion Sprite 3").gameObject;

		opponentSpriteOne = gameObject.transform.parent.gameObject
			.transform.Find ("Opponent Minion Sprite 1").gameObject;

		opponentSpriteTwo = gameObject.transform.parent.gameObject
			.transform.Find ("Opponent Minion Sprite 2").gameObject;

		opponentSpriteThree = gameObject.transform.parent.gameObject
			.transform.Find ("Opponent Minion Sprite 3").gameObject;
	}

	public void OnPointerClick(PointerEventData eventData){
		Debug.Log ("Found minion: " + minion.ToString ());
		Debug.Log (minionOneText.activeSelf);
		if (minionOneText.activeSelf || firstActive) {
			SetAttacking ();
			firstActive = false;
		} else {
			SetPassive (true);
		}
	}

	void SetAttacking(){
		if (playerSpriteOne.activeSelf) {
			minionOneText.SetActive (false);
		}

		if (playerSpriteTwo.activeSelf) {
			minionTwoText.SetActive (false);
		}

		if (playerSpriteThree.activeSelf) {
			minionThreeText.SetActive (false);
		}

		if (opponentSpriteOne.activeSelf) {
			opponentOneText.SetActive (false);
		}

		if (opponentSpriteTwo.activeSelf) {
			opponentTwoText.SetActive (false);
		}

		if (opponentSpriteThree.activeSelf) {
			opponentThreeText.SetActive (false);
		}

		attackButton.SetActive (true);
		attackButton.GetComponent<OnClickAttack> ().minion = minion;
		attackButton.GetComponent<OnClickAttack> ().minionSpriteCalling = gameObject;
		attackButton.GetComponent<OnClickAttack> ().battleKey = battleKey;
		skipButton.SetActive (true);
		skipButton.GetComponent<OnClickSkip> ().minion = minion;
		skipButton.GetComponent<OnClickSkip> ().minionSpriteCalling = gameObject;
		skipButton.GetComponent<OnClickSkip> ().battleKey = battleKey;
	}

	public void SetPassive(bool minionWasClicked){
		
		attackButton.SetActive (false);

		skipButton.SetActive (false);

		if (playerSpriteOne.activeSelf) {
			minionOneText.SetActive (true);
		}

		if (playerSpriteTwo.activeSelf) {
			minionTwoText.SetActive (true);
		}

		if (playerSpriteThree.activeSelf) {
			minionThreeText.SetActive (true);
		}

		if (opponentSpriteOne.activeSelf) {
			opponentOneText.SetActive (true);
			OnClickOpponentBattleMinion one = opponentSpriteOne
				.GetComponent<OnClickOpponentBattleMinion>();
			if(one != null){
				Destroy(one);
			}
		}

		if (opponentSpriteTwo.activeSelf) {
			opponentTwoText.SetActive (true);
			OnClickOpponentBattleMinion two = opponentSpriteOne
				.GetComponent<OnClickOpponentBattleMinion>();
			if(two != null){
				Destroy(two);
			}
		}

		if (opponentSpriteThree.activeSelf) {
			opponentThreeText.SetActive (true);
			OnClickOpponentBattleMinion three = opponentSpriteOne
				.GetComponent<OnClickOpponentBattleMinion>();
			if(three != null){
				Destroy(three);
			}
		}

		if (!minionWasClicked) {
			Destroy (this);
		}
	}
}
