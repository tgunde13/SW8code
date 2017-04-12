using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpdatePosition : MonoBehaviour {
	
	public DialogPanel error;
	public int frames_before_update = 300;
	public Mapbox.MeshGeneration.MapController mapController;
	public int zoom = 18;
	public int range = 1;
	public Camera unity_camera;
	public SpriteController sprites;

	int frames_past = 0;
	float current_latitude;
	float current_longitude;
	Vector3 current_unity_pos;

	// Use this for initialization
	IEnumerator Start () {
		StartCoroutine (getLocation ());
		yield return new WaitForSeconds(10);
		mapController.Execute(((double) current_latitude), ((double) current_longitude), zoom, range);

	}

	void Update () {
		frames_past++;
		//Calls the functions for updating location and camera pos
		if (frames_past == frames_before_update){
			StartCoroutine (getLocation ());
			getNewMap ();
			frames_past = 0;
		}
	}

	/// <summary>
	/// Requests a new tile from the MapController, and moves camera to new location
	/// </summary>
	void getNewMap(){
		Vector2 pos = new Vector2 (current_latitude, current_longitude);
		mapController.Request (pos, zoom);

		//update camera pos
		Vector3 new_camera_pos = Mapbox.Scripts.Utilities.VectorExtensions.AsUnityPosition (pos);
		new_camera_pos.y = 500;
		unity_camera.transform.position = new_camera_pos;
	}

	/// <summary>
	/// Updates the location from GPS (Works only on devices)
	/// </summary>
	IEnumerator getLocation(){
		//Making sure location is enabled
		if (!Input.location.isEnabledByUser) {
			Debug.Log ("GetLocation 1: Location not enabled");
			error.show("Error: Please enable location.");
			yield break;
		}

		//Starting the location service
		Input.location.Start();

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
		
			Input.location.Stop ();
		}
	}
}
