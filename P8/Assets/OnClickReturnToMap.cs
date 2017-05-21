using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OnClickReturnToMap : MonoBehaviour {
	public Button TheButton;

	void Start(){
		TheButton = gameObject.GetComponent<Button> ();
		TheButton.onClick.AddListener (loadMap);
	}

	void loadMap(){
		SceneManager.LoadScene (Constants.MapSceneName);
	}
}
