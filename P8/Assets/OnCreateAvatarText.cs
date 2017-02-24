using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnCreateAvatarText : MonoBehaviour {
	public GameObject thisPanel;
	public InstantiateEmailCreatePanel panelToShow;
	public InputField emailField, passwordField;

	// Use this for initialization
	void Start () {
		
	}

	public void OnClick() {
		thisPanel.SetActive (false);
		panelToShow.instantiate (emailField.text, passwordField.text);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
