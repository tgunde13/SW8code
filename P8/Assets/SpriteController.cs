using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour {
	public GameObject spawn_sprite;
	public float y_pos_of_sprites = 490f;

	private List<GameObject> sprites;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Removes all sprites on the map.
	/// </summary>
	public void removeAllSprites(){
		foreach (GameObject g in sprites) {
			Destroy (g);
		}
	}

	/// <summary>
	/// Removes the sprite in the location provided.
	/// </summary>
	/// <param name="latitude">Latitude of sprite.</param>
	/// <param name="longitude">Longitude of sprite.</param>
	public void removeSprite(double latitude, double longitude){
		Vector3 pos = new Vector3 ((float)latitude, (float)longitude, y_pos_of_sprites);
		foreach (GameObject g in sprites) {
			if (g.transform.position == pos) {
				Destroy (g);
				break;
			}
		}
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


	void addSprite(Vector2 pos, string minion){
		Vector3 unity_pos = Mapbox.Scripts.Utilities.VectorExtensions.AsUnityPosition (pos);
		unity_pos.y = y_pos_of_sprites;
		GameObject sprite = minionTypeInstantiate(minion);
		sprite.transform.name = ("Sprite - " + unity_pos.x + " | " + unity_pos.z);
		sprite.transform.position = unity_pos;
		sprites.Add (sprite);
	}

	/// <summary>
	/// Instantiates the minion sprite to the given type.
	/// </summary>
	/// <returns>The type instantiate.</returns>
	/// <param name="minion">Name of minion wanted.</param>
	GameObject minionTypeInstantiate(string minion){
		switch (minion) {
		default:
			Debug.Log ("SpriteController: " + minion + " sprite not found");
			return Instantiate (spawn_sprite);
		}
	}
}
