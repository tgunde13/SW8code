using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Back button.
/// The onClick is invoked if back button on Android is pressed.
/// </summary>
public class BackButton : Button {
	#if UNITY_ANDROID
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			onClick.Invoke ();
		}
	}
	#endif
}
