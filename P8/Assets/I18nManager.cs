using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mgl;

public class I18nManager : MonoBehaviour {

	private I18n i18n = I18n.Instance;

	private void Start(){
		SetLanguage();
		DontDestroyOnLoad (gameObject);
	}

	private void SetLanguage(){
		if (Application.systemLanguage == SystemLanguage.English) {
			I18n.SetLocale ("en");
		} else if (Application.systemLanguage == SystemLanguage.Danish) {
			I18n.SetLocale ("da");
		} else {
			I18n.SetLocale ("en");
		}


	}

	public string GetLocalisedString(string key){
		return i18n.__ (key);
	}
}
