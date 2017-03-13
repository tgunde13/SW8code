using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

/// <summary>
/// Facebook login handler.
/// Some code is taken from https://developers.facebook.com/docs/unity/examples.
/// </summary>
public class FacebookLoginHandler : MonoBehaviour {
	public AlertDialog alertDialog;
	public GameObject processIndicator;
	public Selectable[] selectables;

	// Initialise Facebook SDK
	void Awake () {
		if (!FB.IsInitialized) {
			FB.Init(InitCallback, OnHideUnity);
		} else {
			// Already initialized, signal an app activation App Event
			FB.ActivateApp();
		}
	}

	/// <summary>
	/// Check status of initialisation.
	/// </summary>
	private void InitCallback () {
		if (FB.IsInitialized) {
			// Signal an app activation App Event
			FB.ActivateApp();
		} else {
			Debug.Log("TOB: Failed to Initialize the Facebook SDK");
		}
	}

	/// <summary>
	/// Pause or resumes game when Facebook tries to display HTML.
	/// </summary>
	/// <param name="isGameShown">If set to <c>true</c> the game is shown.</param>
	private void OnHideUnity (bool isGameShown) {
		if (!isGameShown) {
			// Pause the game - we will need to hide
			Time.timeScale = 0;
		} else {
			// Resume the game - we're getting focus again
			Time.timeScale = 1;
		}
	}

	private List<string> perms = new List<string>(){"public_profile", "email", "user_friends"};

	/// <summary>
	/// Starts Facebook login dialog.
	/// </summary>
	public void LogIn() {
		// Disable selectables in the panel
		foreach (Selectable selectable in selectables) {
			selectable.interactable = false;
		}

		FB.LogInWithReadPermissions(perms, AuthCallback);
	}

	/// <summary>
	/// Authentication callback.
	/// If succeed, tries to log in with Firebase.
	/// </summary>
	/// <param name="result">Result of Firebase login.</param>
	private void AuthCallback (ILoginResult result) {
		if (FB.IsLoggedIn) {
			// AccessToken class will have session details
			AccessToken aToken = AccessToken.CurrentAccessToken;
			// Print current access token's User ID
			Debug.Log("TOB: aToken.UserId: " + aToken.UserId);

			Debug.Log("TOB: aToken.TokenString: " + aToken.TokenString);

			// Print current access token's granted permissions
			foreach (string perm in aToken.Permissions) {
				Debug.Log(perm);
			}

			Firebase.Auth.Credential credential = Firebase.Auth.FacebookAuthProvider.GetCredential (aToken.TokenString);

			FirebaseLoginHandler.LogIn (credential, alertDialog, processIndicator, selectables);

		} else {
			Debug.Log("TOB: User cancelled login");

			// Enable selectables in the panel
			foreach (Selectable selectable in selectables) {
				selectable.interactable = true;
			}
		}
	}
}
