using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mgl;

public static class I18nTools {

	public static string SetLanguage(){
		if (Application.systemLanguage == SystemLanguage.Danish) {
			return Constants.LocaleDaDk;
		} else {
			return Constants.LocaleEnGb;
		}
	}
}
