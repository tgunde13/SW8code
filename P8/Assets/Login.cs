using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class Login : MonoBehaviour {

	// Use this for initialization
	void Start () {
		FirebaseApp.DefaultInstance..SetEditorDatabaseUrl("https://p8-server.firebaseio.com/");

		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
		reference.Child ("players").Push().SetValueAsync ("test");
	}

	// Update is called once per frame
	void Update () {

	}
}
