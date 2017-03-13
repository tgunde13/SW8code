using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Input field that can have its content be inserted from another input field.
/// </summary>
public class InsertableInputField : InputField {
	/// <summary>
	/// Inserts from another field.
	/// </summary>
	/// <param name="field">Field to insert from.</param>
	public void InsertFrom(InputField field) {
		text = field.text;
	}
}
