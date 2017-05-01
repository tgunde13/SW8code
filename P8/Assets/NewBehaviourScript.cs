using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour {
	public Button TheButton;

	void Start(){
		Button btn = TheButton.GetComponent<Button> ();
		btn.onClick.AddListener (loadMap);
	}

	void loadMap(){
		SceneManager.LoadScene (Constants.MapSceneName);
	}
}
