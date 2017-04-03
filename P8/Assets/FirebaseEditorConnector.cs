using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

/// <summary>
/// Firebase editor connector, so that the Unity Editor can use the Firebase datebase.
/// </summary>
public class FirebaseEditorConnector : MonoBehaviour {

	/// <summary>
	/// Connects the Unity editor to the Firebase database.
	/// </summary>
	void Start () {
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(Constants.FirebaseUrl);
	}
}
