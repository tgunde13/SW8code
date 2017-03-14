using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

/// <summary>
/// Input validator.
/// </summary>
public static class InputValidator {

	/// <summary>
	/// Validates the e-mail address.
	/// </summary>
	/// <returns><c>true</c>, if email was validated, <c>false</c> otherwise. The error text field is updated to the status of the validation.</returns>
	/// <param name="email">E-mail address.</param>
	/// <param name="errorText">Error text field.</param>
	public static bool validateEmail(string email, Text errorText) {
		if (email == "") {
			errorText.text = I18nManager.GetInstance ().GetLocalisedString ("NoEmailError");
			return false;
		} else if (Regex.IsMatch(email, Constants.MatchEmailPattern)) {
			errorText.text = "";
			return true;
		} else {
			errorText.text = I18nManager.GetInstance ().GetLocalisedString ("InvalidEmailError");
			return false;
		}
	}

	/// <summary>
	/// Validates the input of two password fields when creating an avatar.
	/// </summary>
	/// <returns><c>true</c>, if the passwords was validated, <c>false</c> otherwise. The error text field is updated to the status of the validation.</returns>
	/// <param name="password1">The first password.</param>
	/// <param name="password2">The second password.</param>
	/// <param name="errorText">Error text.</param>
	public static bool ValidatePasswordsCreate(string password1, string password2, Text errorText) {
		if (password1 == "") {
			errorText.text = I18nManager.GetInstance ().GetLocalisedString ("NoPasswordError");
			return false; 
		}

		if (password1.Length < Constants.MinimumPasswordLength) {
			errorText.text = I18nManager.GetInstance ().GetLocalisedString ("PasswordNotStrongEnoughError");
			return false;
		}

		if (password1 != password2) {
			errorText.text = I18nManager.GetInstance ().GetLocalisedString ("PasswordsNotSameError");
			return false;
		}

		errorText.text = "";
		return true;
	}

	/// <summary>
	/// Validates the password without checking for its length.
	/// </summary>
	/// <returns><c>true</c>, if password was validated, <c>false</c> otherwise. The error text field is updated to the status of the validation.</returns>
	/// <param name="password">Password.</param>s
	/// <param name="errorText">Error text.</param>
	public static bool validatePasswordWithoutLength(string password, Text errorText) {
		if (password == "") {
			errorText.text = I18nManager.GetInstance().GetLocalisedString ("NoPasswordError");
			return false;
		} else {
			errorText.text = "";
			return true;
		}
	}
}
