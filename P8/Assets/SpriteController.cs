using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour {
	public GameObject spawn_sprite;
	// Use this for initialization
	void Start () {
		addSprite (0, 0, "");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Adds the sprite to the map, at given position.
	/// </summary>
	/// <param name="latitude">Latitude of the minion.</param>
	/// <param name="longitude">Longitude of the minion.</param>
	/// <param name="name">Name of minion.</param>
	public void addSprite(double latitude, double longitude, string name){
		addSprite ((new Vector2 ((float)latitude, (float)longitude)), name);
	}


	public void addSprite(Vector2 pos, string minion){
		Vector3 unity_pos = Mapbox.Scripts.Utilities.VectorExtensions.AsUnityPosition (pos);
		unity_pos.y = 490;
		GameObject sprite;
		sprite = Instantiate (spawn_sprite);
		sprite.transform.name = ("Sprite - " + unity_pos.x + " | " + unity_pos.z);
		sprite.transform.position = unity_pos;
	}
}
