using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase;
using Firebase.Unity.Editor;

public class CreateAvatarEmail : MonoBehaviour {
	public Text emailText, emailErrorText, passwordText, passwordErrorText;
	public AlertDialog ShowErrorScript;

	// Use this for initialization
	void Start () {
	}

	public void perform() {
		string email = emailText.text;
		string password = passwordText.text;

		// Validate input before call to Firebase
		bool validEmail = InputValidator.validateEmail(email, emailErrorText);
		bool validPassword = InputValidator.validatePassword (password, passwordErrorText);

		if (!(validEmail && validPassword)) {
			return;
		}

		FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

		auth.CreateUserWithEmailAndPasswordAsync(email, passwordText.text).ContinueWith(task => {
			if (task.IsCanceled) {
				return;
			}
			if (task.IsFaulted) {
				ShowErrorScript.show("Unknow error.");
				return;
			}

			// Firebase user has been created.
			Firebase.Auth.FirebaseUser newUser = task.Result;
			Debug.LogFormat("Firebase user created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
			ShowErrorScript.show("User created");
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
