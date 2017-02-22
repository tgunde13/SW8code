using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;

public class LoginHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public void LogIn(Credential credential) {
		Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

		auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
			if (task.IsCanceled) {
				Debug.LogError("SignInWithCredentialAsync was canceled.");
				return;
			}
			if (task.IsFaulted) {
				Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
				return;
			}

			Firebase.Auth.FirebaseUser newUser = task.Result;
			Debug.LogFormat("User signed in successfully: {0} ({1})",
				newUser.DisplayName, newUser.UserId);
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
