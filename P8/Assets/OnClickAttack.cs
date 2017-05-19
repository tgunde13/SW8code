using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnClickAttack : MonoBehaviour {
	public Button TheButton;
	public Minion minion;
	public GameObject minionSpriteCalling;
	public string battleKey;

	void Start(){
		TheButton = gameObject.GetComponent<Button> ();
		TheButton.onClick.AddListener (attackClicked);
	}

	void attackClicked(){
		Debug.Log (minion.ToString ());
	}
}
