﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Firebase.Database;
using Mapbox.Scripts.Utilities;

public class SpriteController : MonoBehaviour {
	public GameObject placeholderSprite;
	public GameObject playerSprite;
	public GameObject cleric;
	public GameObject spearmaiden;
	public GameObject swordman;
	public float yPosOfSprites = 5f;

	private List<Zone> zones;
	private int latitudeIndex;
	private int longitudeIndex;
	private Zone centerZone;

	// Use this for initialization
	void Start () {
		zones = new List<Zone> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Sets the initial latitude and longitude index and builds the zones.
	/// </summary>
	/// <param name="latitude">Current latitude.</param>
	/// <param name="longitude">Current longitude.</param>
	public void IniPosition(float latitude, float longitude){
		latitudeIndex = (int)Math.Floor(latitude * 100f);
		longitudeIndex = (int)Math.Floor(longitude * 100f);
		centerZone = new Zone (latitudeIndex, longitudeIndex, this);
		NewZone ();
	}

	/// <summary>
	/// Updates the position and updates the references to firebase if needed.
	/// </summary>
	/// <param name="latitude">Current latitude.</param>
	/// <param name="longitude">Current longitude.</param>
	public void UpdatePosition(float latitude, float longitude){
		latitudeIndex = (int)Math.Floor(latitude * 100f);
		longitudeIndex = (int)Math.Floor(longitude * 100f);

		int latitudeChange = centerZone.GetLatitudeIndex () - latitudeIndex;
		int longitudeChange = centerZone.GetLongitudeIndex () - longitudeIndex;

		if (latitudeChange != 0 || longitudeChange != 0) {
			centerZone = new Zone (latitudeIndex, longitudeIndex, this);
			UpdateZone (latitudeChange, longitudeChange);
		}
	}

	/// <summary>
	/// Generates a new list and copies old references if they exists in the new list
	/// </summary>
	/// <param name="latitude_change">Latitude change.</param>
	/// <param name="longitude_change">Longitude change.</param>
	void UpdateZone(int latitudeChange, int longitudeChange){
		if (Math.Abs (latitudeChange) > 2 || Math.Abs (longitudeChange) > 2) {
			NewZone ();
		} else {
			List<Zone> newZones = new List<Zone> ();
			AddZonesToList (newZones);
			CopyOldRef (newZones);
			AddMissingRef ();
		}
	}

	/// <summary>
	/// Copies the old references from zones to the given list, if the coordinates are the same.
	/// </summary>
	/// <param name="newZones">List of the new zones.</param>
	void CopyOldRef(List<Zone> newZones){
		foreach (Zone zone in zones) {
			foreach (Zone newZone in newZones) {
				if (zone.Equals(newZone)) {
					newZone.SetMinionRef (zone.GetMinionRef());
					zone.SetMinionRef (null);
				}
			}
		}
		zones = newZones;
	}

	/// <summary>
	/// Adds the missing references to the zones list.
	/// </summary>
	void AddMissingRef(){
		foreach (Zone zone in zones) {
			if (zone.GetMinionRef () == null) {
				zone.SetMinionRef (FirebaseDatabase.DefaultInstance.GetReference ("eSquads").Child (zone.GetLatitudeIndex ().ToString ()).Child (zone.GetLongitudeIndex ().ToString ()));
			}
		}
	}

	/// <summary>
	/// Rebuilds zones and squad listeners.
	/// </summary>
	void AddZonesToList(){
		AddZonesToList (zones, true);
	}

	/// <summary>
	/// Adds the zones to the given list, from the current latitude and longitude index.
	/// </summary>
	/// <param name="list">List to add zones to.</param>
	/// <param name="newZone">Should only be set to <c>true</c> if called from newZone.</param>
	void AddZonesToList(List<Zone> list, bool newZone = false){
		for (int lat = latitudeIndex - 1; lat <= latitudeIndex + 1; lat++) {
			for (int lon = longitudeIndex - 1; lon <= longitudeIndex + 1; lon++) {
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
	void NewZone(){
		zones.Clear();
		AddZonesToList();
	}

	/// <summary>
	/// Removes the sprite in the location provided.
	/// </summary>
	/// <param name="latitude">Latitude of sprite.</param>
	/// <param name="longitude">Longitude of sprite.</param>
	public void RemoveSprite(string key){
		GameObject g = GameObject.Find (key);
		GameObject.Destroy (g);
	}

	/// <summary>
	/// Adds the sprite to the map, at given position.
	/// </summary>
	/// <param name="pos">Position of the minion as a Vector2.</param>
	/// <param name="name">Name of minion.</param>
	public void AddSprite(Squad squad){
		Vector3 unity_pos = VectorExtensions.AsUnityPosition (squad.GetPos());
		unity_pos.y = yPosOfSprites;
		GameObject sprite = MinionTypeInstantiate(squad.GetName());
		sprite.transform.name = (squad.GetKey());
		sprite.transform.position = unity_pos;
		sprite.GetComponent<SpriteOnClick> ().squad = squad;
	}

	/// <summary>
	/// Instantiates the minion sprite to the given type.
	/// </summary>
	/// <returns>The type instantiate.</returns>
	/// <param name="minion">Name of minion wanted.</param>
	GameObject MinionTypeInstantiate(string minion){
		switch (minion) {
		case "Cleric":
			return Instantiate (cleric);
		case "Swordman":
			return Instantiate (swordman);
		case "Spearman":
			return Instantiate (spearmaiden);
		default:
			Debug.Log ("SpriteController: " + minion + " sprite not found");
			return Instantiate (placeholderSprite);
		}
	}
}
