using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase;
using Firebase.Unity.Editor;

public class CreateAvatarEmail : MonoBehaviour {
	public Text emailErrorText, passwordErrorText;
	public InputField emailField, passwordField;
	public AlertDialog ShowErrorScript;

	// Use this for initialization
	void Start () {
	}

	public void perform() {
		string email = emailField.text;
		string password = passwordField.text;

		// Validate input before call to Firebase
		bool validEmail = InputValidator.validateEmail(email, emailErrorText);
		bool validPassword = InputValidator.validatePassword (password, passwordErrorText);

		if (!(validEmail && validPassword)) {
			return;
		}

		FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

		auth.CreateUserWithEmailAndPasswordAsync(email, passwordField.text).ContinueWith(task => {
			if (task.IsCanceled) {
				ShowErrorScript.show("Canceled...");
				return;
			}
			if (task.IsFaulted) {
				ShowErrorScript.show("Unknow error.");
				return;
			}

			ShowErrorScript.show("User created");
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
