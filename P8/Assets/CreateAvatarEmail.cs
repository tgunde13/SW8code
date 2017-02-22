using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase;
using Firebase.Unity.Editor;

public class CreateAvatarEmail : MonoBehaviour {
	public Text emailText, passwordText;

	// Use this for initialization
	void Start () {
	}

	public void perform() {

		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://p8-server.firebaseio.com/");

		FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;


		Debug.Log ("test");

		auth.CreateUserWithEmailAndPasswordAsync("2@tobiasgundersen.dk", "123456").ContinueWith(task => {
			if (task.IsCanceled) {
				Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
				return;
			}
			if (task.IsFaulted) {
				Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
				return;
			}

			// Firebase user has been created.
			Firebase.Auth.FirebaseUser newUser = task.Result;
			Debug.LogFormat("Firebase user created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
