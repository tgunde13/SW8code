using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mgl;

public class LocalizeText : MonoBehaviour {
	
	public string localizeKey;

	void Start(){
		GameObject gameObject = GameObject.Find ("I18nManager");
		if (gameObject != null) {
			GetComponent<Text> ().text = gameObject.GetComponent<I18nManager> ().GetLocalisedString (localizeKey);
		} else {
			GetComponent<Text> ().text = localizeKey;
			Debug.Log("I18n: Couldn't find I18nManager");	
		}
	}


}
