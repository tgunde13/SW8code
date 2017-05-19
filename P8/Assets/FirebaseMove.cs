using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;

public class FirebaseMove{
	private string battleKey;
	private Minion playerMinion;
	private Minion targetMinion;
	private int targetIndex;
	private Dictionary<string, object> requestData;

	public FirebaseMove (string battleKey, Minion playerMinion, Minion targetMinion, int targetIndex)	{
		this.battleKey = battleKey;
		this.playerMinion = playerMinion;
		this.targetMinion = targetMinion;
		this.targetIndex = targetIndex;
	}

	public FirebaseMove(string battleKey, Minion playerMinion){
		this.battleKey = battleKey;
		this.playerMinion = playerMinion;
		this.targetMinion = null;
		this.targetIndex = -1;
	}

	public void Start(){
		requestData = new Dictionary<string, object> ();
		if (targetIndex != -1) {
			requestData.Add ("avatarKey", targetMinion.getKey ());
			requestData.Add ("minionKey", ("minion-" + targetIndex));
		}

		FirebaseDatabase.DefaultInstance.GetReference ("battles")
			.Child (battleKey)
			.Child("chosenMoves")
			.Child("moves")
			.Child(FirebaseAuthHandler.getUserId())
			.Child(playerMinion.getKey())
			.SetValueAsync (requestData).ContinueWith(task => {
				Debug.Log("Started to set data");
				if (task.IsFaulted) {
					Debug.Log("Failed to set move data");
				}
				else if (task.IsCompleted) {
					Debug.Log("Move data set correctly");
				}
			});;
	}

}

