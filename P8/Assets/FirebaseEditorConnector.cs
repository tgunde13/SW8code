using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class FirebaseEditorConnector : MonoBehaviour {

	// Use this for initialization
	void Start () {
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://league-of-locations-26618308.firebaseio.com");

		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
		reference.Child ("players").Push().SetValueAsync ("test2");
	}

	// Update is called once per frame
	void Update () {

	}
}
