using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase;
using Firebase.Unity.Editor;
using UnityEngine.SceneManagement;
using Mgl;

/// <summary>
/// Used for creating an avatar with an e-mail address.
/// </summary>
public class EmailCreateAvatarHandler : MonoBehaviour {
	public Text emailErrorText, passwordErrorText;
	public InputField emailField, passwordField1, passwordField2;
	public DialogPanel dialog;
	public GameObject processIndicator;
	public Selectable[] selectables;

	/// <summary>
	/// Create an avatar with an e-mail address.
	/// </summary>
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

		TaskIndicator indicator = new TaskIndicator (processIndicator, selectables);
		indicator.OnStart ();

		// Check internet connection
		StartCoroutine(InternetConnectionHelper.CheckInternetConnection((isConnected) => {
			if (!isConnected) {
				dialog.show(I18n.Instance.__ ("ErrorInternet"), () => indicator.OnEnd());
				indicator.OnPause();
				return;
			}

			FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
			auth.CreateUserWithEmailAndPasswordAsync(email, passwordField1.text).ContinueWith(task => {
				if (task.IsCanceled || task.IsFaulted) {
					dialog.show(I18n.Instance.__ ("ErrorUnknown"), () => indicator.OnEnd());
					indicator.OnPause();
					return;
				}

				Debug.Log("TOB: EmailCreateAvatarHandler, user created, logged in, user id: " + auth.CurrentUser.UserId);
				// Firebase Auth Setup should handle scene switching
			});
		}));
	}
}
