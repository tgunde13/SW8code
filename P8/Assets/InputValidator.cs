using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

/// <summary>
/// Input validator.
/// </summary>
public static class InputValidator {
	// Pattern from https://www.codeproject.com/kb/recipes/emailregexvalidator.aspx
    public const string MatchEmailPattern = 
			@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
     + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
     + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
     + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$";
	private const int MinimumPasswordLength = 6;
	private const string NoEmailError = "Enter your e-mail address";
	private const string InvalidEmailError = "Not a valid e-mail address.";
	private const string NoPasswordError = "Enter a password."; 
	private const string PasswordNotStrongEnoughError = "The password must be at least 6 characters long";

	/// <summary>
	/// Validates the e-mail address.
	/// </summary>
	/// <returns><c>true</c>, if email was validated, <c>false</c> otherwise. The error text field is updated to the status of the validation.</returns>
	/// <param name="email">E-mail address.</param>
	/// <param name="errorText">Error text field.</param>
	public static bool validateEmail(string email, Text errorText) {
		if (email == "") {
			errorText.text = NoEmailError;
			return false;
		} else if (Regex.IsMatch(email, MatchEmailPattern)) {
			errorText.text = "";
			return true;
		} else {
			errorText.text = InvalidEmailError;
			return false;
		}
	}

	/// <summary>
	/// Validates the password.
	/// </summary>
	/// <returns><c>true</c>, if password was validated, <c>false</c> otherwise. The error text field is updated to the status of the validation.</returns>
	/// <param name="password">Password.</param>
	/// <param name="errorText">Error text field.</param>
	public static bool validatePassword(string password, Text errorText) {
		if (validatePasswordWithoutLength(password, errorText)) {
			return false;
		} else if (password.Length < MinimumPasswordLength) {
			errorText.text = PasswordNotStrongEnoughError;
			return false;
		} else {
			return true;
		}
	}

	/// <summary>
	/// Validates the password without checking for its length.
	/// </summary>
	/// <returns><c>true</c>, if password was validated, <c>false</c> otherwise. The error text field is updated to the status of the validation.</returns>
	/// <param name="password">Password.</param>
	/// <param name="errorText">Error text.</param>
	public static bool validatePasswordWithoutLength(string password, Text errorText) {
		if (password == "") {
			errorText.text = NoPasswordError;
			return false;
		} else {
			errorText.text = "";
			return true;
		}
	}
}
