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
	/// Show the specified message and indicates a pause with the indicator.
	/// When the OK button is touched, indicates an end with the indicator.
	/// </summary>
	/// <param name="message">Message.</param>
	/// <param name="indicator">Indicator.</param>
	public void show(string message, TaskIndicator indicator) {
		show (message, indicator.OnEnd);
		indicator.OnPause();
	}

	/// <summary>
	/// Show the specified message.
	/// Perform an action when the OK button is touched.
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
