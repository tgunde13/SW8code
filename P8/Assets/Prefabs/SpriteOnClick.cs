using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpriteOnClick : MonoBehaviour {
	public Squad squad;


	void OnMouseUpAsButton(){
		Input.location.Stop();
		DontDestroyOnLoad (transform.gameObject);
		SceneManager.LoadScene(Constants.BattleSceneName);
	}
}
