using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Mgl;
using UnityEngine.SceneManagement;

public class ServerConfigure : MonoBehaviour {
	public DialogPanel dialog;
	public GameObject processIndicator;

	public void configure () {
		processIndicator.SetActive (true);

		DatabaseReference taskRef = FirebaseDatabase.DefaultInstance
			.GetReference (Constants.FirebaseTasksNode);
		string userId = FirebaseAuthHandler.getUserId ();
		DatabaseReference responseRef = taskRef.Child (Constants.FirebaseResponsesNode)
			.Child (userId);

		// Remove old reponse
		responseRef.RemoveValueAsync ()
			.ContinueWith (task1 => {
			if (task1.IsFaulted) {
				Debug.Log ("TOB: ServerConfigure, Remove old reponse IsFaulted");
				dialog.show (I18n.Instance.__ ("ErrorFirebase"));
			} else if (task1.IsCompleted) {
				Debug.Log ("TOB: ServerConfigure, Remove old reponse IsCompleted");
				// Listen to response
				responseRef.ValueChanged += OnResponseChanged;

				// Request
				taskRef.Child (Constants.FirebaseRequestsNode)
						.Child (userId)
						.Child (Constants.FirebaseStatusCodeNode)
						.SetValueAsync (Constants.RequestCodeConfigure)
						.ContinueWith (task2 => {
					if (task2.IsFaulted) {
						Debug.Log ("TOB: ServerConfigure, Request IsFaulted");
						dialog.show (I18n.Instance.__ ("ErrorFirebase"));
					}
				});
			}
		});
	}

	void OnResponseChanged (object sender, ValueChangedEventArgs args) {
		Debug.Log ("TOB: ServerConfigure, OnResponseChanged");

		if (!args.Snapshot.Exists) {
			Debug.Log ("TOB: ServerConfigure, OnResponseChanged, !args.Snapshot.Exists");
			return;
		}

		long responseCode = (long)args.Snapshot.Child (Constants.FirebaseStatusCodeNode).GetValue (false);

		switch (responseCode) {
		case Constants.HttpOk:
			Debug.Log ("TOB: ServerConfigure, OnResponseChanged, HttpOk");
			SceneManager.LoadScene (Constants.MapSceneName);
			break;
		default:
			Debug.Log ("TOB: ServerConfigure, OnResponseChanged, default");
			processIndicator.SetActive (false);
			dialog.show (I18n.Instance.__ ("ErrorFirebase"));
			break;
		}
	}
}
