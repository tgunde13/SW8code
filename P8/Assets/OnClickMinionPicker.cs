using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnClickMinionPicker : MonoBehaviour, IPointerClickHandler {
	public Minion minion;
	private GameObject minionPanelController;
	private GameObject border;

	void Start () {
		minionPanelController = gameObject.transform.parent.gameObject;
		border = gameObject.transform.Find ("Border").gameObject;
	}

	public void OnPointerClick(PointerEventData eventData){
		if (minionPanelController.GetComponent<MinionPanelController> ().RemoveMinionSelection (minion)) {
			if (border.activeSelf) {
				border.SetActive (false);
			} else {
				border.SetActive (true);
			}
		}
	}
}
