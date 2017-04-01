using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseAuthHandler {
	/// <summary>
	/// Gets the user identifier.
	/// Make sure to be logged in before calling this.
	/// </summary>
	/// <returns>The user identifier.</returns>
	public static string getUserId() {
		return Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser.UserId;
	}

}
