using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mgl;

/// <summary>
/// Firebase login handler.
/// </summary>
public static class FirebaseLoginHandler {
	/// <summary>
	/// Logs in.
	/// </summary>
	/// <param name="credential">Credential to log in with.</param>
	/// <param name="dialog">Dialog to show a potential error with.</param>
	/// <param name="processIndicator">Process indicator to indicate this operation with.</param>
	/// <param name="selectables">Selectables to enable once this operation is done.</param>
	public static void LogIn(Credential credential, DialogPanel dialog, TaskIndicator indicator) {
		Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

		auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
			if (task.IsCanceled) {
				Debug.Log("TOB: FirebaseLoginHandler, login canceled");
				indicator.OnEnd();
				return;
			}

			if (task.IsFaulted) {
				Debug.Log("TOB: FirebaseLoginHandler, login faulted");
				dialog.show(I18n.Instance.__ ("ErrorLogin"), () => indicator.OnEnd());
				indicator.onPause();
				return;
			}

			Debug.Log("TOB: FirebaseLoginHandler, login succeded");
			// Firebase Auth Setup should handle scene switching
		});
	}
}
