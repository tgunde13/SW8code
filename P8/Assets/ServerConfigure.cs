using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Mgl;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ServerConfigure : MonoBehaviour {
	public InputField avatarField;
	public Text nameErrorText;
	public DialogPanel dialog;
	public GameObject processIndicator;
	public Selectable[] selectables;

	private TaskIndicator taskIndicator;
	private DatabaseReference responseRef;

	/// <summary>
	/// Requests the server to configure the avatar.
	/// If the device does not have internet access, an error dialog is shown.
	/// </summary>
	public void configure () {
		taskIndicator = new TaskIndicator (processIndicator, selectables);
		taskIndicator.OnStart ();

		nameErrorText.text = "";

		// Check internet connection
		StartCoroutine (InternetConnectionHelper.CheckInternetConnection ((isConnected) => {
			Debug.Log ("TOB: ServerConfigure, configure, isConnected: " + isConnected);
			if (!isConnected) {
				// Show error message, hide process indicator while message is shown
				dialog.show (I18n.Instance.__ ("ErrorInternet"), taskIndicator);
				return;
			}

			MakeRequest ();
		}));
	}

	/// <summary>
	/// Makes the request to configure.
	/// </summary>
	private void MakeRequest () {
		DatabaseReference taskRef = FirebaseDatabase.DefaultInstance
			.GetReference (Constants.FirebaseTasksNode);
		string userId = FirebaseAuthHandler.getUserId ();
		responseRef = taskRef.Child (Constants.FirebaseResponsesNode)
			.Child (userId);

		// Remove old reponse
		responseRef.RemoveValueAsync ()
			.ContinueWith (task1 => {
			if (task1.IsFaulted) {
				Debug.Log ("TOB: ServerConfigure, Remove old reponse IsFaulted");
				dialog.show (I18n.Instance.__ ("ErrorFirebase"), taskIndicator);
			} else if (task1.IsCompleted) {
				Debug.Log ("TOB: ServerConfigure, Remove old reponse IsCompleted");
				// Listen to response
				responseRef.ValueChanged += OnResponseChanged;

				// Make request object
				Dictionary<string, object> request = new Dictionary<string, object> ();
				request.Add (Constants.FirebaseStatusCodeNode, Constants.RequestCodeConfigure);
				request.Add (Constants.FirebaseDataNode, avatarField.text);

				// Request
				taskRef.Child (Constants.FirebaseRequestsNode)
						.Child (userId)
						.SetValueAsync (request)
						.ContinueWith (task2 => {
					if (task2.IsFaulted) {
						Debug.Log ("TOB: ServerConfigure, Request IsFaulted");
						dialog.show (I18n.Instance.__ ("ErrorFirebase"), taskIndicator);

						// Stop listening
						responseRef.ValueChanged -= OnResponseChanged;
					}
				});
			}
		});
	}

	/// <summary>
	/// Handles a change in response from the server.
	/// If response is OK, the map scene is loaded.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="args">Arguments.</param>
	void OnResponseChanged (object sender, ValueChangedEventArgs args) {
		// If no response exists, do nothing
		if (!args.Snapshot.Exists) {
			Debug.Log ("TOB: ServerConfigure, OnResponseChanged, !args.Snapshot.Exists");
			return;
		}

		long responseCode = (long)args.Snapshot.Child (Constants.FirebaseStatusCodeNode).GetValue (false);

		switch (responseCode) {
			case Constants.HttpOk:
				Debug.Log ("TOB: ServerConfigure, OnResponseChanged, HttpOk");
				SceneManager.LoadSceneAsync (Constants.MapSceneName);
				break;
			case Constants.HttpConflict:
				Debug.Log ("TOB: ServerConfigure, OnResponseChanged, HttpConflict");
				nameErrorText.text = I18n.Instance.__ ("ErrorAvatarNameConflict");
				avatarField.text = "";
				taskIndicator.OnEnd ();
				break;
			default:
				Debug.Log ("TOB: ServerConfigure, OnResponseChanged, default");
				dialog.show (I18n.Instance.__ ("ErrorFirebase"), taskIndicator);
				break;
		}

		// Stop listening
		responseRef.ValueChanged -= OnResponseChanged;

		// Remove response
		responseRef.RemoveValueAsync ();
	}
}
