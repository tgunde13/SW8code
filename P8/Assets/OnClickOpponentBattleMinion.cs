using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnClickOpponentBattleMinion : MonoBehaviour, IPointerClickHandler {
	public Minion minion;
	private GameObject attackButton;
	private GameObject skipButton;
	private GameObject minionOneText;
	private GameObject minionTwoText;
	private GameObject minionThreeText;
	private GameObject playerSpriteOne;
	private GameObject playerSpriteTwo;
	private GameObject playerSpriteThree;
	private GameObject opponentSpriteOne;
	private GameObject opponentSpriteTwo;
	private GameObject opponentSpriteThree;

	void Start () {
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
		
	}
}
