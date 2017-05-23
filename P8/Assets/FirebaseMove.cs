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
	private bool isPickingMinion;

	/// <summary>
	/// Initializes a new instance of the <see cref="FirebaseMove"/> class.
	/// Should be used for attacking an opponent minion.
	/// </summary>
	/// <param name="battleKey">Battle key.</param>
	/// <param name="playerMinion">Player minion.</param>
	/// <param name="targetMinion">Target minion.</param>
	/// <param name="targetIndex">Target index.</param>
	public FirebaseMove (string battleKey, Minion playerMinion, Minion targetMinion, int targetIndex)	{
		this.battleKey = battleKey;
		this.playerMinion = playerMinion;
		this.targetMinion = targetMinion;
		this.targetIndex = targetIndex;
		this.isPickingMinion = false;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="FirebaseMove"/> class.
	/// Used for picking a minion.
	/// </summary>
	/// <param name="battleKey">Battle key.</param>
	/// <param name="playerMinion">Player minion.</param>
	/// <param name="targetIndex">Target index.</param>
	public FirebaseMove(string battleKey, Minion playerMinion, int targetIndex){
		this.battleKey = battleKey;
		this.playerMinion = playerMinion;
		this.targetMinion = null;
		this.targetIndex = targetIndex;
		this.isPickingMinion = true;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="FirebaseMove"/> class.
	/// Used when skipping an attack
	/// </summary>
	/// <param name="battleKey">Battle key.</param>
	/// <param name="playerMinion">Player minion.</param>
	public FirebaseMove(string battleKey, Minion playerMinion){
		this.battleKey = battleKey;
		this.playerMinion = playerMinion;
		this.targetMinion = null;
		this.targetIndex = -1;
		this.isPickingMinion = false;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="FirebaseMove"/> class.
	/// Used when skipping picking a minion
	/// </summary>
	/// <param name="battleKey">Battle key.</param>
	/// <param name="targetIndex">Target index.</param>
	public FirebaseMove(string battleKey, int targetIndex){
		this.battleKey = battleKey;
		this.playerMinion = null;
		this.targetIndex = targetIndex;
		this.targetMinion = null;
		this.isPickingMinion = true;
	}

	/// <summary>
	/// Starts uploading the move to Firebase.
	/// </summary>
	public void Start(){
		requestData = new Dictionary<string, object> ();
		if (isPickingMinion) {
			if (playerMinion != null) {
				requestData.Add ("avatarKey", FirebaseAuthHandler.getUserId ());
				requestData.Add ("minionKey", playerMinion.GetKey ());
			}
		} else if (targetIndex != -1) {
			requestData.Add ("avatarKey", targetMinion.GetKey ());
			requestData.Add ("minionKey", ("minion-" + targetIndex));
		}

		//Note that picking a minion has a diffent path
		if (isPickingMinion) {
			FirebaseDatabase.DefaultInstance.GetReference ("battles")
				.Child (battleKey)
				.Child("chosenMoves")
				.Child("moves")
				.Child(FirebaseAuthHandler.getUserId())
				.Child("minion-" + targetIndex)
				.SetValueAsync (requestData).ContinueWith(task => {
					Debug.Log("Started to set minion");
					if (task.IsFaulted) {
						Debug.Log("Failed to set minion data");
					}
					else if (task.IsCompleted) {
						Debug.Log("Minion data set correctly");
					}
				});
		} else {
			FirebaseDatabase.DefaultInstance.GetReference ("battles")
			.Child (battleKey)
			.Child ("chosenMoves")
			.Child ("moves")
			.Child (FirebaseAuthHandler.getUserId ())
			.Child (playerMinion.GetKey ())
			.SetValueAsync (requestData).ContinueWith (task => {
				Debug.Log ("Started to set data");
				if (task.IsFaulted) {
					Debug.Log ("Failed to set move data");
				} else if (task.IsCompleted) {
					Debug.Log ("Move data set correctly");
				}
			});
		}
	}

}

