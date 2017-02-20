using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;

public class LoginEmailButton : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	public void onClick() {
		Debug.Log ("Email Button Click");

		Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
