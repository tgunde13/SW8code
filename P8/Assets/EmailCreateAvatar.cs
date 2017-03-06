using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase;
using Firebase.Unity.Editor;

/// <summary>
/// Used for creating an avatar with an e-mail address.
/// </summary>
public class EmailCreateAvatar : MonoBehaviour {
	public Text emailErrorText, passwordErrorText;
	public InputField emailField, passwordField;
	public AlertDialog ShowErrorScript;

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
				Debug.Log("TOB: Stack trace: " + task.Exception.StackTrace);
				Debug.Log("TOB: task.Exception: " + task.Exception);
				Debug.Log("TOB: task.Exception.InnerException: " + task.Exception);
				Debug.Log("TOB: task.Exception.InnerException.StackTrace: " + task.Exception.InnerException.StackTrace);
				Debug.Log("TOB: task.Exception.InnerException.InnerException: " + task.Exception.InnerException.InnerException);
				return;
			}

			ShowErrorScript.show("User created");
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
