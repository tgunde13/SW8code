using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class FacebookLoginHandler : MonoBehaviour {
	public AlertDialog alertDialog;

	// Use this for initialization
	void Start () {
		
	}

	// Awake function from Unity's MonoBehavior
	void Awake ()
	{
		if (!FB.IsInitialized) {
			// Initialize the Facebook SDK
			FB.Init(InitCallback, OnHideUnity);
		} else {
			// Already initialized, signal an app activation App Event
			FB.ActivateApp();
		}
	}

	private void InitCallback ()
	{
		if (FB.IsInitialized) {
			// Signal an app activation App Event
			FB.ActivateApp();
			// Continue with Facebook SDK
			// ...
		} else {
			Debug.Log("TOB: Failed to Initialize the Facebook SDK");
		}
	}

	private void OnHideUnity (bool isGameShown)
	{
		if (!isGameShown) {
			// Pause the game - we will need to hide
			Time.timeScale = 0;
		} else {
			// Resume the game - we're getting focus again
			Time.timeScale = 1;
		}
	}

	private List<string> perms = new List<string>(){"public_profile", "email", "user_friends"};

	public void onFacebookLoginButtonClick() {
		FB.LogInWithReadPermissions(perms, AuthCallback);
	}

	private void AuthCallback (ILoginResult result) {
		if (FB.IsLoggedIn) {
			// AccessToken class will have session details
			var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
			// Print current access token's User ID
			Debug.Log("TOB: aToken.UserId: " + aToken.UserId);

			Debug.Log("TOB: aToken.TokenString: " + aToken.TokenString);

			// Print current access token's granted permissions
			foreach (string perm in aToken.Permissions) {
				Debug.Log(perm);
			}

			Firebase.Auth.Credential credential = Firebase.Auth.FacebookAuthProvider.GetCredential (aToken.TokenString);

			FirebaseLoginHandler.LogIn (credential, alertDialog);

		} else {
			Debug.Log("TOB: User cancelled login");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
