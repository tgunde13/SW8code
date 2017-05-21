using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public class RewardScreenController : MonoBehaviour {
	private string battleKey;
	private List<Minion> playerMinionsUsed;
	private Minion awardedMinion;
	private GameObject rewardScreen;
	private List<object> gainedXP;
	private List<object> deadMinions;
	private long goldRecived;
	private long xpRecived = 0;
	private bool gotAwardedMinion = false;
	private DatabaseReference rewardRef;

	// Use this for initialization
	void Start () {
		playerMinionsUsed = new List<Minion> ();
		gainedXP = new List<object> ();
		deadMinions = new List<object> ();
		rewardScreen = gameObject.transform.Find ("Reward Screen").gameObject;
	}

	public void AwardReward(string battleKey, List<Minion> playerMinions, Minion opponentMinion){
		rewardScreen.SetActive (true);
		this.battleKey = battleKey;
		this.playerMinionsUsed = playerMinions;
		this.awardedMinion = opponentMinion;
		GetFirebaseReward ();
	}

	void GetFirebaseReward(){
		rewardRef = FirebaseDatabase.DefaultInstance.GetReference ("battles")
			.Child (battleKey)
			.Child (FirebaseAuthHandler.getUserId ());
		rewardRef.ValueChanged += handleValueChanged;
	}

	void handleValueChanged(object sender, ValueChangedEventArgs args){
		DataSnapshot snapshot = args.Snapshot;

		if (snapshot.HasChildren) {
			AssignFirebaseReward (snapshot);
			rewardRef.ValueChanged -= handleValueChanged;
		}
	}

	void AssignFirebaseReward(DataSnapshot reward){
		Debug.Log ("Looking at reward data");
		goldRecived = (long)reward.Child ("gold")
			.GetValue (false);
		Debug.Log (goldRecived);
		
		if (reward.Child ("deadMinions").Exists) {
			deadMinions = (List<object>)reward.Child ("deadMinions").GetValue (false);
		}

		if (reward.Child ("minion").Exists) {
			gotAwardedMinion = true;
		}

		if(reward.Child("usedMinions").Exists){
			gainedXP = (List<object>)reward.Child ("usedMinions").GetValue (false);
		}

		if (reward.Child ("xp").Exists && gainedXP.Count > 0) {
			xpRecived = (long)reward.Child ("xp").GetValue (false);
		}
		SetTextInRewardScreen ();
	}

	void SetTextInRewardScreen (){
		Debug.Log ("Seting text");
		Debug.Log ("Got awarded minion: " + gotAwardedMinion);
		Debug.Log ("Gold gained: " + goldRecived);
		if (gotAwardedMinion) {
			Debug.Log ("Minion in battle: " + gainedXP [0]);
		}
		Debug.Log ("Gained xp: " + xpRecived);
		if (deadMinions.Count > 0) {
			Debug.Log ("Dead minion: " + deadMinions [0]);
		}
	}
}
