using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logout : MonoBehaviour {
	private const string loginSceneName = "Login";

	// Use this for initialization
	void Start () {
		
	}

	public void LogOut() {
		Firebase.Auth.FirebaseAuth.DefaultInstance.SignOut ();
		Debug.Log("TOB: Logout, logged out ");
		SceneManager.LoadScene(loginSceneName);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
