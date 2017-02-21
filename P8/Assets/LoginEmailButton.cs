using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;

public class LoginEmailButton : MonoBehaviour {

	public GameObject emailLoginPanel;
	public GameObject panel;

	// Use this for initialization
	void Start () {

	}

	public void onClick() {
		Debug.Log ("Email Button Click");

		this.panel.SetActive (false);
		emailLoginPanel.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
