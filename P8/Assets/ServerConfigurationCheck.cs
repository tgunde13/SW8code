using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Mgl;
using UnityEngine.SceneManagement;

public class ServerConfigurationCheck : MonoBehaviour {
	public AlertDialog dialog;
	public GameObject processIndicator, startPanel;

	// Use this for initialization
	void Start () {
		FirebaseDatabase.DefaultInstance
			.GetReference (Constants.FirebasePlayersNode)
			.Child (FirebaseAuthHandler.getUserId ())
			.GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
					Debug.Log("TOB: ServerConfigurationCheck, player data fetching failed");
					dialog.show(I18n.Instance.__ ("ErrorFirebase"));
					processIndicator.SetActive(false);	
				} else if (task.IsCompleted) {
					DataSnapshot snapshot = task.Result;

					// Check if player data exists
					if (snapshot.Exists) {
						// There is player data, so switch scenes
						Debug.Log("TOB: ServerConfigurationCheck, player data exists");
						SceneManager.LoadScene(Constants.MapSceneName);
					} else {
						// No player data, so server has not configured the player
						Debug.Log("TOB: ServerConfigurationCheck, no player data");
						processIndicator.SetActive(false);
						startPanel.SetActive(true);
					}
				}
			});
	}
}
