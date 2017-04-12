using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Firebase.Database;

public class SpriteController : MonoBehaviour {
	public GameObject spawn_sprite;
	public float y_pos_of_sprites = 490f;

	private List<GameObject> sprites;
	private List<Zone> zones;
	private int latitude_index;
	private int longitude_index;
	private Zone center_zone;

	// Use this for initialization
	void Start () {
		zones = new List<Zone> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void iniPosition(float latitude, float longitude){
		latitude_index = Math.Floor(latitude * 100f);
		longitude_index = Math.Floor(longitude * 100f);
		center_zone = new Zone (latitude_index, longitude_index);

		for (int lat = latitude_index - 1; lat <= latitude_index + 1; lat++) {
			for(int lon = longitude_index - 1; lon <= longitude_index + 1; lon++){
				Zone z = new Zone (lat, lon);
				zones.Add(z);
				FirebaseDatabase.DefaultInstance.GetReference ("eSquads").Child (z.getLatitudeIndex ().ToString ()).Child (z.getLongitudeIndex ().ToString ()).ChildAdded += handleChildAdded;
			}
		}
	}

	public void updatePosition(float latitude, float longitude){
		latitude_index = Math.Floor(latitude * 100f);
		longitude_index = Math.Floor(longitude * 100f);

		switch(center_zone.getLatitudeIndex() - latitude_index){
		case -1:
			
			switch(center_zone.getLongitudeIndex() - longitude_index){
			case -1:
				break;
			case 1:
				break;
			case 0:
				break;
			default:
				;
			}

			break;
		case 1:
			switch(center_zone.getLongitudeIndex() - longitude_index){
			case -1:
				break;
			case 1:
				break;
			case 0:
				break;
			default:
				;
			}
			break;
		case 0:
			switch(center_zone.getLongitudeIndex() - longitude_index){
			case -1:
				break;
			case 1:
				break;
			case 0: //Same center do nothing
				break;
			default:
				;
			}
			break;
		default:
			;
		}
	}

	void updateZone(int latitude_index, int longitude_index){
		List<Zone> new_zones = new List<Zone> ();

		for (int lat = latitude_index - 1; lat <= latitude_index + 1; lat++) {
			for(int lon = longitude_index - 1; lon <= longitude_index + 1; lon++){
				new_zones.Add(new Zone (lat, lon));
			}
		}

		foreach (Zone z in zones) {

		}
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

	void handleChildAdded(object sender, ChildChangedEventArgs args){
		args.Snapshot.Child("currentHealth").GetValue;//Make it a squad
		//draw squad on card
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
