using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertDialog : MonoBehaviour {
	public Text messageField;

	// Use this for initialization
	void Start () {
		
	}

	public void show(string message) {
		messageField.text = message;
		gameObject.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
