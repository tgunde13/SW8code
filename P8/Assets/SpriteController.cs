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
		sprites = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Sets the initial latitude and longitude index and builds the zones.
	/// </summary>
	/// <param name="latitude">Current latitude.</param>
	/// <param name="longitude">Current longitude.</param>
	public void iniPosition(float latitude, float longitude){
		latitude_index = (int)Math.Floor(latitude * 100f);
		longitude_index = (int)Math.Floor(longitude * 100f);
		center_zone = new Zone (latitude_index, longitude_index, this);
		newZone ();
	}

	/// <summary>
	/// Updates the position and updates the references to firebase if needed.
	/// </summary>
	/// <param name="latitude">Current latitude.</param>
	/// <param name="longitude">Current longitude.</param>
	public void updatePosition(float latitude, float longitude){
		latitude_index = (int)Math.Floor(latitude * 100f);
		longitude_index = (int)Math.Floor(longitude * 100f);

		int latitude_change = center_zone.getLatitudeIndex () - latitude_index;
		int longitude_change = center_zone.getLongitudeIndex () - longitude_index;

		if (latitude_change != 0 || longitude_change != 0) {
			center_zone = new Zone (latitude_index, longitude_index, this);
			updateZone (latitude_change, longitude_change);
		}
	}

	/// <summary>
	/// Generates a new list and copies old references if they exists in the new list
	/// </summary>
	/// <param name="latitude_change">Latitude change.</param>
	/// <param name="longitude_change">Longitude change.</param>
	void updateZone(int latitude_change, int longitude_change){
		if (Math.Abs (latitude_change) > 2 || Math.Abs (longitude_change) > 2) {
			newZone ();
		} else {
			List<Zone> newZones = new List<Zone> ();
			addZonesToList (newZones);
			copyOldRef (newZones);
			addMissingRef ();
		}
	}

	/// <summary>
	/// Copies the old references from zones to the given list, if the coordinates are the same.
	/// </summary>
	/// <param name="newZones">List of the new zones.</param>
	void copyOldRef(List<Zone> newZones){
		foreach (Zone zone in zones) {
			foreach (Zone newZone in newZones) {
				if (zone.Equals(newZone)) {
					newZone.setMinionRef (zone.getMinionRef());
					zone.setMinionRef (null);
				}
			}
		}
		zones = newZones;
	}

	/// <summary>
	/// Adds the missing references to the zones list.
	/// </summary>
	void addMissingRef(){
		foreach (Zone zone in zones) {
			if (zone.getMinionRef () == null) {
				zone.setMinionRef (FirebaseDatabase.DefaultInstance.GetReference ("eSquads").Child (zone.getLatitudeIndex ().ToString ()).Child (zone.getLongitudeIndex ().ToString ()));
			}
		}
	}

	/// <summary>
	/// Rebuilds zones and squad listeners.
	/// </summary>
	void addZonesToList(){
		addZonesToList (zones, true);
	}

	/// <summary>
	/// Adds the zones to the given list, from the current latitude and longitude index.
	/// </summary>
	/// <param name="list">List to add zones to.</param>
	/// <param name="newZone">Should only be set to <c>true</c> if called from newZone.</param>
	void addZonesToList(List<Zone> list, bool newZone = false){
		for (int lat = latitude_index - 1; lat <= latitude_index + 1; lat++) {
			for (int lon = longitude_index - 1; lon <= longitude_index + 1; lon++) {
				if (newZone) {
					Zone z = new Zone (lat, lon, this, FirebaseDatabase.DefaultInstance.GetReference ("eSquads").Child (lat.ToString ()).Child (lon.ToString ()));
					list.Add (z);
				}
				else{
					Zone z = new Zone (lat, lon, this);
					list.Add (z);
				}
			}
		}
	}

	/// <summary>
	/// Clears zones List and rebuilds it from the new latitude and longitude index.
	/// </summary>
	void newZone(){
		zones.Clear();
		removeAllSprites ();
		addZonesToList();
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
		Vector3 pos = new Vector3 ((float)latitude, y_pos_of_sprites, (float)longitude);
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
	/// <param name="pos">Position of the minion as a Vector2.</param>
	/// <param name="name">Name of minion.</param>
	public void addSprite(Squad squad){
		Vector3 unity_pos = Mapbox.Scripts.Utilities.VectorExtensions.AsUnityPosition (squad.getPos());
		unity_pos.y = y_pos_of_sprites;
		GameObject sprite = minionTypeInstantiate(squad.getName());
		sprite.transform.name = ("Sprite - " + unity_pos.x + " | " + unity_pos.z);
		sprite.transform.position = unity_pos;
		//Debug.Log ("lat: " + squad.getPos().x + " lon: " + squad.getPos().y);
		Debug.Log ("Sprite: " + unity_pos.x + " | " + unity_pos.z);
		//add ref to the squad
		sprites.Add (sprite);
	}

	/// <summary>
	/// Instantiates the minion sprite to the given type.
	/// </summary>
	/// <returns>The type instantiate.</returns>
	/// <param name="minion">Name of minion wanted.</param>
	GameObject minionTypeInstantiate(string minion){
		switch (minion) {
			case "Cleric":
			case "Swordman":
			case "Spearmaiden":
		default:
			Debug.Log ("SpriteController: " + minion + " sprite not found");
			return Instantiate (spawn_sprite);
		}
	}
}
