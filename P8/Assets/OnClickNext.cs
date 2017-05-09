using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnClickNext : MonoBehaviour {
	GameObject minionPanelControllerGO;
	MinionPanelController minionPanelController;
	public Button TheButton;

	// Use this for initialization
	void Start () {
		TheButton = gameObject.GetComponent<Button>();
		minionPanelControllerGO = gameObject.transform.parent.gameObject
			.transform.parent.gameObject
			.transform.Find("Minion Panel").gameObject;
		minionPanelController = minionPanelControllerGO.GetComponent<MinionPanelController> ();
		TheButton.onClick.AddListener (NextClicked);
	}

	void NextClicked(){
		Debug.Log ("Next");
		minionPanelController.NextPressed ();
	}
}
