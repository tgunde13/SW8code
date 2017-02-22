using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase;
using Firebase.Unity.Editor;

public class CreateAvatarEmail : MonoBehaviour {
	public Text emailText, passwordText;
	public ShowError ShowErrorScript;

	// Use this for initialization
	void Start () {
	}

	public void perform() {
		FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

		auth.CreateUserWithEmailAndPasswordAsync(emailText.text, passwordText.text).ContinueWith(task => {
			if (task.IsCanceled) {
				Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
				return;
			}
			if (task.IsFaulted) {
				Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
				ShowErrorScript.perform("An error occured. Possible reasons:\nThe e-mail address is already in use by another player");
				return;
			}

			// Firebase user has been created.
			Firebase.Auth.FirebaseUser newUser = task.Result;
			Debug.LogFormat("Firebase user created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
			ShowErrorScript.perform("User created");
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
