using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mapbox.MeshGeneration;
using Mapbox.Scripts.Utilities;
using Mapbox;
using Mapbox.Map;
using Mapbox.Scripts.Utilities;

public class UpdatePosition : MonoBehaviour {
	
	public DialogPanel error;
	public MapController mapController;
	public int zoom = 18;
	public int range = 3;
	public Camera unityCamera;
	public SpriteController sprites;

	float currentLatitude;
	float currentLongitude;
	private bool updatingLocation = false;
	private Vector2 currentTile;
	private Vector2 cachedTile;
	private float cameraHeight = 500f;

	// Use this for initialization
	IEnumerator Start () {
		Input.location.Start();
		StartCoroutine (GetLocation ());
		yield return new WaitForSeconds(5);
		StartCoroutine (GetLocation ());
		yield return new WaitForSeconds(1);
		mapController.Execute(((double) currentLatitude), ((double) currentLongitude), zoom, range);
		yield return new WaitForSeconds (1);
		sprites.IniPosition (currentLatitude, currentLongitude);
		updatingLocation = false;
	}

	void Update () {
		if (!updatingLocation) {
			//Calls the functions for updating location and camera pos
			StartCoroutine (GetLocation ());
			GetNewMap ();
			sprites.UpdatePosition (currentLatitude, currentLongitude);
		}
	}

	/// <summary>
	/// Requests a new tile from the MapController, and moves camera to new location
	/// </summary>
	void GetNewMap(){
		Vector2 pos = new Vector2 (currentLatitude, currentLongitude);
		GetNewCameraPos (pos);
		//The rest of the method is based on Mapbox Slippy helper function
		currentTile = Conversions.MetersToTile (sprites.playerSprite.transform.position.ToVector2xz () + MapController.ReferenceTileRect.center, mapController.Zoom);
		if (currentTile != cachedTile) {
			for (int i = -range; i <= range; i++) {
				for (int j = -range; j <= range; j++) {
					mapController.Request (new Vector2 (currentTile.x + i, currentTile.y + j), mapController.Zoom);
				}
			}
			cachedTile = currentTile;
		}
		updatingLocation = false;
	}

	void GetNewCameraPos(Vector2 pos){
		//update camera pos
		Vector3 new_camera_pos = VectorExtensions.AsUnityPosition (pos);
		new_camera_pos.y = sprites.yPosOfSprites;
		sprites.playerSprite.transform.position = new_camera_pos;
		float spriteXPos = sprites.playerSprite.transform.position.x;
		float spriteZPos = sprites.playerSprite.transform.position.z;
		unityCamera.transform.position = 
			new Vector3(spriteXPos, cameraHeight, spriteZPos);
	}

	/// <summary>
	/// Updates the location from GPS (Works only on devices)
	/// </summary>
	IEnumerator GetLocation(){
		updatingLocation = true;
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
			currentLatitude = Input.location.lastData.latitude;
			currentLongitude = Input.location.lastData.longitude;
		}
	}
}
