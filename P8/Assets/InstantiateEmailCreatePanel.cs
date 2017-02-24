using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantiateEmailCreatePanel : MonoBehaviour {
	public InputField nameField, emailField, passwordField;

	// Use this for initialization
	void Start () {
		
	}

	public void instantiate(string email, string password) {
		nameField.text = "";
		emailField.text = email;
		passwordField.text = password;
		gameObject.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
