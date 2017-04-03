using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mgl;

/// <summary>
/// E-mail address login handler.
/// </summary>
public class EmailLoginHandler : MonoBehaviour {
	public Text emailErrorText, passwordErrorText;
	public InputField emailField, passwordField;
	public DialogPanel dialog;
	public GameObject processIndicator;
	public Selectable[] selectables;

	/// <summary>
	/// Tries to log in with an e-mail address and a password.
	/// </summary>
	public void LogIn() {
		string email = emailField.text;
		string password = passwordField.text;

		// Validate input before call to Firebase
		bool validEmail = InputValidator.validateEmail(email, emailErrorText);
		bool validPassword = InputValidator.validatePasswordWithoutLength (password, passwordErrorText);

		if (!(validEmail && validPassword)) {
			return;
		}

		TaskIndicator indicator = new TaskIndicator (processIndicator, selectables);
		indicator.OnStart ();

		// Check internet connection
		StartCoroutine(InternetConnectionHelper.CheckInternetConnection((isConnected) => {
			if (!isConnected) {
				dialog.show(I18n.Instance.__ ("ErrorInternet"));
				indicator.OnEnd();
				return;
			}

			FirebaseLoginHandler.LogIn (Firebase.Auth.EmailAuthProvider.GetCredential (email, password), dialog, indicator);
		}));
	}
}
