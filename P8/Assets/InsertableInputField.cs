using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsertableInputField : InputField {
	public void InsertFrom(InputField field) {
		text = field.text;
	}
}
