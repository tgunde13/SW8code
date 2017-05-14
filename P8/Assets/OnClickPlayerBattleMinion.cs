using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnClickPlayerBattleMinion : MonoBehaviour, IPointerClickHandler {
	public Minion minion;
	private bool pickingOption = false;
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
		if (pickingOption) {
			attackButton.SetActive (false);
			skipButton.SetActive (false);
			minionOneText.SetActive (true);
			minionTwoText.SetActive (true);
			minionThreeText.SetActive (true);
		} else {
			minionOneText.SetActive (false);
			minionTwoText.SetActive (false);
			minionThreeText.SetActive (false);
			attackButton.SetActive (true);
			skipButton.SetActive (true);
		}
	}
}
