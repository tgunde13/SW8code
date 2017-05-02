using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpriteOnClick : MonoBehaviour {
	public Squad squad;

	void OnMouseDown(){
		this.gameObject.SetActive (false);
		DontDestroyOnLoad (this.gameObject);
		SceneManager.LoadScene(Constants.BattleSceneName);
	}
}
