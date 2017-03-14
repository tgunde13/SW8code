using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mgl;

public class LocalizeText : MonoBehaviour {
	
	public string localizeKey;

	void Start(){
		GetComponent<Text> ().text = I18nManager.GetInstance ().GetLocalisedString (localizeKey);
	}


}
