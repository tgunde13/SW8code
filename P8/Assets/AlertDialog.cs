using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Alert dialog for showing messages to the user.
/// </summary>
public class AlertDialog : MonoBehaviour {
	public Text messageField;

	/// <summary>
	/// Show the specified message.
	/// </summary>
	/// <param name="message">Message.</param>
	public void show(string message) {
		messageField.text = message;
		gameObject.SetActive (true);
	}
}
