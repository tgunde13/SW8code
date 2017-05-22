using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mapbox.Unity.MeshGeneration;
using Mapbox.Unity.Utilities;
using Mapbox;
using Mapbox.Map;
using Mapbox.Utils;

public class UpdatePosition : MonoBehaviour {
	
	public DialogPanel error;
	public MapController mapController;
	public int zoom = 18;
	public int range = 3;
	public Camera unity_camera;
	public SpriteController sprites;
	public GameObject progress_indicatior;

	float current_latitude;
	float current_longitude;
	Vector3 current_unity_pos;
	private bool updating_location = false;
	private Vector2 current_tile;
	private Vector2 cached_tile;

	// Use this for initialization
	IEnumerator Start () {
		Input.location.Start();
		StartCoroutine (getLocation ());
		yield return new WaitForSeconds(5);
		StartCoroutine (getLocation ());
		yield return new WaitForSeconds(1);
		mapController.Execute(((double) current_latitude), ((double) current_longitude), zoom, range);
		yield return new WaitForSeconds (1);
		sprites.iniPosition (current_latitude, current_longitude);
		updating_location = false;
		progress_indicatior.SetActive(false);
	}

	void Update () {
		if (!updating_location) {
			//Calls the functions for updating location and camera pos
			StartCoroutine (getLocation ());
			getNewMap ();
			sprites.updatePosition (current_latitude, current_longitude);
		}
	}

	/// <summary>
	/// Requests a new tile from the MapController, and moves camera to new location
	/// </summary>
	void getNewMap(){
		Vector2 pos = new Vector2 (current_latitude, current_longitude);
		Vector2d pos2 = new Vector2d(0, 0);
		getNewCameraPos (pos, pos2);
		//The rest of the method is based on Mapbox Slippy helper function
		current_tile = Conversions.MetersToTile (sprites.player_sprite.transform.position.ToVector2d () + MapController.ReferenceTileRect.Center, mapController.Zoom);
		if (current_tile != cached_tile) {
			for (int i = -range; i <= range; i++) {
				for (int j = -range; j <= range; j++) {
					mapController.Request (new Vector2 (current_tile.x + i, current_tile.y + j), mapController.Zoom);
				}
			}
			cached_tile = current_tile;
		}
		updating_location = false;
	}

	void getNewCameraPos(Vector2 pos, Vector2d pos2){
		//update camera pos
		Vector3 new_camera_pos = Mapbox.Unity.Utilities.VectorExtensions.AsUnityPosition (pos, pos2, (float)2.5);
		new_camera_pos.y = sprites.y_pos_of_sprites;
		sprites.player_sprite.transform.position = new_camera_pos;
		unity_camera.transform.position = new Vector3(sprites.player_sprite.transform.position.x, 500f, sprites.player_sprite.transform.position.z);
	}

	/// <summary>
	/// Updates the location from GPS (Works only on devices)
	/// </summary>
	IEnumerator getLocation(){
		updating_location = true;
		//Making sure location is enabled
		if (!Input.location.isEnabledByUser) {
			Debug.Log ("GetLocation 1: Location not enabled");
			error.show("Error: Please enable location.");
			yield break;
		}

		//Starting the location service
		if (Input.location.status != LocationServiceStatus.Running) {
			Input.location.Start ();
		}

		//Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0){
			//Debug.Log ("GetLocation 0: Location not yet retrived waited for {0} secounds", (maxWait + 20));
			yield return new WaitForSeconds(1);
			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1){
			Debug.Log ("GetLocation 2: Location service didn't start");
			error.show("Location service didn't start");
			yield break;
		}

		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed) {
			Debug.Log ("GetLocation 3: Unable to determine device location");
			error.show ("Unable to determine device location");
			yield break;
		} else {
			// Access granted and location value could be retrieved
			current_latitude = Input.location.lastData.latitude;
			current_longitude = Input.location.lastData.longitude;
		}
	}
}
