using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Firebase authentication setup.
/// Some code is taken from https://firebase.google.com/docs/auth/unity/manage-users
/// </summary>
public class FirebaseAuthSetup : MonoBehaviour {
	Firebase.Auth.FirebaseAuth auth;

	// Use this for initialization
	void Start () {
		InitializeFirebase ();
	}

	// Handle initialization of the necessary firebase modules:
	private void InitializeFirebase() {
		Debug.Log("TOB: FirebaseAuthSetup, setting up Firebase Auth");
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		auth.StateChanged += AuthStateChanged;
		AuthStateChanged(this, null);
	}

	// Track state changes of the auth object.
	void AuthStateChanged(object sender, System.EventArgs eventArgs) {
		if (auth.CurrentUser == null) {
			Debug.Log ("TOB: FirebaseAuthSetup, not logged in");
			SceneManager.LoadScene (Constants.loginSceneName);			
		} else {
			Debug.Log("TOB: FirebaseAuthSetup, logged in, user id: " + auth.CurrentUser.UserId);
			SceneManager.LoadScene(Constants.mapSceneName);
		}
	}

	/// <summary>
	/// Cleans up.
	/// </summary>
	void OnDestroy() {
		auth.StateChanged -= AuthStateChanged;
		auth = null;
	}
}
