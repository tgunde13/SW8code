using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopyToField : MonoBehaviour {
	public InputField field;

	// Use this for initialization
	void Start () {
		
	}

	public void copy(Text textToCopy) {
		field.text = textToCopy.text;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
