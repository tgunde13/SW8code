using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mgl;

public class I18nManager : MonoBehaviour {

	private static GameObject i18nManager;

	private I18n i18n = I18n.Instance;
	private bool languageSet = false;

	public static I18nManager GetInstance() {
		if (i18nManager == null) {
			i18nManager = new GameObject ();
			return i18nManager.AddComponent<I18nManager> ();
		} else {
			return i18nManager.GetComponent<I18nManager> ();
		}
	}

	private I18nManager() { } 

	private void Start(){
		DontDestroyOnLoad (gameObject);
		SetLanguage();
	}

	private void SetLanguage(){
		if (Application.systemLanguage == SystemLanguage.Danish) {
			I18n.SetLocale ("da");
		} else {
			I18n.SetLocale ("en");
		}
		languageSet = true;


	}

	public string GetLocalisedString(string key){
		if (!languageSet) {
			SetLanguage ();
		}
		return i18n.__ (key);
	}
}
