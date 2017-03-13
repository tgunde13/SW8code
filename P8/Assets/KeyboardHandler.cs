using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// Hide preview when showing keyboaord
		Debug.Log ("TOB: KeyboardHandler, TouchScreenKeyboard.hideInput: " + TouchScreenKeyboard.hideInput);
		TouchScreenKeyboard.hideInput = true;
		Debug.Log ("TOB: KeyboardHandler, TouchScreenKeyboard.hideInput: " + TouchScreenKeyboard.hideInput);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
