using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FindNPCMinion : MonoBehaviour {
	public bool IsEnviormentBattle = false;
	public GameObject ComputerGameobject;

	// Use this for initialization
	void Start () {
		ComputerGameobject = GameObject.FindGameObjectWithTag ("Minion");
		if(ComputerGameobject != null){
			IsEnviormentBattle = true;
			ComputerGameobject.SetActive (false);
			Debug.Log (ComputerGameobject.name);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
