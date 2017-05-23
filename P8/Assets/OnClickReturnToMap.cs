using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OnClickReturnToMap : MonoBehaviour {
	public Button TheButton;

	void Start(){
		TheButton = gameObject.GetComponent<Button> ();
		TheButton.onClick.AddListener (LoadMap);
	}

	void LoadMap(){
		SceneManager.LoadScene (Constants.MapSceneName);
	}
}
