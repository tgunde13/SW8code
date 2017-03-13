using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase;
using Firebase.Unity.Editor;
using UnityEngine.SceneManagement;

/// <summary>
/// Used for creating an avatar with an e-mail address.
/// </summary>
public class EmailCreateAvatarHandler : MonoBehaviour {
	private const string mapSceneName = "Map";

	public Text emailErrorText, passwordErrorText;
	public InputField emailField, passwordField1, passwordField2;
	public AlertDialog ShowErrorScript;
	public GameObject processIndicator;
	public Selectable[] selectables;

	public void create() {
		string email = emailField.text;
		string password1 = passwordField1.text;
		string password2 = passwordField2.text;

		// Validate input before call to Firebase
		bool validEmail = InputValidator.validateEmail(email, emailErrorText);
		bool validPassword = InputValidator.ValidatePasswordsCreate (password1, password2, passwordErrorText);

		if (!(validEmail && validPassword)) {
			return;
		}

		// Disable selectables in the panel
		foreach (Selectable selectable in selectables) {
			selectable.interactable = false;
		}

		// Show process indicator
		processIndicator.SetActive(true);

		FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

		auth.CreateUserWithEmailAndPasswordAsync(email, passwordField1.text).ContinueWith(task => {
			if (task.IsCanceled) {
				cleanUp();
				ShowErrorScript.show("Canceled...");
				return;
			}

			if (task.IsFaulted) {
				cleanUp();
				ShowErrorScript.show("Unknown error.");
				return;
			}

			Debug.Log("TOB: EmailCreateAvatarHandler, user created, logged in, user id: " + auth.CurrentUser.UserId);
			SceneManager.LoadScene(mapSceneName);
		});
	}

	private void cleanUp() {
		// Enable selectables in the panel
		foreach (Selectable selectable in selectables) {
			selectable.interactable = true;
		}

		processIndicator.SetActive(false);
	}
}
