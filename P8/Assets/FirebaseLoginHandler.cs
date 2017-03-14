using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
	public static void LogIn(Credential credential, AlertDialog dialog, GameObject processIndicator, Selectable[] selectables) {
		processIndicator.SetActive(true);

		Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

		auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
			if (task.IsCanceled) {
				Debug.Log("TOB: FirebaseLoginHandler, login canceled");
				cleanUp(processIndicator, selectables);
				return;
			}

			if (task.IsFaulted) {
				dialog.show("Login did not succeed.");
				Debug.Log("TOB: FirebaseLoginHandler, login faulted");
				cleanUp(processIndicator, selectables);
				return;
			}

			Debug.Log("TOB: FirebaseLoginHandler, login succeded");
			SceneManager.LoadScene(Constants.mapSceneName);
		});
	}

	/// <summary>
	/// Enables selectables and hides the process indicator
	/// </summary>
	/// <param name="processIndicator">Process indicator.</param>
	/// <param name="selectables">Selectables.</param>
	private static void cleanUp(GameObject processIndicator, Selectable[] selectables) {
		// Enable selectables in the panel
		foreach (Selectable selectable in selectables) {
			selectable.interactable = true;
		}

		processIndicator.SetActive(false);
	}
}
