using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnClickAttack : MonoBehaviour {
	public Button TheButton;
	public Minion minion;
	public GameObject minionSpriteCalling;
	public string battleKey;

	private GameObject opponentOne;
	private GameObject opponentTwo;
	private GameObject opponentThree;

	void Start(){
		TheButton = gameObject.GetComponent<Button> ();
		TheButton.onClick.AddListener (AttackClicked);
	}

	void AttackClicked(){
		opponentOne = minionSpriteCalling
			.GetComponent<OnClickPlayerBattleMinion> ().opponentSpriteOne;
		OnClickOpponentBattleMinion one = opponentOne
			.AddComponent<OnClickOpponentBattleMinion> () as OnClickOpponentBattleMinion;
		one.playerMinion = minion;
		one.minionSpriteCalling = minionSpriteCalling;
		one.battleKey = battleKey;

		opponentTwo = minionSpriteCalling
			.GetComponent<OnClickPlayerBattleMinion> ().opponentSpriteTwo;
		OnClickOpponentBattleMinion two = opponentTwo
			.AddComponent<OnClickOpponentBattleMinion> () as OnClickOpponentBattleMinion;
		two.playerMinion = minion;
		two.minionSpriteCalling = minionSpriteCalling;
		two.battleKey = battleKey;

		opponentThree = minionSpriteCalling
			.GetComponent<OnClickPlayerBattleMinion> ().opponentSpriteThree;
		OnClickOpponentBattleMinion three = opponentThree
			.AddComponent<OnClickOpponentBattleMinion> () as OnClickOpponentBattleMinion;
		three.playerMinion = minion;
		three.minionSpriteCalling = minionSpriteCalling;
		three.battleKey = battleKey;
	}
}
