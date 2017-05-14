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

	void Start () {
		minionOneText = gameObject.transform.parent.gameObject
			.transform.Find("Battle Panel").gameObject
			.transform.Find("Minion 1").gameObject;
		minionTwoText = gameObject.transform.parent.gameObject
			.transform.Find("Battle Panel").gameObject
			.transform.Find("Minion 2").gameObject;
		minionThreeText = gameObject.transform.parent.gameObject
			.transform.Find("Battle Panel").gameObject
			.transform.Find("Minion 3").gameObject;
		attackButton = gameObject.transform.parent.gameObject
			.transform.Find("Battle Panel").gameObject
			.transform.Find("Attack").gameObject;
		skipButton = gameObject.transform.parent.gameObject
			.transform.Find("Battle Panel").gameObject
			.transform.Find("Skip").gameObject;
	}

	public void OnPointerClick(PointerEventData eventData){
		
	}
}
