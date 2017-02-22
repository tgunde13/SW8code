using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogInEmail : MonoBehaviour {
	public Text emailText, emailErrorText, passwordText, passwordErrorText;

	// Use this for initialization
	void Start () {
		
	}

	public void LogIn() {
		string email = emailText.text;
		string password = passwordText.text;

		// Validate input before call to Firebase
		bool validEmail = InputValidator.validateEmail(email, emailErrorText);
		bool validPassword = InputValidator.validatePassword (password, passwordErrorText);

		if (!(validEmail && validPassword)) {
			return;
		}
	}
		
	
	// Update is called once per frame
	void Update () {
		
	}
}
