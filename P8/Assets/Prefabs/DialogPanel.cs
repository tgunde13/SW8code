using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Alert dialog for showing messages to the user.
/// </summary>
public class DialogPanel : MonoBehaviour {
	public Text messageField;

	public Action action;

	/// <summary>
	/// Show the specified message.
	/// </summary>
	/// <param name="message">Message.</param>
	public void show(string message) {
		show (message, () => {
			// Do nothing
		});
	}

	/// <summary>
	/// Show the specified message.
	/// Perform an action when the OK button is clicked.
	/// </summary>
	/// <param name="message">Message.</param>
	/// <param name="action">Action.</param>
	public void show(string message, Action action) {
		messageField.text = message;
		gameObject.SetActive (true);

		this.action = action;
	}

	public void performAction() {
		action ();
	}
}
