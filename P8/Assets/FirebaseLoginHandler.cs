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
	private const string mapSceneName = "Map";

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
			SceneManager.LoadScene(mapSceneName);
		});
	}

	private static void cleanUp(GameObject processIndicator, Selectable[] selectables) {
		// Enable selectables in the panel
		foreach (Selectable selectable in selectables) {
			selectable.interactable = true;
		}

		processIndicator.SetActive(false);
	}
}
