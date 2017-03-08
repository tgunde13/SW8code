using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.SceneManagement;

/// <summary>
/// Firebase login handler.
/// </summary>
public static class FirebaseLoginHandler {
	private const string mapSceneName = "Map";

	/// <summary>
	/// Logs in.
	/// </summary>
	/// <param name="credential">Firebase credential.</param>
	/// <param name="dialog">Dialog to show a potential error with.</param>
	public static void LogIn(Credential credential, AlertDialog dialog) {
		Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

		auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
			if (task.IsCanceled) {
				Debug.Log("TOB: FirebaseLoginHandler, login canceled");
				return;
			}
			if (task.IsFaulted) {
				dialog.show("Login did not succeed.");
				Debug.Log("TOB: FirebaseLoginHandler, login faulted");
				return;
			}

			Debug.Log("TOB: FirebaseLoginHandler, login succeded");
			SceneManager.LoadScene(mapSceneName);
		});
	}
}
