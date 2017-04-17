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

		nameErrorText.text = "";

		// Make request object
		Dictionary<string, object> request = new Dictionary<string, object> ();
		request.Add (Constants.FirebaseStatusCodeNode, Constants.RequestCodeConfigure);
		request.Add (Constants.FirebaseDataNode, avatarField.text);

		// Check internet connection
		new Request(this, taskIndicator, dialog, HandleResponse, request).Start();
	}

	/// <summary>
	/// Handles the response.
	/// </summary>
	/// <returns><c>true</c>, if response was handled, <c>false</c> otherwise.</returns>
	/// <param name="snapshot">Firebase snapshot.</param>
	bool HandleResponse(DataSnapshot snapshot) {
		long responseCode = (long)snapshot.Child (Constants.FirebaseStatusCodeNode).GetValue (false);

		switch (responseCode) {
			case Constants.HttpOk:
				Debug.Log ("TOB: ServerConfigure, OnResponseChanged, HttpOk");
				SceneManager.LoadSceneAsync (Constants.MapSceneName);
				return true;
			case Constants.HttpConflict:
				Debug.Log ("TOB: ServerConfigure, OnResponseChanged, HttpConflict");
				nameErrorText.text = I18n.Instance.__ ("ErrorAvatarNameConflict");
				avatarField.text = "";
				taskIndicator.OnEnd ();
				return true;
			default:
				return false;
		}
	}
}
