using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;

public class RewardScreenController : MonoBehaviour {
	private string battleKey;
	private List<Minion> playerMinionsUsed;
	private Minion awardedMinion;
	private GameObject rewardScreen;
	private GameObject titleText;
	private GameObject battleOverText;
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
		titleText = rewardScreen.transform.Find ("Over Text").gameObject;
		battleOverText = rewardScreen.transform.Find ("Text").gameObject;
	}

	public void AwardReward(string battleKey, List<Minion> playerMinions, Minion opponentMinion){
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
		Debug.Log ("Setting text");
		string battleText = "";

		if (gainedXP.Count > 0) {
			titleText.GetComponent<Text> ().text = "Victory!";
		} else {
			titleText.GetComponent<Text> ().text = "Defeat!";
		}

		battleText += "Recived " + goldRecived + " gold\n\n";

		if (deadMinions.Count > 0) {
			battleText += "Following minions are dead:\n";
			foreach (object m in deadMinions) {
				battleText += ((string)m) + "\n";
			}
			battleText += "\n";
		}
			
		if (gainedXP.Count > 0) {
			battleText += "Following minions gained " + xpRecived + " xp:\n";
			foreach (object m in gainedXP) {
				battleText += ((string)m) + "\n";
			}
			battleText += "\n";
		}
			
		if (gotAwardedMinion) {
			battleText += "Gained minion:\n";
			battleText += awardedMinion.ToString ();
		}

		battleOverText.GetComponent<Text> ().text = battleText;
		rewardScreen.SetActive (true);
	}
}
