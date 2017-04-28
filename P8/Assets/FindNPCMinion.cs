using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindNPCMinion : MonoBehaviour {
	public bool IsEnviormentBattle = false;
	public Squad ComputerSquad;
	public string UserKey = FirebaseAuthHandler.getUserId ();

	private GameObject ComputerGameobject;
	//FirebaseAuthHandler.getUserId ();
	// Use this for initialization
	void Start () {
		ComputerGameobject = GameObject.FindGameObjectWithTag ("Minion");
		if(ComputerGameobject != null){
			ComputerSquad = ComputerGameobject.GetComponent<SpriteOnClick>().squad;
			IsEnviormentBattle = true;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
