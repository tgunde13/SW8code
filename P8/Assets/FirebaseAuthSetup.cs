using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Firebase authentication setup.
/// Some code is taken from https://firebase.google.com/docs/auth/unity/manage-users
/// </summary>
public class FirebaseAuthSetup : MonoBehaviour {
	public GameObject processIndicator, startPanel;

	private Firebase.Auth.FirebaseAuth auth;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
		InitializeFirebase ();
	}

	// Handle initialization of the necessary firebase modules:
	private void InitializeFirebase() {
		Debug.Log("TOB: ---------------- FirebaseAuthSetup, setting up Firebase Auth ----------------");
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		auth.StateChanged += AuthStateChanged;
		AuthStateChanged(this, null);
	}

	// 
	/// <summary>
	/// If logged in, loads the server configuration scene.
	/// Otherwise, shows the first panel for logging in.
	/// Triggered when authentication state has changed.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="eventArgs">Event arguments.</param>
	void AuthStateChanged(object sender, System.EventArgs eventArgs) {
		if (auth.CurrentUser == null) {
			Debug.Log ("TOB: FirebaseAuthSetup, not logged in");
			processIndicator.SetActive(false);
			startPanel.SetActive (true);
		} else {
			Debug.Log("TOB: FirebaseAuthSetup, logged in, user id: " + auth.CurrentUser.UserId);
			SceneManager.LoadScene(Constants.ServerConfigurationSceneName);
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
