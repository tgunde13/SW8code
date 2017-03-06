using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public static class FirebaseLoginHandler {
	private const string mapSceneName = "Map";

	public static void LogIn(Credential credential, AlertDialog dialog) {
		Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

		auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
			if (task.IsCanceled) {
				return;
			}
			if (task.IsFaulted) {
				dialog.show("Login did not succeed.");
				return;
			}

			Debug.Log("TOB: Logged in to Firebase");
			SceneManager.LoadScene(mapSceneName);
		});
	}
}
