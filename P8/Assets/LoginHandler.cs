using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;

public static class LoginHandler {

	public static void LogIn(Credential credential, AlertDialog dialog) {
		Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

		auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
			if (task.IsCanceled) {
				return;
			}
			if (task.IsFaulted) {
				dialog.show("Login did not succeed.");
				return;
			}

			dialog.show("Logged in.");
		});
	}
}
