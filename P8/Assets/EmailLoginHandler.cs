using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// E-mail address login handler.
/// </summary>
public class EmailLoginHandler : MonoBehaviour {
	public Text emailErrorText, passwordErrorText;
	public InputField emailField, passwordField;
	public AlertDialog alertDialog;
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
			
		// Disable selectables in the panel
		foreach (Selectable selectable in selectables) {
			selectable.interactable = false;
		}

		FirebaseLoginHandler.LogIn (Firebase.Auth.EmailAuthProvider.GetCredential (email, password), alertDialog, processIndicator, selectables);
	}
}
