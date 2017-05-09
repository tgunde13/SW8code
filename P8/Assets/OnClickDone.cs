using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnClickDone : MonoBehaviour {
	public GameObject minionPanelController;
	public Button TheButton;

	// Use this for initialization
	void Start () {
		TheButton = gameObject.GetComponent<Button>();
		minionPanelController = gameObject.transform.parent.gameObject
			.transform.parent.gameObject
			.transform.Find("Minion Panel").gameObject;
		TheButton.onClick.AddListener(DoneClicked);
	}

	void DoneClicked(){
		Debug.Log ("Not implemented");
	}
}
