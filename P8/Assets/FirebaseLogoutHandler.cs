using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Firebase logout handler.
/// </summary>
public class FirebaseLogoutHandler : MonoBehaviour {
	private const string loginSceneName = "Login";

	/// <summary>
	/// Logs out.
	/// </summary>
	public void LogOut() {
		Firebase.Auth.FirebaseAuth.DefaultInstance.SignOut ();
		Debug.Log("TOB: LogoutHandler, logged out ");
		SceneManager.LoadScene(loginSceneName);
	}
}
