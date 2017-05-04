using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnClickMinionPicker : MonoBehaviour, IPointerClickHandler {
	public Minion minion;
	private GameObject minionPanelController;

	void Start () {
		minionPanelController = gameObject.transform.parent.gameObject;
	}

	public void OnPointerClick(PointerEventData eventData){
		minionPanelController.GetComponent<MinionPanelController> ().RemoveMinionSelection (minion);
	}
}
